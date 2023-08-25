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

    [Range(0.0f, 90.0f)] // TODO: Consider having a minimum limitation of 22.5 to ensure no directional blindspots
    public float ConeAngle = 30.0f;

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
            var entitySize = entity.Size * 0.5f;
            DebugExtensions.DrawCircle(entity.transform.position, entitySize, Color.green, 0);

            var damageable = entity.GetComponent<IDamageable>();
            if (damageable == null)
            {
                continue;
            }

            damageable.TakeDamage(Damage);
        }

        if (entities.Count > 0)
        {
            DrawIsoCircle(checkPoint, 128, Radius, Color.green, mapSpace);
        }
        else
        {
            DrawIsoCircle(checkPoint, 128, Radius, Color.red, mapSpace);
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

    private Vector2 ClosestPoint(Vector2 a, Vector2 b, Vector2 p)
    {
        var dir = p - a;
        var line = b - a;

        var t = Vector2.Dot(dir, line) / line.sqrMagnitude;
        t = Mathf.Clamp01(t);

        return a + t * line;
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

        var hits = Physics2D.OverlapCircleAll(position, Radius, Mask);
        foreach (var hit in hits)
        {
            var hitEntity = hit.GetComponent<Entity>();
            if (hitEntity == null || hitEntity == source) // Don't self-hit or attack NULL entities
            {
                continue;
            }

            var checkPoint = (Vector2)hit.transform.position;
            var entitySize = hitEntity.Size * 0.5f;

            DebugExtensions.DrawCircle(checkPoint, entitySize, Color.red, 0);

            var dir = (checkPoint - position).normalized;
            var radiusDir = mapSpace.GetMapDirection(dir) * Radius;

            // Get the closest point on the entity radius along the aim direction
            var closest = checkPoint -dir * entitySize;
            var distance = Vector2.Distance(position, closest);

            // Outside the radius?
            if(distance > radiusDir.magnitude)
            {
                continue;
            }

            var a = NormalizedAngle(mapMinDir, mapMaxDir, dir);

            if(a > 1.0f) // To the left of the cone?
            {
                var cl = ClosestPoint(position, position + (mapMaxDir * Radius), checkPoint);

                var dst = Vector2.Distance(cl, checkPoint);
                if(dst > entitySize)
                {
                    Debug.DrawLine(checkPoint, cl, Color.red);
                    continue;
                }

                Debug.DrawLine(checkPoint, cl, Color.green);
            }
            else if(a < 0.0f) // To the right of the cone?
            {
                var cl = ClosestPoint(position, position + (mapMinDir * Radius), checkPoint);

                var dst = Vector2.Distance(cl, checkPoint);
                if (dst > entitySize)
                {
                    Debug.DrawLine(checkPoint, cl, Color.red);
                    continue;
                }

                Debug.DrawLine(checkPoint, cl, Color.green);
            }

            entities.Add(hitEntity);
        }

        // Draw cone angles
        Debug.DrawLine(position, position + mapMinDir * Radius, Color.white);
        Debug.DrawLine(position, position + mapMaxDir * Radius, Color.white);
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
