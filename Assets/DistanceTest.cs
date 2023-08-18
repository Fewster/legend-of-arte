using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class DistanceTest : MonoBehaviour
{
    public Transform src;
    public Transform dst;
    public Transform pnt;

    private void OnDrawGizmos()
    {
        var r = 1.0f;
        var w = 1.0f;
        var h = 1.0f;

        var bounds = new Bounds(dst.position, new Vector3(w, h, 0));
        if (CircleToAABB(src.position, r, bounds))
        {
            Handles.color = Color.red;
        }
        else
        {
            Handles.color = Color.green;
        }

        Handles.DrawWireCube(dst.position, new Vector3(w, h, 1));
        Handles.DrawWireDisc(src.position, new Vector3(0, 0, 1), r);

        Gizmos.color = Color.cyan;
    }

    private bool CircleToAABB(Vector3 position, float radius, Bounds bounds)
    {
        var closest = ClosestPointOnAABB(position, bounds);
        var distance = Vector3.Distance(position, closest);

        return distance < radius;
    }

    private Vector3 ClosestPointOnAABB(Vector3 position, Bounds bounds)
    {
        var dir = position - bounds.center;
        dir.x = Mathf.Clamp(dir.x, -bounds.extents.x, bounds.extents.x);
        dir.y = Mathf.Clamp(dir.y, -bounds.extents.y, bounds.extents.y);

        return bounds.center + dir;
    }

    private Vector3 ClosestPoint(Vector3 a, Vector3 b, Vector3 p)
    {
        var dir = p - a;
        var line = b - a;

        var t = Vector3.Dot(dir, line) / line.sqrMagnitude;
        t = Mathf.Clamp01(t);

        return a + t * line;
    }





    //private void OnDrawGizmos()
    //{
    //    var rad = src.transform.localScale.x;
    //    var w = dst.transform.localScale.x;
    //    var h = src.transform.localScale.y;

    //    var bounds = new Bounds(dst.transform.position, new Vector3(w, h, 1));
    //    var hit = CircleToBox(bounds, src.transform.position, rad);

    //    if (hit)
    //    {
    //        Handles.color = Color.green;
    //    }
    //    else
    //    {
    //        Handles.color = Color.red;
    //    }

    //    Handles.DrawWireDisc(src.transform.position, new Vector3(0, 0, 1), rad);
    //    Handles.DrawWireCube(dst.transform.position, new Vector3(w, h, 1));
    //}

    private bool CircleToBox(Bounds bounds, Vector3 point, float radius)
    {
        return false;

        //// THis math sucks. Look at these:
        //// https://gamedev.stackexchange.com/questions/156870/how-do-i-implement-a-aabb-sphere-collision
        //// Follow SAT flow:
        //// Centre of circle -> box centre = axis
        //// Project points onto line.
        //// Check line points

        //var dir = (point - bounds.center).normalized;


        //Vector3.proj

        //var dx = Mathf.Abs(point.x - bounds.center.x);
        //var dy = Mathf.Abs(point.y - bounds.center.y);

        //var minX = bounds.extents.x + radius;
        //var minY = bounds.extents.y + radius;

        //if (dx < minX && dy < minY)
        //{
        //    return true;
        //}

        //return false;
    }
}
