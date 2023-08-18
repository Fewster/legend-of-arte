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
        var dir = new Vector2(fwd.x, fwd.y);

        var loco = caster.GetComponent<Locomotion>();
        var dirAng = loco.Direction.To2DAngle();
        var minAng = dirAng - ConeAngle;
        var maxAng = dirAng + ConeAngle;

        var minVec = mapSpace.GetMapDirection(minAng);
        var maxVec = mapSpace.GetMapDirection(maxAng);
        Debug.DrawLine(pos, (Vector2)pos + minVec, Color.cyan);
        Debug.DrawLine(pos, (Vector2)pos + maxVec, Color.cyan);

        GatherNearbyEntities(checkPoint, dir, caster);



        if (entities.Count > 0)
        {
            DrawCircle(checkPoint, 16, Radius, Color.green, 0.5f);
        }
        else
        {
            DrawCircle(checkPoint, 16, Radius, Color.red, 0.5f);
        }
    }

    private void GatherNearbyEntities(Vector2 position, Vector2 direction, Entity source)
    {
        entities.Clear();

        var minDir = (Vector2)(Quaternion.Euler(0, 0, minAng) * direction);
        var maxDir = (Vector2)(Quaternion.Euler(0, 0, maxAng) * direction);

        Vector2 minPos = position + (minDir * Radius);
        Vector2 maxPos = position + (maxDir * Radius);

        Debug.DrawLine(position, position + (direction * Radius), Color.red);
        Debug.DrawLine(position, minPos, Color.white);
        Debug.DrawLine(position, maxPos, Color.white);

      //  Debug.DrawLine(position, position + ((Vector2)minDir * 5.0f), Color.white, 1.0f);
       // Debug.DrawLine(position, position + ((Vector2)maxDir * 5.0f), Color.white, 1.0f);

        var hits = Physics2D.OverlapCircleAll(position, Radius, Mask);
        foreach(var hit in hits)
        {
            var hitEntity = hit.GetComponent<Entity>();
            if (hitEntity == null || hitEntity == source) // Don't self-hit or attack NULL entities
            {
                continue;
            }

            //Physics2D.OverlapBox()

            var pos = hit.ClosestPoint(position);

            Debug.DrawLine(position, pos, Color.magenta, 3.0f);

            entities.Add(hitEntity);
        }
    }

    private void DrawCircle(Vector2 position, int segments, float radius, Color color, float duration)
    {
        var mod = (Mathf.PI * 2.0f) / segments;

        for (int i = 0; i < segments; i++)
        {
            var a = position + new Vector2(radius * Mathf.Cos((i - 1) * mod), radius * Mathf.Sin((i - 1) * mod));
            var b = position + new Vector2(radius * Mathf.Cos(i * mod), radius * Mathf.Sin(i * mod));

            Debug.DrawLine(a, b, color, duration);
        }
    }
}
