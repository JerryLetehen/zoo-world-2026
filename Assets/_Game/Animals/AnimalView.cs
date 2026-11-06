using System;
using UnityEngine;
using ZooWorld.CameraControl;
using ZooWorld.Core;
using ZooWorld.Movement;

namespace ZooWorld.Animals
{
    public sealed class AnimalView : MonoBehaviour
    {
        [SerializeField] private Rigidbody body;
        [SerializeField] private Collider bodyCollider;
        [SerializeField] private AnimalMovement movement;

        public AnimalHandle Handle { get; private set; }
        public Rigidbody Body => body;
        public Collider BodyCollider => bodyCollider;

        public event Action<AnimalView, Rigidbody> AnimalCollision;

        public void Activate(
            AnimalHandle handle,
            IGameTicker ticker,
            IVisibleArea visibleArea,
            Vector3 position,
            Vector3 initialHeading)
        {
            Handle = handle;

            transform.position = position;
            body.position = position;
            body.isKinematic = false;
            body.linearVelocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;
            body.detectCollisions = true;
            bodyCollider.enabled = true;

            movement.Activate(ticker, visibleArea, initialHeading);
        }

        public void Deactivate()
        {
            movement.Deactivate();

            body.linearVelocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;
            body.detectCollisions = false;
            body.isKinematic = true;
            bodyCollider.enabled = false;
        }

        private void OnCollisionEnter(UnityEngine.Collision collision)
        {
            if (collision.rigidbody != null)
            {
                AnimalCollision?.Invoke(this, collision.rigidbody);
            }
        }
    }
}
