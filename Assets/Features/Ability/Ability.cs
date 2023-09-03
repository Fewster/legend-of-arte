using Game.Framework;
using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public struct CastResult
{
    public CastStatus Status { get; set; }
    public CastError Error { get; set; }
}

public enum CastStatus
{
    Failure,
    Successful,
}

[Flags]
public enum CastError
{
    None = 0,
    InsufficientResource = 1,
    AlreadyCasting = 2,
    CoolingDown = 4
}

public enum CastPhase
{
    None,
    Casting,
    Cooldown
}

public abstract class Ability : ScriptableObject
{
    private Task task;
    private CoroutineRunner runner;
    private CancellationTokenSource cts;

    [SerializeField]
    [Min(0.0f)]
    protected float cooldown;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    protected float progress;

    public float Cooldown
    {
        get
        {
            return cooldown;
        }
    }

    public float Progress
    {
        get
        {
            return progress;
        }
    }

    public CastPhase Phase
    {
        get; protected set;
    }

    private void OnDestroy()
    {
        if (cts != null)
        {
            cts.Cancel();
            cts.Dispose();
        }
    }

    public virtual CastResult Cast(Entity caster)
    {
        switch (Phase)
        {
            case CastPhase.None:
                {
                    var error = ValidateCast();
                    if (error != CastError.None) // Handle validation failure
                    {
                        return new CastResult()
                        {
                            Error = error,
                            Status = CastStatus.Failure
                        };
                    }

                    if (cts != null)
                    {
                        cts.Cancel();
                        cts.Dispose();
                    }

                    cts = new CancellationTokenSource();
                    task = CastRoutine(caster, cts.Token);

                    return new CastResult()
                    {
                        Error = CastError.None,
                        Status = CastStatus.Successful
                    };
                }
            case CastPhase.Casting:
                {
                    return new CastResult()
                    {
                        Error = CastError.AlreadyCasting,
                        Status = CastStatus.Failure
                    };
                }
            case CastPhase.Cooldown:
                {
                    return new CastResult()
                    {
                        Error = CastError.CoolingDown,
                        Status = CastStatus.Failure
                    };
                }
            default:
                throw new Exception($"Invalid cast phase");
        }
    }

    public void Cancel()
    {
        if(Phase == CastPhase.Casting && cts != null)
        {
            cts.Cancel();
            cts.Dispose();
            OnCancel();
        }
    }

    protected virtual CastError ValidateCast()
    {
        return CastError.None;
    }

    private async Task CastRoutine(Entity caster, CancellationToken cancellationToken)
    {
        Phase = CastPhase.Casting;

        try
        {
            // hack
            runner = caster.Resolver.Resolve<CoroutineRunner>();

            await OnCast(caster, cancellationToken);

            Phase = CastPhase.Cooldown;

            await WaitFor(Cooldown);
        }
        catch(Exception ex)
        {
            Debug.LogException(ex);
        }
        finally
        {
            Phase = CastPhase.None;
        }
    }

    protected Task WaitFor(float cooldown)
    {
        var routine = WaitRoutine(cooldown);
        return runner.Run(routine);
    }

    private IEnumerator WaitRoutine(float cooldown)
    {
        if (cooldown <= 0.0f)
        {
            yield break;
        }

        yield return new WaitForSeconds(cooldown);
    }

    protected virtual Task OnCast(Entity caster, CancellationToken cancellation) { return Task.CompletedTask; }
    protected virtual void OnCancel() { }
    protected virtual void OnBind() { }
}
