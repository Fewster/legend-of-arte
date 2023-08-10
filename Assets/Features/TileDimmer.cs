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
        if (Input.GetMouseButtonDown(0))
        {
            var mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var pos = layout.WorldToCell(mPos);

            map.SetTileFlags(pos, TileFlags.None);
            map.SetColor(pos, Color);
        }

        if (Input.GetMouseButtonDown(1))
        {
            map.RefreshAllTiles();
        }
    }
}
