using UnityEngine;
using UnityEngine.Events;

public class Locomotion : MonoBehaviour
{
    public float MoveSpeed = 1.0f;

    public UnityEvent<Direction> OnDirectionSet;

    public void Move(Vector2 input)
    {
        var pos = transform.position;
        var dir = input.normalized;
        transform.rotation = Quaternion.LookRotation(dir, -Vector3.forward);

        // TODO: Do not use fixed delta time here. It just works because the calling code uses fixed update...

        pos += input.x * Vector3.right * MoveSpeed * Time.fixedDeltaTime;
        pos += input.y * Vector3.up * MoveSpeed * Time.fixedDeltaTime;

        transform.position = pos;

        // hack
        ComputeDirection(input.normalized);
    }

    private void ComputeDirection(Vector2 input)
    {
        // TODO: Works, but needs a big cleanup

        var step = 22.5f;

        var angle = Vector2.SignedAngle(Vector2.up, input);
        var abs = Mathf.Abs(angle);

        if(abs < step)
        {
            InvokeDirection(Direction.North);
        }
        else if (abs < step * 3.0)
        {
            if (angle < 0)
            {
                InvokeDirection(Direction.NorthEast);
            }
            else
            {
                InvokeDirection(Direction.NorthWest);
            }
        }
        else if (abs < step * 5.0)
        {
            if (angle < 0)
            {
                InvokeDirection(Direction.East);
            }
            else
            {
                InvokeDirection(Direction.West);
            }
        }
        else if (abs < step * 7.0)
        {
            if (angle < 0)
            {
                InvokeDirection(Direction.SouthEast);
            }
            else
            {
                InvokeDirection(Direction.SouthWest);
            }
        }
        else
        {
            InvokeDirection(Direction.South);
        }
    }

    private void InvokeDirection(Direction dir)
    {
        OnDirectionSet?.Invoke(dir);
    }
}
