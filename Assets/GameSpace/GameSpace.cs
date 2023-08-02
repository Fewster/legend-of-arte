using Game.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a GameObject which hosts the content of a game.
/// Game content will be attached to this object.
/// </summary>
public interface IGameSpace
{
    GameObject GetGameObject();
}

public class GameSpace : GameService<IGameSpace>, IGameSpace
{
    /// <summary>
    /// The GameObject parent representing the game space
    /// </summary>
    public GameObject HostObject;

    public GameObject GetGameObject()
    {
        if(HostObject == null)
        {
            return gameObject;
        }

        return HostObject;
    }
}
