using Game.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Projectile")]
public class ProjectileAbility : Ability
{
    public LayerMask Layer;
    public GameObject Projectile;
    public float SpawnDistance;

    public override void Cast(Entity caster)
    {
        var gameSpace = caster.Resolver.Resolve<IGameSpace>();

        var origin = (Vector2)caster.transform.position;
        var mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        var dir = (mousePosition - origin).normalized;

        var instance = Instantiate(Projectile);
        instance.transform.position = origin + dir * SpawnDistance;
        //.transform.forward = dir;
        instance.transform.parent = gameSpace.GetGameObject().transform;
    }
}
