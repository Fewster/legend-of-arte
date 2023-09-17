using Game.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class ProjectileAbility : AbilityComponent
{
    private IGameSpace gameSpace;

    public LayerMask Layer;
    public GameObject Projectile;
    public float SpawnDistance;

    protected override void Setup()
    {
        base.Setup();

        gameSpace = Resolver.Resolve<IGameSpace>();
    }

    protected override Task OnCast(CancellationToken cancellation)
    {
        var origin = (Vector2)Entity.transform.position;
        var mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        var dir = (mousePosition - origin).normalized;

        var instance = Instantiate(Projectile);
        instance.transform.position = origin + dir * SpawnDistance;
        instance.transform.forward = dir;
        instance.transform.parent = gameSpace.GetGameObject().transform;

        return Task.CompletedTask;
    }
}
