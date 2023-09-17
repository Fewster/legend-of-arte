using Game.Framework;
using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class AbilityComponent : GameBehaviour
{
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

    public Entity Entity { get; private set; }
    public Abilities Abilities { get; private set; }

    protected override void Setup()
    {
        Entity = GetComponentInParent<Entity>();
        Abilities = GetComponentInParent<Abilities>();
        runner = Resolver.Resolve<CoroutineRunner>();

        Abilities.Add(this);
    }

    protected override void Cleanup()
    {
        if (cts != null)
        {
            cts.Cancel();
            cts.Dispose();
        }
    }

    // Unity event support
    public void CastAbility()
    {
        Cast();
    }

    public virtual CastResult Cast()
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
                    _ = CastRoutine(cts.Token);

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
        if (Phase == CastPhase.Casting && cts != null)
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

    private async Task CastRoutine(CancellationToken cancellationToken)
    {
        Phase = CastPhase.Casting;

        try
        {
            // hack
            runner = Entity.Resolver.Resolve<CoroutineRunner>();

            await OnCast(cancellationToken);

            Phase = CastPhase.Cooldown;

            await WaitFor(Cooldown);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
        finally
        {
            Phase = CastPhase.None;
        }
    }

    [ContextMenu("Cast")]
    private void ManualCast()
    {
        Cast();
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

    protected virtual Task OnCast(CancellationToken cancellation) { return Task.CompletedTask; }
    protected virtual void OnCancel() { }
    protected virtual void OnBind() { }
}
