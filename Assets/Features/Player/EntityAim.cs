//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Events;

//public interface IEntityAim
//{
//    void Aim(Vector2 direction);
//}

//public class EntityAim : MonoBehaviour, IEntityAim
//{
//    [SerializeField]
//    protected Entity entity;

//    public UnityEvent<Direction> OnAimDirectionSet;

//    private void Reset()
//    {
//        entity = GetComponent<Entity>();
//    }

//    public void Aim(Vector2 direction)
//    {
//        transform.rotation = Quaternion.LookRotation(direction);

//        var dir = DirectionExtensions.GetNearestDirection(direction.normalized);
//        OnAimDirectionSet?.Invoke(dir);
//    }

//    /*
     
//        public void Aim(Vector2 direction)
//    {
//        var dir = DirectionExtensions.GetNearestDirection(direction.normalized);
//        entity.Direction = dir;
//    } 

//     */
//}