using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyCamera : MonoBehaviour
{
    public float movementSpeed = 10.0f; // Speed of camera movement
    public float mouseSensitivity = 2.0f; // Sensitivity of mouse movement
    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    void Start()
    {
        // Lock and hide the mouse cursor in the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Rotate the camera based on mouse movement
        rotationX += Input.GetAxis("Mouse X") * mouseSensitivity;
        rotationY -= Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Clamp vertical rotation to prevent flipping
        rotationY = Mathf.Clamp(rotationY, -90f, 90f);

        // Apply the rotation to the camera
        transform.localRotation = Quaternion.Euler(rotationY, rotationX, 0);

        // Move the camera based on keyboard input
        float moveForward = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;  // W/S keys
        float moveSideways = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime; // A/D keys

        // Move the camera relative to its current direction using world space
        Vector3 move = transform.right * moveSideways + transform.forward * moveForward;
        transform.position += move;
    }
}
