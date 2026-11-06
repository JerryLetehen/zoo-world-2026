using UnityEngine;
using ZooWorld.Config;

namespace ZooWorld.Movement
{
    public sealed class JumpMovement : AnimalMovement
    {
        [SerializeField] private JumpMovementConfigAsset config;

        protected override void Move(float fixedDeltaTime)
        {
            var settings = config.Value;
            if (!IsAtRest(settings.RestSpeedThreshold))
            {
                return;
            }

            var takeoffSpeed = Mathf.Sqrt(2f * Mathf.Abs(Physics.gravity.y) * settings.PeakHeight);
            var impulse = Vector3.up * takeoffSpeed + HorizontalHopVelocity(takeoffSpeed, settings.HopDistance);
            Body.AddForce(impulse * Body.mass, ForceMode.Impulse);
        }

        private bool IsAtRest(float restSpeedThreshold) =>
            Body.linearVelocity.sqrMagnitude <= restSpeedThreshold * restSpeedThreshold;

        private Vector3 HorizontalHopVelocity(float takeoffSpeed, float hopDistance)
        {
            var gravity = Mathf.Abs(Physics.gravity.y);
            var timeToPeak = takeoffSpeed / Mathf.Max(0.0001f, gravity);
            var airTime = 2f * timeToPeak;
            var horizontalSpeed = hopDistance / Mathf.Max(0.0001f, airTime);
            return Heading * horizontalSpeed;
        }
    }
}
