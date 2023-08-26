using Game.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Provides spawn behaviour for entities.
/// </summary>
public class EntitySpawner : GameBehaviour
{
    private IEntityStore store;
    private IGameSpace space;

    public UnityEvent<Entity> OnSpawned;

    protected override void Setup()
    {
        store = Resolver.Resolve<IEntityStore>();
        space = Resolver.Resolve<IGameSpace>();
    }

    public Entity Spawn(string id)
    {
        var gameSpace = space.GetGameObject();
        if(gameSpace == null) 
        { 
            return default;
        }

        var entity = store.GetPrefab(id);
        if(entity == null)
        {
            return default;
        }

        var instance = Instantiate<Entity>(entity);
        instance.transform.SetParent(gameSpace.transform, false);

        // TODO: We may want to let callers specify the spawn rotation
        var initialDirection = Direction.North;
        instance.transform.transform.rotation = initialDirection.ToRotationAngle();

        OnSpawned?.Invoke(instance);

        return instance;
    }

    public void SpawnEntity(string id)
    {
        _ = Spawn(id);
    }
}
