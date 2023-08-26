using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventProvider : MonoBehaviour
{
    public UnityEvent OnStart;

    private void Start()
    {
        OnStart?.Invoke();
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
