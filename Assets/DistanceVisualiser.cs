using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DistanceVisualiser : MonoBehaviour
{
    public GameObject a;
    public GameObject b;

    public float Distance;

    public float xMod = 0.25f;
    public float yMod = 0.5f;

    public float ang;

    // Update is called once per frame
    void Update()
    {
        if (a == null || b == null)
        {
            return;
        }

        Distance = Vector3.Distance(a.transform.position, b.transform.position);
        Debug.DrawLine(a.transform.position, b.transform.position, Color.red);

        var pt = ComputePoint(transform.position, 1.0f, ang * Mathf.Deg2Rad);
        Debug.DrawLine(transform.position, pt, Color.green);

        DrawCircle(transform.position, 32, 1.0f, Color.red, 0.1f);
    }

    private void DrawCircle(Vector2 position, int segments, float radius, Color color, float duration)
    {
        var mod = (Mathf.PI * 2.0f) / segments;

        for (int i = 0; i < segments; i++)
        {
            var a = ComputePoint(position, radius, (i - 1) * mod);
            var b = ComputePoint(position, radius, i * mod);

            //var a = position + new Vector2((radius * Mathf.Cos((i - 1) * mod)) * xMod, (radius * Mathf.Sin((i - 1) * mod)) * yMod);
            //var b = position + new Vector2((radius * Mathf.Cos(i * mod)) * xMod, (radius * Mathf.Sin(i * mod)) * yMod);

            Debug.DrawLine(a, b, color, duration);
        }
    }

    private Vector2 ComputePoint(Vector2 origin, float radius, float angle)
    {
        return origin + new Vector2((radius * Mathf.Cos(angle)) * xMod, (radius * Mathf.Sin(angle)) * yMod);
    }
}
