using LoxQuest3D.Settings;
using UnityEngine;

namespace LoxQuest3D.FPS
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class FpsPlayerController : MonoBehaviour
    {
        public Camera playerCamera;
        public float moveSpeed = 4.2f;
        public float gravity = -18f;
        public float jumpHeight = 1.2f;
        public float lookClamp = 80f;

        private CharacterController _cc;
        private float _pitch;
        private float _yVel;

        private void Awake()
        {
            _cc = GetComponent<CharacterController>();
            if (playerCamera == null)
                playerCamera = GetComponentInChildren<Camera>();
        }

        private void OnEnable()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            var settings = SettingsSystem.Load();
            Look(settings.mouseSensitivity);
            Move();
        }

        private void Look(float sensitivity)
        {
            var mx = Input.GetAxis("Mouse X") * sensitivity;
            var my = Input.GetAxis("Mouse Y") * sensitivity;

            transform.Rotate(Vector3.up, mx, Space.World);

            _pitch -= my;
            _pitch = Mathf.Clamp(_pitch, -lookClamp, lookClamp);
            if (playerCamera != null)
                playerCamera.transform.localRotation = Quaternion.Euler(_pitch, 0, 0);
        }

        private void Move()
        {
            var x = Input.GetAxisRaw("Horizontal");
            var z = Input.GetAxisRaw("Vertical");
            var dir = (transform.right * x + transform.forward * z);
            if (dir.sqrMagnitude > 1) dir.Normalize();

            var grounded = _cc.isGrounded;
            if (grounded && _yVel < 0) _yVel = -2f;

            if (grounded && Input.GetButtonDown("Jump"))
                _yVel = Mathf.Sqrt(jumpHeight * -2f * gravity);

            _yVel += gravity * Time.deltaTime;
            dir = dir * moveSpeed;
            dir.y = _yVel;
            _cc.Move(dir * Time.deltaTime);
        }
    }
}

