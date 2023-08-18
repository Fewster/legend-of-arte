using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyTestDamage : MonoBehaviour
{
    public GameObject Target;
    public int Amount;

    [ContextMenu("Apply")]
    public void ApplyDamage()
    {
        var damageable = Target.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(Amount);
        }
    }
}
