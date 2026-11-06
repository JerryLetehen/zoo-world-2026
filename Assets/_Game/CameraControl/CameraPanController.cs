using System;
using R3;
using UnityEngine;
using ZooWorld.Arena;
using ZooWorld.Config;
using ZooWorld.Core;

namespace ZooWorld.CameraControl
{
    public sealed class CameraPanController : IDisposable
    {
        private readonly Camera camera;
        private readonly ICameraPanInput input;
        private readonly IActiveArena activeArena;
        private readonly IConfigProvider<CameraConfig> config;
        private readonly IDisposable subscription;

        public CameraPanController(
            Camera camera,
            ICameraPanInput input,
            IActiveArena activeArena,
            IGameTicker ticker,
            IConfigProvider<CameraConfig> config)
        {
            this.camera = camera;
            this.input = input;
            this.activeArena = activeArena;
            this.config = config;
            subscription = ticker.Update.Subscribe(OnUpdated);
        }

        private void OnUpdated(float deltaTime)
        {
            if (camera == null || !input.IsDragging)
            {
                return;
            }

            var screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
            var dragEnd = screenCenter + input.DragDelta * config.Get().PanSpeed;
            if (!GroundProjection.TryScreenToGround(camera, screenCenter, out var lookPoint)
                || !GroundProjection.TryScreenToGround(camera, dragEnd, out var dragPoint))
            {
                return;
            }

            var pannedLookPoint = lookPoint + (lookPoint - dragPoint);
            var clampedLookPoint = CameraPanMath.ClampToBounds(pannedLookPoint, activeArena.Current);
            var shift = clampedLookPoint - lookPoint;
            camera.transform.position += new Vector3(shift.x, 0f, shift.z);
        }

        public void Dispose()
        {
            subscription.Dispose();
        }
    }
}
