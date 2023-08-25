using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    public int Current;
    public float Max;

    public void TakeDamage(int value)
    {
        Current -= value;
        if(Current <= 0)
        {
            // Death
            //Debug.Log("Ded");
        }
    }
}