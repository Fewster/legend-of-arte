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
        var rot = caster.transform.rotation;
        var fwd = caster.transform.forward;

        var checkPoint = new Vector2(pos.x, pos.y);

        //DrawAttackCone(pos, caster, mapSpace);

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
    /// /// Any value outside of the 0-1 range is considered outside the cone.
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

    private static bool OvalIntersection(Vector2 point, Vector2 origin, float radiusX, float radiusY)
    {
        float a = radiusY;
        float b = radiusX;

        var x = point.x;
        var y = point.y;
        float h = origin.x;
        float k = origin.y;

        var theta = Mathf.Atan2(b * (y - k), a * (x - h));
        var distance =
            Mathf.Pow((x - h) * Mathf.Cos(theta) + (y - k) * Mathf.Sin(theta), 2.0f) / Mathf.Pow(a, 2.0f) + 
            Mathf.Pow((x - h) * Mathf.Sin(theta) - (y - k) * Mathf.Cos(theta), 2.0f) / Mathf.Pow(b, 2.0f);

        //Debug.Log(distance);

        if (distance < 1.0f)
        {
            return true;
        }

        return false;
    }

    private void DrawAttackCone(Vector2 position, Entity source, MapSpace mapSpace)
    {
        {
            // Compute aim angles
            var worldAimDir = source.Direction.ToDirection();
            var worldMinDir = Quaternion.Euler(0.0f, 0.0f, -ConeAngle) * worldAimDir;
            var worldMaxDir = Quaternion.Euler(0.0f, 0.0f, ConeAngle) * worldAimDir;

            // Convert poses to map space
            var mapAimDir = mapSpace.ToMap(worldAimDir);
            var mapMinDir = mapSpace.ToMap(worldMinDir);
            var mapMaxDir = mapSpace.ToMap(worldMaxDir);

            Vector2 minPos2 = position + mapMinDir * Radius * 3.0f;
            Vector2 maxPos2 = position + mapMaxDir * Radius * 3.0f;

            Debug.DrawLine(position, position + (mapAimDir.normalized * Radius), Color.black);
            Debug.DrawLine(position, minPos2, Color.red);
            Debug.DrawLine(position, maxPos2, Color.red);
        }

        var aimAngle = source.Direction.To2DAngle();
        var minAngle = aimAngle - ConeAngle;
        var maxAngle = aimAngle + ConeAngle;

        var aimDir = mapSpace.GetMapDirection(aimAngle);
        var minDir = mapSpace.GetMapDirection(minAngle);
        var maxDir = mapSpace.GetMapDirection(maxAngle);

        Vector2 minPos = position + (minDir * Radius);
        Vector2 maxPos = position + (maxDir * Radius);

        Debug.DrawLine(position, position + (aimDir.normalized * Radius), Color.black);
        Debug.DrawLine(position, minPos, Color.white);
        Debug.DrawLine(position, maxPos, Color.white);
    }

    private float MapDistance(Vector2 source, Vector2 other)
    {
        return 0.0f;
    }

    private void GatherNearbyEntities(Vector2 position, Entity source, MapSpace mapSpace)
    {
        entities.Clear();

        // Compute aim angles
        var worldAimDir = source.Direction.ToDirection();
        var worldMinDir = Quaternion.Euler(0.0f, 0.0f, -ConeAngle) * worldAimDir;
        var worldMaxDir = Quaternion.Euler(0.0f, 0.0f, ConeAngle) * worldAimDir;

        // Convert poses to map space
        var mapAimDir = mapSpace.ToMap(worldAimDir);
        var mapMinDir = mapSpace.ToMap(worldMinDir) * Radius;
        var mapMaxDir = mapSpace.ToMap(worldMaxDir) * Radius;

        Debug.DrawLine(position, position + mapMinDir * Radius, Color.white);
        Debug.DrawLine(position, position + mapMaxDir * Radius, Color.white);

        DrawCircle(position, 32, Radius, Color.cyan, mapSpace);

        var hits = Physics2D.OverlapCircleAll(position, Radius, Mask);
        foreach (var hit in hits)
        {
            var hitEntity = hit.GetComponent<Entity>();
            if (hitEntity == null || hitEntity == source) // Don't self-hit or attack NULL entities
            {
                continue;
            }

            var checkPoint = (Vector2)hit.transform.position;

            var mapScale = mapSpace.ToMap(new Vector2(1.0f, 1.0f));
            if (!OvalIntersection(position, checkPoint, mapScale.x, mapScale.y))
            {
               // continue;
            }

            var dir = (checkPoint - position);
            var normalizedAngle = NormalizedAngle(mapMinDir, mapMaxDir, dir);

            //Debug.DrawLine(position, checkPoint, Color.red);

            // If the angle is outside the range, ignore the entity.
            if (normalizedAngle < 0.0f || normalizedAngle > 1.0f)
            {
                continue;
            }

            var heading = mapSpace.ToMapHeading(dir) * Radius;
            
            if(dir.magnitude > heading.magnitude)
            {
                continue;
            }

            Debug.DrawLine(position, position + heading, Color.magenta);

            //Debug.DrawLine(position, checkPoint, Color.green);

            entities.Add(hitEntity);
        }
    }

    private void DrawCircle(Vector2 position, int segments, float radius, Color color, MapSpace mapSpace)
    {
        var mod = (Mathf.PI * 2.0f) / segments;

        for (int i = 0; i < segments; i++)
        {
            var a = position + new Vector2(radius * Mathf.Cos((i - 1) * mod), radius * Mathf.Sin((i - 1) * mod));
            var b = position + new Vector2(radius * Mathf.Cos(i * mod), radius * Mathf.Sin(i * mod));

            Debug.DrawLine(a, b, color);
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
