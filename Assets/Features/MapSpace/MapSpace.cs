using Game.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpace : GameService<MapSpace>
{
    public Grid Grid;
    public Direction dir;
    public float ang;

    /// <summary>
    /// Convert an angle (in degrees) with 0 representing north into a direction which maps into the isometric space.
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    public Vector2 GetMapDirection(float angle)
    {
        return ComputePoint(Vector2.zero, 1.0f, (-angle + 90.0f) * Mathf.Deg2Rad);
    }

    public Vector2 GetMapDirection(Direction direction)
    {
        var qPi = Mathf.PI * 0.25f;
        var hPi = Mathf.PI * 0.5f;

        switch (direction)
        {
            case Direction.North:
                return ComputePoint(Vector2.zero, 1.0f, hPi);
            case Direction.NorthEast:
                return ComputePoint(Vector2.zero, 1.0f, qPi);
            case Direction.East:
                return ComputePoint(Vector2.zero, 1.0f, 0.0f);
            case Direction.SouthEast:
                return ComputePoint(Vector2.zero, 1.0f, -qPi);
            case Direction.South:
                return ComputePoint(Vector2.zero, 1.0f, -hPi);
            case Direction.SouthWest:
                return ComputePoint(Vector2.zero, 1.0f, -hPi - qPi);
            case Direction.West:
                return ComputePoint(Vector2.zero, 1.0f, Mathf.PI);
            case Direction.NorthWest:
                return ComputePoint(Vector2.zero, 1.0f, +hPi + qPi);
            default:
                return new Vector2(0, 0);
        }
    }

    private void OnDrawGizmos()
    {
        if (Grid == null)
        {
            return;
        }

        Debug.DrawLine(transform.position, transform.position + (Vector3)GetMapDirection(dir), Color.magenta);
        Debug.DrawLine(transform.position, transform.position + (Vector3)GetMapDirection(ang), Color.yellow);

        DrawCircle(transform.position, 32, 1.0f, Color.red);
    }

    private void DrawCircle(Vector2 position, int segments, float radius, Color color)
    {
        var mod = (Mathf.PI * 2.0f) / segments;

        for (int i = 0; i < segments; i++)
        {
            var a = ComputePoint(position, radius, (i - 1) * mod);
            var b = ComputePoint(position, radius, i * mod);

            Debug.DrawLine(a, b, color);
        }
    }

    private Vector2 ComputePoint(Vector2 origin, float radius, float angle)
    {
        return origin + new Vector2((radius * Mathf.Cos(angle)) * Grid.cellSize.x, (radius * Mathf.Sin(angle)) * Grid.cellSize.y);
    }
}
