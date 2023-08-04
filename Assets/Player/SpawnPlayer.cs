using Game.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : GameBehaviour
{
    private LocalPlayer player;
    private CameraSystem cameraSystem;

    public string PlayerEntityId;
    public EntitySpawner Spawner;

    protected override void Setup()
    {
        player = Resolver.Resolve<LocalPlayer>();
        cameraSystem = Resolver.Resolve<CameraSystem>();
    }

    public void Spawn()
    {
        var entity = Spawner.Spawn(PlayerEntityId);
        if(entity == null)
        {
            Debug.LogError("Player entity is not valid");
            return;
        }

        player.SetPlayerEntity(entity);
        cameraSystem.SetFocus(entity.transform);
    }
}
