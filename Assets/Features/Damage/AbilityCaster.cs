using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCaster : MonoBehaviour
{
    public Ability ability;
    public Entity caster;

    public void Cast()
    {
        ability.Cast(caster);
    }
}
