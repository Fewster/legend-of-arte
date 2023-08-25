using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebugExtensions 
{
    public static void DrawCone(Vector2 origin, float radius, Color color, float duration, float sizeX, float sizeY, float angle)
    {

    }

    public static void DrawCircle(Vector2 origin, float radius, Color color, float duration, int segments = 32)
    {
        DrawEllipse(origin, radius, color, duration, 1.0f, 1.0f, segments);
    }

    public static void DrawEllipse(Vector2 origin, float radius, Color color, float duration, float sizeX, float sizeY, int segments = 32)
    {
        var mod = (Mathf.PI * 2.0f) / segments;

        for (int i = 0; i < segments; i++)
        {
            var a = ComputePoint(origin, radius, (i - 1) * mod, sizeX, sizeY);
            var b = ComputePoint(origin, radius, i * mod, sizeX, sizeY);

            Debug.DrawLine(a, b, color, duration);
        }
    }

    private static Vector2 ComputePoint(Vector2 origin, float radius, float angle, float sizeX, float sizeY)
    {
        return origin + new Vector2((radius * Mathf.Cos(angle)) * sizeX, (radius * Mathf.Sin(angle)) * sizeY);
    }
}
