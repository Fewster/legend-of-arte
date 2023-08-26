using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpriteColour : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        var h = Random.Range(0.0f, 1.0f);
        var col = Color.HSVToRGB(h, 1.0f, 1.0f);

        spriteRenderer.color = col;
    }
}
