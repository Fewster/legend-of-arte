using UnityEngine;

namespace Game.Framework
{
    /// <summary>
    /// An object which interacts with services.
    /// </summary>
    [DefaultExecutionOrder(-1000)]
    public abstract class GameBehaviour : MonoBehaviour, IResolverProvider
    {
        public IResolver Resolver { get; protected set; }

        protected virtual void Start()
        {
            Resolver = FindResolver();
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
    }
}
