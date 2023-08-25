using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamageable
{
    public int Current;
    public float Max;

    public UnityEvent OnDeath;

    public void TakeDamage(int value)
    {
        Current -= value;
        if(Current <= 0)
        {
            OnDeath?.Invoke();

            // Death
            //Debug.Log("Ded");
        }
    }
}