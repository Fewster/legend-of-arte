using Game.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : GameBehaviour
{
    private LocalPlayer localPlayer;

    // TODO: Multiple abilities
    public Ability Ability;

    protected override void Setup()
    {
        localPlayer = Resolver.Resolve<LocalPlayer>();
    }

    private void Update()
    {
       // Cast();
    }

    public void Cast()
    {
        var playerEntity = localPlayer.GetPlayerEntity();
        if (playerEntity != null)
        {
            Ability.Cast(playerEntity);
        }
    }
}
