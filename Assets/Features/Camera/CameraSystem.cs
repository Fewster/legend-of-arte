using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Framework;

public class CameraSystem : GameService<CameraSystem>
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private Camera followCamera;

    public float LerpRate;

    public Transform Focus { get { return target; } }

    public void SetFocus(Transform target)
    {
        this.target = target;

        followCamera.transform.position = target.position + new Vector3(0.0f, 0.0f, -1.0f);
    }

    private void Update()
    {
        if(target == null)
        {
            return;
        }

        var targetPos = Vector3.Lerp(followCamera.transform.position, target.position + new Vector3(0.0f, 0.0f, -1.0f), LerpRate * Time.deltaTime);
        followCamera.transform.position = targetPos;
    }
}
