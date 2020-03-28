using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RTSCamera
{
    public class CameraController : MonoBehaviour
    {
        public Transform cameraTransform;

        [Header("Movement")] public float fastSpeed;
        public float slowSpeed;
        private float movementSpeed;
        public float movementTime;

        [Header("Rotation")] public float rotationAmount;

        [Header("Zoom Settings")] public Vector3 zoomAmount;
        public float minZoom;
        public float maxZoom;

        private Vector3 newPosition;
        private Quaternion newRotation;
        private Vector3 newZoom;

        private CameraControls controller = null;
        private bool shiftPressed = false;

        private void Awake()
        {
            controller = new CameraControls();
        }

        private void OnEnable()
        {
            controller.CameraActionMap.Enable();
        }

        private void OnDisable()
        {
            controller.CameraActionMap.Disable();
        }

        private void Start()
        {
            newPosition = transform.position;
            newRotation = transform.rotation;
            newZoom = cameraTransform.localPosition;
        }

        private void Update()
        {
            HandleInput();

            movementSpeed = shiftPressed ? fastSpeed : slowSpeed;
        }

        public void HandleInput()
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
            cameraTransform.localPosition =
                Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);


            newZoom.y = Mathf.Clamp(newZoom.y, minZoom, maxZoom);
            newZoom.z = Mathf.Clamp(newZoom.z, -maxZoom, -minZoom);

            var movementInput = controller.CameraActionMap.MovementControls.ReadValue<Vector2>();

            var movement = new Vector3
            {
                x = movementInput.x,
                z = movementInput.y
            };

            movement.Normalize();
            newPosition += movement * movementSpeed;

            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
            //use if you'd rather more rigid camera movement
            //transform.Translate(movement * movementSpeed * Time.deltaTime);
        }

        public void ZoomIn()
        {
            newZoom += zoomAmount;
        }

        public void ZoomOut()
        {
            newZoom -= zoomAmount;
        }

        public void RotateLeft()
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }

        public void RotateRight()
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }

        public void SpeedUp(InputAction.CallbackContext context)
        {
            // var value = context.ReadValue<bool>();
            // shiftPressed = value == true;

            shiftPressed = !shiftPressed;
        }
    }
}