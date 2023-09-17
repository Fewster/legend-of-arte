using Game.Framework;
using UnityEngine;
using UnityEngine.Events;

public class Locomotion : GameBehaviour
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

    public void Move(Vector2 amount)
    {
        var position = transform.position;
        position += (Vector3)amount * MoveSpeed * Time.deltaTime;
        position.z = 0.0f;

        transform.position = position;
        transform.rotation = Quaternion.LookRotation(amount);

        var dir = DirectionExtensions.GetNearestDirection(amount.normalized);
        entity.Direction = dir;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var wPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var dir = wPos - transform.position;

            Debug.DrawLine(transform.position, transform.position + dir);

            Move(dir.normalized);
        }
    }
}
