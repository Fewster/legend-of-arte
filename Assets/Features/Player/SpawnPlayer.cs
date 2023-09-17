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

    //[SerializeField]
    //private List<Ability> abilities;

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

        var abil = entity.GetComponent<Abilities>();
        //foreach(var ability in abilities)
        //{
           // abil.Add(ability);
        //}

        player.SetPlayerEntity(entity);
        cameraSystem.SetFocus(entity.transform);
    }
}
