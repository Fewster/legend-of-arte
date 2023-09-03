using Game.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : GameBehaviour
{
    private LocalPlayer localPlayer;

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
            var abilities = playerEntity.GetComponent<Abilities>();
            if(abilities.Count > 0)
            {
                var ability = abilities[0];
                var result = ability.Cast(playerEntity);
                if (result.Status != CastStatus.Successful)
                {
                    Debug.LogError($"{result.Error}");
                }
            }

            
        }
    }
}
