using UnityEngine;

public class Locomotion : MonoBehaviour
{
    public float MoveSpeed = 1.0f;

    public void Move(Vector2 input)
    {
        var pos = transform.position;

        // TODO: Do not use fixed delta time here. It just works because the calling code uses fixed update...

        pos += input.x * transform.right * MoveSpeed * Time.fixedDeltaTime;
        pos += input.y * transform.up * MoveSpeed * Time.fixedDeltaTime;

        transform.position = pos;
    }
}
