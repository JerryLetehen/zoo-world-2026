using System;
using UnityEngine;
using ZooWorld.CameraControl;
using ZooWorld.Config;
using ZooWorld.Core;
using ZooWorld.Pooling;

namespace ZooWorld.Animals
{
    public sealed class AnimalFactory : IAnimalFactory
    {
        private readonly IPrefabPool<AnimalView> pool;
        private readonly IGameTicker ticker;
        private readonly IConfigProvider<PhysicsConfig> physics;
        private readonly IVisibleArea visibleArea;
        private PhysicsMaterial sharedMaterial;
        private int nextId;

        public AnimalFactory(
            IPrefabPool<AnimalView> pool,
            IGameTicker ticker,
            IConfigProvider<PhysicsConfig> physics,
            IVisibleArea visibleArea)
        {
            this.pool = pool;
            this.ticker = ticker;
            this.physics = physics;
            this.visibleArea = visibleArea;
        }

        public AnimalHandle Create(AnimalDefinition definition, Vector3 position)
        {
            if (definition.Prefab == null)
            {
                throw new InvalidOperationException(
                    $"Animal definition '{definition.name}' has no prefab assigned.");
            }

            var physicsConfig = physics.Get();
            var view = pool.Get(definition.Prefab);

            ConfigureBody(view.Body, physicsConfig);

            view.BodyCollider.enabled = true;
            view.BodyCollider.material = SharedMaterial(physicsConfig);
            var handle = new AnimalHandle(nextId++, definition, view);

            view.Activate(handle, ticker, visibleArea, position, RandomHeading());

            return handle;
        }

        private static void ConfigureBody(Rigidbody body, PhysicsConfig physicsConfig)
        {
            body.useGravity = true;
            body.collisionDetectionMode = physicsConfig.DetectionMode;
            body.interpolation = physicsConfig.Interpolation;
            body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        private PhysicsMaterial SharedMaterial(PhysicsConfig physicsConfig)
        {
            if (sharedMaterial == null)
            {
                sharedMaterial = new PhysicsMaterial("ZooWorldAnimal")
                {
                    bounciness = physicsConfig.Bounciness,
                    dynamicFriction = physicsConfig.Friction,
                    staticFriction = physicsConfig.Friction,
                };
            }

            return sharedMaterial;
        }

        private static Vector3 RandomHeading()
        {
            var angle = UnityEngine.Random.Range(0f, Mathf.PI * 2f);
            return new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
        }
    }
}
