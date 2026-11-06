using UnityEngine;

namespace ZooWorld.Animals
{
    /// <summary>
    /// A lightweight, stable reference to a spawned animal used by the resolver, labels, and
    /// spawner. Carries the species definition and identity without exposing the whole view.
    /// </summary>
    public sealed class AnimalHandle
    {
        public AnimalHandle(int id, AnimalDefinition definition, AnimalView view)
        {
            Id = id;
            Definition = definition;
            View = view;
        }

        public int Id { get; }
        public AnimalDefinition Definition { get; }
        public AnimalView View { get; }
        public bool IsAlive { get; private set; } = true;

        public Vector3 Position => View != null ? View.transform.position : Vector3.zero;

        public void MarkDead() => IsAlive = false;
    }
}
