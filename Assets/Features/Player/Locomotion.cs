using UnityEngine;
using UnityEngine.Events;

public class Locomotion : MonoBehaviour
{
    public float MoveSpeed = 1.0f;

    [SerializeField] protected Direction direction = Direction.North;

    public Direction Direction 
    { 
        get 
        { 
            return direction; 
        }
        set
        {
            direction = value;
            transform.rotation = direction.ToRotationAngle();
            InvokeOnDirectionSet(value);
        }
    }

    public UnityEvent<Direction> OnDirectionSet;

    public void Move(float amount)
    {
        var position = transform.position;
        position += transform.forward * amount * MoveSpeed * Time.deltaTime;
        position.z = 0.0f; // HACK: Z seems to drift without this.

        transform.position = position;
    }

    private void InvokeOnDirectionSet(Direction dir)
    {
        OnDirectionSet?.Invoke(dir);
    }
}
