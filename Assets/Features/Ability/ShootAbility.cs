using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Shoot")]
public class ShootAbility : Ability
{
    public LayerMask Layer;

    public override void Cast(Entity caster)
    {
        var origin = caster.transform.position;
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var hitPoint = new Vector2(pos.x, pos.y);

        var dir = hitPoint - (Vector2)origin;
        Debug.DrawLine((Vector3)hitPoint, origin, Color.red, 1.0f);

        var hits = Physics2D.RaycastAll(caster.transform.position, dir.normalized, 100.0f, Layer);
        foreach(var hit in hits)
        {
            Debug.DrawLine(origin, hit.point, Color.yellow, 1.0f);
            Debug.Log("hit");

            var hitEntity = hit.collider.GetComponent<Entity>();
            if(hitEntity == null)
            {
                continue;
            }

            var damageable = hitEntity.GetComponent<IDamageable>();
            if(damageable == null)
            {
                continue;
            }

            damageable.TakeDamage(10);
        }
    }
}
