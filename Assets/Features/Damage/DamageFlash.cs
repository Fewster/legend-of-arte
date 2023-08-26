using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    public SpriteRenderer rend;
    public float time;

    public void Flash()
    {
        StartCoroutine(FlashRoutine(time));
    }

    private IEnumerator FlashRoutine(float time)
    {
        var prev = rend.color;

        var t = 0.0f;
        var h = time * 0.5f;
        var m = 1.0f / time;

        while (t < time)
        {
            t += Time.deltaTime;

            var mod = Mathf.PingPong(t, h);
            rend.color = Color.Lerp(prev, Color.red, mod * m);

            yield return null;
        }

        rend.color = prev;
    }
}
