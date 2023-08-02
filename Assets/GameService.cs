using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Framework
{
    public abstract class GameService : MonoBehaviour, IResolverProvider
    {
        public IResolver Resolver { get; protected set; }

        protected virtual void Awake()
        {
            Resolver = FindResolver();
            Register();
        }

        protected virtual void Start()
        {
            Setup();
        }

        protected virtual void OnDestroy()
        {
            Cleanup();
        }

        protected virtual void Setup() { }
        protected virtual void Cleanup() { }

        protected virtual IResolver FindResolver()
        {
            return GetComponentInParent<IResolver>();
        }

        protected virtual void Register() { }
    }

    public abstract class GameService<T> : GameService
        where T : class
    {
        protected override void Register()
        {
            Resolver.Register<T>(this);
        }

        protected override void Cleanup()
        {
            Resolver.Unregister<T>();
        }
    }

    public abstract class GameService<T1, T2> : GameService
        where T1 : class
        where T2 : class
    {
        protected override void Register()
        {
            Resolver.Register<T1>(this);
            Resolver.Register<T2>(this);
        }

        protected override void Cleanup()
        {
            Resolver.Unregister<T1>();
            Resolver.Unregister<T2>();
        }
    }
}
