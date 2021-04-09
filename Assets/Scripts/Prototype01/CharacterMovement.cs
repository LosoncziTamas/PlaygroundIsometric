using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Prototype01
{
    public class CharacterMovement : MonoBehaviour
    {
        private MouseInput _mouseInput;

        private void Awake()
        {
            _mouseInput = new MouseInput();
        }

        private void Start()
        {
            _mouseInput.Mouse.MouseClick.performed += OnMouseClick;
        }

        private void OnMouseClick(InputAction.CallbackContext obj)
        {
            var mousePos = _mouseInput.Mouse.MousePosition.ReadValue<Vector2>();
            Debug.Log(mousePos);
        }

        private void OnEnable()
        {
            _mouseInput.Enable();
        }

        private void OnDisable()
        {
            _mouseInput.Disable();
        }
    }
}
