using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class Rotater : MonoBehaviour
    {
        [SerializeField]
        public float rotationSpeed = 5f;
        private bool isRotating = false;
        private Vector3 lastMousePosition;

        void Update()
        {
            // Check if right mouse button is down
            if (Input.GetMouseButtonDown(1))
            {
                isRotating = true;
                lastMousePosition = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(1))
            {
                isRotating = false;
            }

            // Rotate cube if right mouse button is held down
            if (isRotating)
            {
                Vector3 currentMousePosition = Input.mousePosition;
                Vector3 mouseDelta = currentMousePosition - lastMousePosition;
                float rotationY = mouseDelta.x * rotationSpeed;
                transform.Rotate(0, rotationY, 0);
                lastMousePosition = currentMousePosition;
            }
        }
    }
}

