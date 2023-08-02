using Game.Framework;
using UnityEngine;

/// <summary>
/// Represents an object which stores entity prefabs by their unique name.
/// </summary>
public interface IEntityStore
{
    Entity GetPrefab(string id);
}

public class EntityStore : GameService<IEntityStore>, IEntityStore
{
    [SerializeField]
    private Entity[] entities;

    public Entity GetPrefab(string id)
    {
        foreach(var entity in entities)
        {
            if(entity.Id == id)
            {
                return entity;
            }
        }

        return default;
    }
}
