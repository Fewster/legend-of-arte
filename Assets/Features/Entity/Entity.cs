using Game.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A base implementation of an entity.
/// </summary>
public class Entity : GameBehaviour
{
    [SerializeField] private string id;

    private IActiveEntities store;

    /// <summary>
    /// The Id of this type of entity.
    /// </summary>
    public string Id { get { return id; } }

    /// <summary>
    /// Guaranteed to be unique per entity.
    /// The Id should survive persistence.
    /// </summary>
    public Guid UniqueId { get; private set; }

    protected virtual void Awake()
    {
        // TODO: When serialization/persistence is involved
        // we need to ensure the Id is correctly transferred.

        UniqueId = Guid.NewGuid();
    }

    protected override void Setup()
    {
        store = Resolver.Resolve<IActiveEntities>();
        store.Register(this);
    }

    protected override void Cleanup()
    {
        if (store != null)
        {
            store.Unregister(this);
        }
    }
}
