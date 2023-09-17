using Game.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyboardInput : GameBehaviour
{
    private LocalPlayer localPlayer;

    protected override void Setup()
    {
        localPlayer = Resolver.Resolve<LocalPlayer>();
    }

    private void Update()
    {
        var player = localPlayer.GetPlayerEntity();
        if (player == null)
        {
            return;
        }

        var loco = player.GetComponent<IEntityLocomotion>();
        if(loco == null)
        {
            return;
        }

        var wPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var dir = wPos - player.transform.position;

        loco.Aim(dir);

        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");

        var input = new Vector2(x, y);
        if(input.magnitude < 0.1f)
        {
            return;
        }

        
        loco.Move(new Vector2(x, y));
    }
}
