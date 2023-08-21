using Game.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Melee")]
public class MeleeAbility : Ability
{
    private List<Entity> entities;

    public int Damage = 10;
    public float Radius = 1.0f;
    public LayerMask Mask;

    public float ConeAngle = 30.0f;

    public float minAng = 30.0f;
    public float maxAng = 30.0f;

    private void OnEnable()
    {
        entities = new List<Entity>();
    }

    public override void Cast(Entity caster)
    {
        var mapSpace = caster.Resolver.Resolve<MapSpace>();

        var pos = caster.transform.position;
        var checkPoint = new Vector2(pos.x, pos.y);

        GatherNearbyEntities(checkPoint, caster, mapSpace);

        foreach (var entity in entities)
        {
            var damageable = entity.GetComponent<IDamageable>();
            if (damageable == null)
            {
                continue;
            }

            damageable.TakeDamage(Damage);
        }

        if (entities.Count > 0)
        {
            DrawIsoCircle(checkPoint, 32, Radius, Color.green, mapSpace);
        }
        else
        {
            DrawIsoCircle(checkPoint, 32, Radius, Color.red, mapSpace);
        }
    }

    /// <summary>
    /// Compute the normalized angle of aim relative to the given cone.
    /// Returns less than 0 if the aim vector is to the left side.
    /// Returns greater than 1 if the aim vector is to the right side.
    /// Any value outside of the 0-1 range is considered outside the cone.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="aim"></param>
    /// <returns></returns>
    private static float NormalizedAngle(Vector2 left, Vector2 right, Vector2 aim)
    {
        var funnelAngle = Vector2.Angle(left, right);
        var checkAngle = Vector2.SignedAngle(left, aim);
        return checkAngle / funnelAngle;
    }

    private void GatherNearbyEntities(Vector2 position, Entity source, MapSpace mapSpace)
    {
        entities.Clear();

        // Compute aim angles
        var worldAimDir = source.Direction.ToDirection();
        var worldMinDir = (Quaternion.Euler(0.0f, 0.0f, -ConeAngle) * worldAimDir);
        var worldMaxDir = (Quaternion.Euler(0.0f, 0.0f, ConeAngle) * worldAimDir);

        // Convert poses to map space. This produces cone directions which map correctly to the world grid.
        var mapMinDir = mapSpace.ToMap(worldMinDir);
        var mapMaxDir = mapSpace.ToMap(worldMaxDir);

        Debug.DrawLine(position, position + mapMinDir * Radius, Color.white);
        Debug.DrawLine(position, position + mapMaxDir * Radius, Color.white);

        var hits = Physics2D.OverlapCircleAll(position, Radius, Mask);
        foreach (var hit in hits)
        {
            var hitEntity = hit.GetComponent<Entity>();
            if (hitEntity == null || hitEntity == source) // Don't self-hit or attack NULL entities
            {
                continue;
            }

            var checkPoint = (Vector2)hit.transform.position;

            var dir = (checkPoint - position); // TODO: Why does a non-normalized value work, but a normalized value not.
            var normalizedAngle = NormalizedAngle(mapMinDir, mapMaxDir, dir);

            // If the angle is outside the cone, ignore it.
            if (normalizedAngle < 0.0f || normalizedAngle > 1.0f)
            {
               // continue;
            }

            var entitySize = hitEntity.Size * 0.5f;

            // FLAW: Seems that there are some cases where the Left/Right points of the entity radius
            // are not correctly mapped. This will be due to the none-equally spaced cone angle.
            // The left/right cone vectors are not guaranteed to be the same angle relative to the aim direction.
            // We may need to project the circle edges onto a radius around the player (basically a curve) to
            // get pixel perfect circle collisions.

            var right = ((Vector2)Vector3.Cross(dir.normalized, Vector3.forward)).normalized;
            var e1 = checkPoint + (right * entitySize);
            var e2 = checkPoint - (right * entitySize);

            Debug.DrawLine(position, e1, Color.yellow);
            Debug.DrawLine(position, e2, Color.yellow);

            var heading1 = (e1 - position).normalized;
            var heading2 = (e2 - position).normalized;

            var ang1 = NormalizedAngle(mapMinDir, mapMaxDir, heading1);
            var ang2 = NormalizedAngle(mapMinDir, mapMaxDir, heading2);

            // If both points are not intersecting, the entity is not in the cone.
            if((ang1 < 0.0f || ang1 > 1.0f) && (ang2 < 0.0f || ang2 > 1.0f))
            {
                continue;
            }

            // If the target is outside the entity radius (skewed into map space), ignore it.
            var heading = mapSpace.GetMapDirection(dir) * Radius;     
            if((dir.magnitude - entitySize) > heading.magnitude)
            {
                continue;
            }

            Debug.DrawLine(position, position + heading, Color.magenta);

            //Debug.DrawLine(position, checkPoint, Color.green);

            entities.Add(hitEntity);
        }
    }

    private void DrawIsoCircle(Vector2 position, int segments, float radius, Color color, MapSpace mapSpace)
    {
        var mod = 360.0f / segments;

        for (int i = 0; i < segments; i++)
        {
            var a = position + (mapSpace.GetMapDirection((i - 1) * mod) * radius);
            var b = position + (mapSpace.GetMapDirection((i) * mod) * radius);

            //var a = position + new Vector2(radius * Mathf.Cos((i - 1) * mod), radius * Mathf.Sin((i - 1) * mod));
            //var b = position + new Vector2(radius * Mathf.Cos(i * mod), radius * Mathf.Sin(i * mod));

            Debug.DrawLine(a, b, color);
        }
    }
}
