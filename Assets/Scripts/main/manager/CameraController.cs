using System;
using Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace main.manager
{
    public class CameraController : MonoBehaviour
    {
        private const float LEFT = -2.12f, RIGHT = 2.12f, TOP = 1.16f, BOTTOM = -1.13f, SPEED = 5f;
        private Camera _camera;

        private void Start()
        {
            _camera = GetComponent<Camera>();
        }

        private void FixedUpdate()
        {
            if (!GameSettingsManager.enableCameraFollow) return;

            var mousePosition = Mouse.current.position.ReadValue();
            var inWorldPosition = _camera.ScreenToViewportPoint(mousePosition);

            var targetPosition = new Vector3(
                Math.Clamp(inWorldPosition.x, LEFT, RIGHT),
                Math.Clamp(inWorldPosition.y, BOTTOM, TOP),
                -10
            );

            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                SPEED
            );
        }
    }
}