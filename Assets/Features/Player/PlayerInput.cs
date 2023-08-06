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

    private void FixedUpdate()
    {
        // TODO: Ensure this fixed update approach works...

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

        var up = Input.GetKey(KeyCode.W);
        var down = Input.GetKey(KeyCode.S);
        var left = Input.GetKey(KeyCode.A);
        var right = Input.GetKey(KeyCode.D);

        var mask = Direction.None;
        if (up)
        {
            mask |= Direction.North;
        }
        if (down)
        {
            mask |= Direction.South;
        }
        if (left)
        {
            mask |= Direction.West;
        }
        if (right)
        {
            mask |= Direction.East;
        }

        if (mask != Direction.None)
        {
            locomotion.Move(mask);
        }

        //if (up)
        //{
        //    if (right)
        //    {
        //        locomotion.Move(Direction.NorthEast);
        //    }
        //    else if(left)
        //    {
        //        locomotion.Move(Direction.NorthWest);
        //    }
        //    else
        //    {
        //        locomotion.Move(Direction.North);
        //    }
        //}
        //else if (down)
        //{
        //    if (right)
        //    {
        //        locomotion.Move(Direction.SouthEast);
        //    }
        //    else if (left)
        //    {
        //        locomotion.Move(Direction.SouthWest);
        //    }
        //    else
        //    {
        //        locomotion.Move(Direction.South);
        //    }
        //}

        //var inputX = Input.GetAxis("Horizontal");
        //var inputY = Input.GetAxis("Vertical");

        //if((Mathf.Abs(inputX) + Mathf.Abs(inputY)) > 0.1f)
        //{
        // //   locomotion.Move(new Vector2(inputX, inputY));
        //}
    }
}