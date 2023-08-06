using UnityEngine;
using UnityEngine.Events;

public class Locomotion : MonoBehaviour
{
    public float MoveSpeed = 1.0f;

    public UnityEvent<Direction> OnDirectionSet;

    public void Move(Direction direction)
    {
        // HACK: We don't yet have a good way to get the direction vector without just relying on Unity calculating the correct forward vector based on our expected rotations.
        // We should improve this.

        transform.rotation = direction.ToRotationAngle();

        var position = transform.position;
        position += transform.forward * MoveSpeed * Time.deltaTime;
        position.z = 0.0f; // HACK: Z seems to drift without this.

        transform.position = position;

        // TODO: Bad practice, just the locomotion store its direction/make it queryable.
        // NO need to fire events per-frame. This'll be expensive for many characters.
        OnDirectionSet?.Invoke(direction);
    }

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
