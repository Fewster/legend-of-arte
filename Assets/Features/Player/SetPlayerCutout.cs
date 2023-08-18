using UnityEngine;
using UnityEngine.Tilemaps;

public class SetPlayerCutout : MonoBehaviour
{
    public TilemapRenderer sRend;
    public float Radius = 1.0f;

    private void Update()
    {
        var pos = transform.position;
        var mat = sRend.material;
        mat.SetVector("_CutoutPosition", new Vector4(pos.x, pos.y, pos.z, Radius));
    }
}
