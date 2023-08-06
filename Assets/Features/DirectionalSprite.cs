using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DirectionalSprite : MonoBehaviour
{
    [SerializeField] protected Sprite[] sprites;

    public UnityEvent<Sprite> OnSpriteSet;

    public void SetSprite(Direction direction)
    {
        var idx = (int)direction;
        OnSpriteSet?.Invoke(sprites[idx]);
    }
}

public enum Direction
{
    North,
    NorthEast,
    East,
    SouthEast,
    South,
    SouthWest,
    West,
    NorthWest
}