using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public float Lifetime = 1.0f;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(Lifetime);
        Destroy(gameObject);
    }
}
