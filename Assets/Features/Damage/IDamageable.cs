using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : ScriptableObject
{

}

public interface IHealth
{
    int Current { get; }
    int Max { get; }
}

public interface IDamageable
{
    void TakeDamage(int value);
}

public interface IEffectable
{
    void ApplyEffect(Effect source);
}
