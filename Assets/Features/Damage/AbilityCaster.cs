using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCaster : MonoBehaviour
{
    public Ability ability;
    public Entity caster;

    public void Cast()
    {
        var result = ability.Cast(caster);
        if (result.Status != CastStatus.Successful)
        {
            Debug.LogError($"{result.Error}");
        }
    }
}
