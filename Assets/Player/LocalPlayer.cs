using Game.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

public class LocalPlayer : GameService<LocalPlayer>
{
    // TODO: Decide how the player will be spawned.
    // Maybe just have something set the 'player' entity.
    // Maybe a bespoke script handles spawning the player + setting it?
    // Who knows...
    //

    public Entity entity;

    public event Action OnPlayerSpawned; 
    public event Action OnPlayerKilled;

    public Entity GetPlayerEntity()
    {
        return entity;
    }

    public void SpawnPlayer()
    {

    }
}
