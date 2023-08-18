using Game.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : GameBehaviour
{
    private LocalPlayer cameraSystem;

    protected override void Setup()
    {
        cameraSystem = Resolver.Resolve<LocalPlayer>();
    }

    private void LateUpdate()
    {
        var focus = cameraSystem.entity;
        if(focus == null)
        {
            return;
        }

        transform.position = focus.transform.position;
    }
}
