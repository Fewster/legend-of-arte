using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileDimmer : MonoBehaviour
{
    [SerializeField] protected Tilemap map;
    [SerializeField] protected GridLayout layout;
    [SerializeField] protected Tile tile;

    public Color Color;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var pos = layout.WorldToCell(mPos);
        var wPos = layout.CellToWorld(pos);

        Debug.Log(pos);
        return;

        var tile = map.GetTile(pos) as Tile;
        if(tile != null)
        {
            //map.cell

            var ppuNorm = (1.0f / tile.sprite.pixelsPerUnit);
            var texW = tile.sprite.texture.width;
            var texH = tile.sprite.texture.height;

            var anchorX = map.tileAnchor.x;
            var anchorY = map.tileAnchor.y;

            var pW = (texW * anchorX) - tile.sprite.pivot.x;
            var pH = (texH * anchorY) - tile.sprite.pivot.y;

            var oW = layout.cellSize.x * 0.5f;
            var oH = layout.cellSize.y * 0.5f;

            var wPivot = (new Vector2(pW, pH) * ppuNorm) + new Vector2(0.0f, oH);

            Debug.Log(wPivot);

            var bounds = tile.sprite.bounds;
            Debug.DrawLine(wPos, wPos + new Vector3(wPivot.x, wPivot.y, 0), Color.red, 30.0f);

            Debug.DrawLine(bounds.center - bounds.extents, bounds.center + bounds.extents, Color.blue, 30.0f);
        }

        if (Input.GetMouseButtonDown(0))
        {
            

            map.SetTileFlags(pos, TileFlags.None);
            map.SetColor(pos, Color);
        }

        if (Input.GetMouseButtonDown(1))
        {
            map.RefreshAllTiles();
        }
    }
}
