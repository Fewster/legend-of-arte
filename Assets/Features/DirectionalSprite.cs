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
        var idx = ToIndex(direction);
        OnSpriteSet?.Invoke(sprites[idx]);
    }

    private int ToIndex(Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return 0;
            case Direction.East:
                return 2;
            case Direction.South:
                return 4;
            case Direction.West:
                return 6;
            case Direction.NorthEast:
                return 1;
            case Direction.SouthEast:
                return 3;
            case Direction.SouthWest:
                return 5;
            case Direction.NorthWest:
                return 7;
            default:
                return 0;
        }
    }
}

public enum Direction
{
    None = 0,
    North = 1,
    East = 2,
    South = 4,
    West = 8,

    NorthEast = 3,
    SouthEast = 6,
    SouthWest = 12,
    NorthWest = 9
}

public static class DirectionExtensions
{
    public static Quaternion ToRotationAngle(this Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return Quaternion.Euler(new Vector3(-90, 90, 0));
            case Direction.East:
                return Quaternion.Euler(new Vector3(0, 90, 0));
            case Direction.South:
                return Quaternion.Euler(new Vector3(90, 90, 0));
            case Direction.West:
                return Quaternion.Euler(new Vector3(0, -90, 0));
            case Direction.NorthEast:
                return Quaternion.Euler(new Vector3(-26.525f, 90, 0));
            case Direction.SouthEast:
                return Quaternion.Euler(new Vector3(26.525f, 90, 0));
            case Direction.SouthWest:
                return Quaternion.Euler(new Vector3(26.525f, -90, -90));
            case Direction.NorthWest:
                return Quaternion.Euler(new Vector3(-26.525f, -90, -90));

        }

        return Quaternion.identity;
    }

    public static Vector2 ToVector(this Direction direction)
    {
        return ToRotationAngle(direction) * Vector2.zero;
    }
}