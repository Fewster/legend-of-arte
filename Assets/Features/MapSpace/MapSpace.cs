using Game.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpace : GameService<MapSpace>
{
    public Grid Grid;
    public Direction dir;
    public float ang;

    public GameObject Test;
    public float Mod;

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

    public float GetMapHeading(Vector2 direction)
    {
        var ang = Vector2.SignedAngle(direction, Vector2.right) + 90.0f;

        return ang * Mathf.Deg2Rad;
    }

    private float EllipseDistance(float angle, float w, float h)
    {
        var w2 = Mathf.Pow(w, 2);
        var h2 = Mathf.Pow(h, 2);
        var x2 = Mathf.Pow(Mathf.Sin(angle), 2);
        var y2 = Mathf.Pow(Mathf.Cos(angle), 2);

        var distance = Mathf.Sqrt((w2 * x2) + (h2 * y2));
        return distance;
    }

    private Vector2 PointOnEllipse(float heading)
    {
        var x = Grid.cellSize.x * Mathf.Sin(heading);
        var y = Grid.cellSize.y * Mathf.Cos(heading);
        return new Vector2(x, y);
    }

    private Vector2 PointOnCircle(float heading)
    {
        var x = Mathf.Sin(heading);
        var y = Mathf.Cos(heading);
        return new Vector2(x, y);
    }

    /// <summary>
    /// Flatten a given world-space direction into map space.
    /// This effectively maps the direction onto an ellipse, stretched to the perspective of the world grid.
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    public Vector2 GetMapDirection(Vector2 dir)
    {
        var grid = Grid.cellSize;
        return PointOnEllipse(Mathf.Atan2(dir.x * grid.y, dir.y * grid.x));
    }

    public Vector2 SquashToMap(float heading)
    {
        var x = Grid.cellSize.x * Mathf.Sin(heading);
        var y = Grid.cellSize.y * Mathf.Cos(heading);
        var pos = new Vector2(x, y);

        return pos;

        var dist = EllipseDistance(heading, Grid.cellSize.x, Grid.cellSize.y);

        var vector = new Vector2(Mathf.Sin(heading), Mathf.Cos(heading)) * dist;

        return vector;

        Debug.DrawLine(Vector2.zero, Vector2.zero + vector, Color.magenta);

        return Vector2.zero;
    }

    public Vector2 ToMap(Vector2 src)
    {
        return src * new Vector2(Grid.cellSize.x, Grid.cellSize.y);
    }

    private void OnDrawGizmos()
    {
        return;

        if (Grid == null)
        {
            return;
        }

        if(Test == null)
        {
            return;
        }

        var dir = Test.transform.position - transform.position;
        var vec = (Vector3)GetMapDirection(dir);

        var v1 = GetMapDirection(dir);
        var v2 = ToMap(dir);

        var refl = (Vector3)Vector2.Reflect(vec.normalized, Vector2.right);
        Debug.DrawLine(transform.position, (Vector2)transform.position + v1, Color.magenta);
        Debug.DrawLine(transform.position, (Vector2)transform.position + v2, Color.yellow);


        DrawEllipse(transform.position, 32, 1.0f, Color.yellow);

        //var a = (-ang + 90.0f) * Mathf.Deg2Rad;
        //var d1 = ((Vector2)transform.position + new Vector2(1.0f * Mathf.Cos(a), 1.0f * Mathf.Sin(a))) * 3.0f;
        //Debug.DrawLine((Vector2)transform.position, (Vector2)transform.position + ToMap(d1), Color.cyan);

        //Debug.DrawLine(transform.position, transform.position + (Vector3)GetMapDirection(dir), Color.magenta);
        //Debug.DrawLine(transform.position, transform.position + (Vector3)GetMapDirection(ang), Color.yellow);

        //DrawCircle(transform.position, 32, 1.0f, Color.red);
    }

    private void DrawEllipse(Vector2 position, int segments, float radius, Color color)
    {
        var mod = (Mathf.PI * 2.0f) / segments;

        for (int i = 0; i < segments; i++)
        {
            var a = position + SquashToMap((i - 1) * mod);
            var b = position + SquashToMap(i * mod);

            Debug.DrawLine(a, b, color);
        }
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
