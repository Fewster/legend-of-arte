using Game.Framework;
using UnityEngine;

public class PlayerInput : GameBehaviour
{
    private LocalPlayer player;
    private Entity playerEntity;

    private Locomotion locomotion;


    protected override void Setup()
    {
        player = Resolver.Resolve<LocalPlayer>();

        playerEntity = player.GetPlayerEntity();
        if (playerEntity == null)
        {
            return;
        }

        locomotion = playerEntity.GetComponent<Locomotion>();

        player.OnPlayerKilled += HandlePlayerKilled;
        player.OnPlayerSpawned += HandlePlayerSpawned;
    }

    private void HandlePlayerSpawned()
    {
        playerEntity = player.GetPlayerEntity();
        if (playerEntity == null)
        {
            return;
        }

        locomotion = playerEntity.GetComponent<Locomotion>();

        player.OnPlayerKilled += HandlePlayerKilled;
        player.OnPlayerSpawned += HandlePlayerSpawned;
    }

    private void HandlePlayerKilled()
    {
        locomotion = null;
    }

    private void Update()
    {
        var pl = player.GetPlayerEntity();
        if(pl == null)
        {
            return;
        }

        locomotion = pl.GetComponent<Locomotion>();

        if(locomotion == null)
        {
            return;
        }

        var inputX = Input.GetAxis("Horizontal");
        var inputY = Input.GetAxis("Vertical");

        locomotion.Move(new Vector2(inputX, inputY));
    }
}