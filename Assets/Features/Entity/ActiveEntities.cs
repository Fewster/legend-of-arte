using Game.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides access to active entities.
/// </summary>
public interface IActiveEntities : IEnumerable<Entity>
{
    /// <summary>
    /// Fired when an <see cref="Entity"/> is added.
    /// </summary>
    event Action<Entity> OnEntityAdded;

    /// <summary>
    /// Fired when an <see cref="Entity"/> is removed.
    /// </summary>
    event Action<Entity> OnEntityRemoved;

    /// <summary>
    /// Find an <see cref="Entity"/> with the given Id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Entity FindEntity(Guid id);

    /// <summary>
    /// Register a given <see cref="Entity"/> into the active entities collection.
    /// </summary>
    /// <param name="entity"></param>
    void Register(Entity entity);

    /// <summary>
    /// Remove a given <see cref="Entity"/> from the active entities collection.
    /// </summary>
    /// <param name="entity"></param>
    void Unregister(Entity entity);
}

public class ActiveEntities : GameService<IActiveEntities>, IActiveEntities
{
    private Dictionary<Guid, Entity> entities;

    public event Action<Entity> OnEntityAdded;
    public event Action<Entity> OnEntityRemoved;

    protected override void Awake()
    {
        entities = new Dictionary<Guid, Entity>();

        base.Awake();
    }

    public Entity FindEntity(Guid id)
    {
        entities.TryGetValue(id, out Entity entity);
        return entity;
    }

    public void Register(Entity entity)
    {
        entities.Add(entity.UniqueId, entity);

        try
        {
            OnEntityAdded?.Invoke(entity);
        }
        catch(Exception e)
        {
            Debug.LogException(e);  
        }
    }

    public void Unregister(Entity entity)
    {
        entities.Remove(entity.UniqueId);

        try
        {
            OnEntityRemoved?.Invoke(entity);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public IEnumerator<Entity> GetEnumerator()
    {
        return entities.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return entities.Values.GetEnumerator();
    }
}
