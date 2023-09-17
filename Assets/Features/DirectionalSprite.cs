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
        var idx = direction.ToIndex();
        OnSpriteSet?.Invoke(sprites[idx]);
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
    public static int ToIndex(this Direction direction)
    {
        switch (direction)
        {
            default:
            case Direction.North:
                return 0;
            case Direction.NorthEast:
                return 1;
            case Direction.East:
                return 2;
            case Direction.SouthEast:
                return 3;
            case Direction.South:
                return 4;
            case Direction.SouthWest:
                return 5;
            case Direction.West:
                return 6;
            case Direction.NorthWest:
                return 7;
        }
    }

    public static Vector2 ToDirection(this Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return new Vector2(0, 1);
            case Direction.NorthEast:
                return new Vector2(1, 1).normalized;
            case Direction.East:
                return new Vector2(1, 0);
            case Direction.SouthEast:
                return new Vector2(1,-1).normalized;
            case Direction.South:
                return new Vector2(0, -1);
            case Direction.SouthWest:
                return new Vector2(-1, -1).normalized;
            case Direction.West:
                return new Vector2(-1, 0);
            case Direction.NorthWest:
                return new Vector2(-1, 1).normalized;
            default:
                return Vector2.zero;
        }
    }

    public static float To2DAngle(this Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return 0.0f;
            case Direction.NorthEast:
                return -45.0f;
            case Direction.East:
                return -90.0f;
            case Direction.SouthEast:
                return -135.0f;
            case Direction.South:
                return 180.0f;
            case Direction.SouthWest:
                return 45.0f;
            case Direction.West:
                return 90.0f;
            case Direction.NorthWest:
                return 135.0f;
            default:
                return 0.0f;
        }
    }

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

    public static Direction GetNearestDirection(Vector2 value)
    {
        var angle = Mathf.Atan2(value.x, value.y) * Mathf.Rad2Deg;
        var seg = (int)(angle / 22.5f);

        switch (seg)
        {
            case 1:
            case 2:
                return Direction.NorthEast;
            case 3:
            case 4:
                return Direction.East;
            case 5:
            case 6:
                return Direction.SouthEast;
            case 7:
            case -7:
                return Direction.South;
            case -1:
            case -2:
                return Direction.NorthWest;
            case -3:
            case -4:
                return Direction.West;
            case -5:
            case -6:
                return Direction.SouthWest;
            default:
                return Direction.North;
        }
    }

}