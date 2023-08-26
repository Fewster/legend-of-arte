using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CollisionTest : MonoBehaviour
{
    public Grid grid;
    public Tilemap map;

    public float Radius = 1.0f;
    public int CheckRadius = 11;

    public Color CollisionColor = Color.white * 0.1f;

    private void OnDrawGizmos()
    {
        //Handles.DrawWireDisc(transform.position, new Vector3(0,0,1), Radius);
    }

    private void Update()
    {
        map.RefreshAllTiles();

        var pos = transform.position;
        var cPos = grid.WorldToCell(pos);
        var wPos = grid.CellToWorld(cPos);

      //  Debug.DrawLine(transform.position, wPos);

        var w = CheckRadius;
        var h = CheckRadius;

        var s = -(w / 2);
        var e = h / 2;

        for(int i = s; i < e; i++)
        {
            for(int j = s; j < e; j++)
            {
                var cIdx = new Vector3Int(i + cPos.x, j + cPos.y, 0);
                var cIdxWPos = grid.CellToWorld(cIdx);

                var tile = map.GetTile(cIdx) as Tile;
                if(tile == null)
                {
                    continue;
                }

                var bounds = GetCellBounds(cIdx, tile);
                var ex = bounds.extents;

                var color = new Color((1.0f / w) * i, (1.0f / h) * j, 1.0f);

                if (CircleToAABB(transform.position, Radius, bounds))
                {
                    map.SetTileFlags(cIdx, TileFlags.None);
                    map.SetColor(cIdx, CollisionColor);

                    color = Color.red;
                }

                Debug.DrawLine(bounds.center + new Vector3(-ex.x, ex.y, 0), bounds.center + new Vector3(ex.x, ex.y, 0), color);
                Debug.DrawLine(bounds.center + new Vector3(-ex.x, -ex.y, 0), bounds.center + new Vector3(ex.x, -ex.y, 0), color);
                Debug.DrawLine(bounds.center + new Vector3(-ex.x, -ex.y, 0), bounds.center + new Vector3(-ex.x, ex.y, 0), color);
                Debug.DrawLine(bounds.center + new Vector3(ex.x, -ex.y, 0), bounds.center + new Vector3(ex.x, ex.y, 0), color);
            }
        }
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

    private Bounds GetCellBounds(Vector3Int tilePos, Tile tile)
    {
        var sprite = tile.sprite;
        var ppu = sprite.pixelsPerUnit;
        var border = sprite.border;
        var anchor = map.tileAnchor;

        // Account for borders
        var bX = sprite.texture.width - border.x - border.z;
        var bY = sprite.texture.height - border.y - border.w;

        // Calculate the size of the sprite
        var ppuX = bX / ppu;
        var ppuY = bY / ppu;

        // TODO: Finish math... need size of the model, etc.

        var aX = anchor.x;
        var aY = anchor.y;

        var pX = sprite.pivot.x;
        var pY = sprite.pivot.y;
        //var pX = sprite.pivot.x - border.x;
        //var pY = sprite.pivot.y - border.y;

        var pvX = (pX / ppu);
        var pvY = (pY / ppu);

        var fX = (pX / ppu) + aX;
        var fY = (pY / ppu) + aY;

        var tileWorld = grid.CellToWorld(tilePos);
        return new Bounds(tileWorld + new Vector3(0, fY, 0), new Vector3(ppuX, ppuY, 0));
    }
}
