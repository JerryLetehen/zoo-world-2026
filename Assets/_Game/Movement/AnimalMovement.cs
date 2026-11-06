using System;
using R3;
using UnityEngine;
using ZooWorld.CameraControl;
using ZooWorld.Core;

namespace ZooWorld.Movement
{
    public abstract class AnimalMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody body;

        private IGameTicker ticker;
        private IVisibleArea visibleArea;
        private IDisposable subscription;

        protected Rigidbody Body => body;
        protected Vector3 Heading { get; private set; }

        public void Activate(IGameTicker ticker, IVisibleArea visibleArea, Vector3 initialHeading)
        {
            this.ticker = ticker;
            this.visibleArea = visibleArea;
            Heading = initialHeading.sqrMagnitude > Mathf.Epsilon ? initialHeading.normalized : Vector3.forward;

            StartTicking();
        }

        public void Deactivate() => StopTicking();


        protected abstract void Move(float fixedDeltaTime);

        private void OnDestroy() => StopTicking();

        private void StartTicking()
        {
            if (subscription != null || ticker == null)
            {
                return;
            }

            subscription = ticker.FixedUpdate.Subscribe(OnFixedUpdated);
        }

        private void StopTicking()
        {
            if (subscription == null)
            {
                return;
            }

            subscription.Dispose();
            subscription = null;
        }

        private void OnFixedUpdated(float fixedDeltaTime)
        {
            SteerBackIfOffScreen();
            Move(fixedDeltaTime);
        }

        private void SteerBackIfOffScreen()
        {
            if (visibleArea == null || !visibleArea.TryGetVisibleRect(out var screen))
            {
                return;
            }

            var position = Body.position;
            if (VisibleAreaMath.ContainsXZ(screen, position))
            {
                return;
            }

            var center = screen.center;
            var inward = new Vector3(center.x - position.x, 0f, center.y - position.z);
            if (inward.sqrMagnitude > Mathf.Epsilon)
            {
                Heading = inward.normalized;
            }
        }
    }
}
