using Game.Framework;
using UnityEngine;
using UnityEngine.Events;

public interface IEntityLocomotion
{
    void Aim(Vector2 direction);
    void Move(Vector2 direction);
}

public class Locomotion : MonoBehaviour, IEntityLocomotion
{
    [SerializeField]
    protected Entity entity;

    public float MoveSpeed = 1.0f;

    private void Reset()
    {
        entity = GetComponent<Entity>();
    }

    [System.Obsolete]
    public void Move(float amount)
    {
        var position = transform.position;
        position += transform.forward * amount * MoveSpeed * Time.deltaTime;
        position.z = 0.0f; // HACK: Z seems to drift without this.

        transform.position = position;
    }

    public void Move(Vector2 direction)
    {
        var position = transform.position;
        position += (Vector3)direction * MoveSpeed * Time.deltaTime;
        position.z = 0.0f;

        transform.position = position;
    }

    public void Aim(Vector2 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction);

        var dir = DirectionExtensions.GetNearestDirection(direction.normalized);

        // TODO: We may want to resolve the legs procedurally...

        //OnAimDirectionSet?.Invoke(dir);
    }

    private void Update()
    {
        var wPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var dir = wPos - transform.position;

        if (Input.GetMouseButton(0))
        {
            Move(dir.normalized);
        }

        //transform.rotation = Quaternion.LookRotation(dir);
    }
}
