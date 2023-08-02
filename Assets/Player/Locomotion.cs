using UnityEngine;

public class Locomotion : MonoBehaviour
{
    public float MoveSpeed = 1.0f;

    public void Move(Vector2 input)
    {
        var pos = transform.position;

        pos += input.x * transform.right * MoveSpeed * Time.deltaTime;
        pos += input.y * transform.forward * MoveSpeed * Time.deltaTime;

        transform.position = pos;
    }
}
