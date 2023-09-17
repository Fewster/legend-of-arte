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
    [SerializeField] private float size = 1.0f;
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
            // Ignore same value
            if(value == Direction)
            {
                return;
            }

            direction = value;
            //transform.rotation = direction.ToRotationAngle();
            InvokeOnDirectionSet(value);
        }
    }

    public float Size { get { return size; } }

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
        Direction = Direction;
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

    protected virtual void LateUpdate()
    {
        if (transform.hasChanged)
        {
            var dir2d = (Vector2)transform.forward;
            var dir = DirectionExtensions.GetNearestDirection(dir2d);
            Direction = dir;

            transform.hasChanged = false;
        }
    }

    private void InvokeOnDirectionSet(Direction dir)
    {
        OnDirectionSet?.Invoke(dir);
    }
}
