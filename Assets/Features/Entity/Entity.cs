using Game.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A base implementation of an entity.
/// </summary>
public class Entity : GameBehaviour
{
    [SerializeField] private string id;
    [SerializeField] private Direction direction;

    private IActiveEntities store;

    /// <summary>
    /// The Id of this type of entity.
    /// </summary>
    public string Id { get { return id; } }

    public Direction Direction
    {
        get
        {
            return direction;
        }
        set
        {
            direction = value;
            transform.rotation = direction.ToRotationAngle();
            InvokeOnDirectionSet(value);
        }
    }

    /// <summary>
    /// Guaranteed to be unique per entity.
    /// The Id should survive persistence.
    /// </summary>
    public Guid UniqueId { get; private set; }

    public UnityEvent<Direction> OnDirectionSet;

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

    private void InvokeOnDirectionSet(Direction dir)
    {
        OnDirectionSet?.Invoke(dir);
    }
}
