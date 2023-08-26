using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MouseInput : MonoBehaviour
{
    public MouseButton Button;
    public UnityEvent OnButtonDown;

    private void Update()
    {

        if(Button == MouseButton.Left)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnButtonDown?.Invoke();
            }
        }
        else if (Button == MouseButton.Right)
        {
            if (Input.GetMouseButtonDown(1))
            {
                OnButtonDown?.Invoke();
            }
        }
    }
}

public enum MouseButton
{
    None,
    Left,
    Right
}
