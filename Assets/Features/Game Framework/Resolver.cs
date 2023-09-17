using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Framework
{
    [DefaultExecutionOrder(-100000)] // TODO: Pick a better order
    public class Resolver : MonoBehaviour, IResolver
    {
        private Dictionary<Type, object> cache;

        [SerializeField] protected Resolver parent;
        [SerializeField] protected bool autoResolveParent;

        private void Awake()
        {
            cache = new Dictionary<Type, object>();
        }

        private void Start()
        {
            if (autoResolveParent && parent == null && transform.parent != null)
            {
                parent = transform.parent.GetComponentInParent<Resolver>();
            }
        }

        public object Resolve(Type type)
        {
            // Prioritise dependencies from ourselves before searching a potential parent
            if (cache.TryGetValue(type, out object result))
            {
                return result;
            }

            if(parent == this)
            {
                Debug.LogError("circular-parent relation detected", this);
                return null;
            }

            // Fallback to resolving items from the parent
            if (parent != null)
            {
                result = parent.Resolve(type);
            }

            return result;
        }

        public void Register(Type type, object target)
        {
            cache.Add(type, target);
        }

        public void Unregister(Type type)
        {
            cache.Remove(type);
        }
    }

    /// <summary>
    /// Provides the means to resolve an object by <see cref="Type"/>.
    /// </summary>
    public interface IResolver
    {
        object Resolve(Type type);
        void Register(Type type, object target);
        void Unregister(Type type);
    }

    /// <summary>
    /// Represents an object which has an <see cref="IResolver"/>.
    /// </summary>
    public interface IResolverProvider
    {
        IResolver Resolver { get; }
    }

    public static class ResolverExtensions
    {
        public static T Resolve<T>(this IResolver resolver)
            where T : class
        {
            var item = resolver.Resolve(typeof(T));
            return item as T;
        }

        public static void Register<T>(this IResolver resolver, object target)
            where T : class
        {
            resolver.Register(typeof(T), target);
        }

        public static void Unregister<T>(this IResolver resolver)
        {
            resolver.Unregister(typeof(T));
        }
    }
}