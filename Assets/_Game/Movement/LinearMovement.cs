using UnityEngine;
using ZooWorld.Config;

namespace ZooWorld.Movement
{
    public sealed class LinearMovement : AnimalMovement
    {
        [SerializeField] private LinearMovementConfigAsset config;

        protected override void Move(float fixedDeltaTime)
        {
            var speed = config.Value.Speed;
            var velocity = Body.linearVelocity;
            velocity.x = Heading.x * speed;
            velocity.z = Heading.z * speed;
            Body.linearVelocity = velocity;
        }
    }
}
