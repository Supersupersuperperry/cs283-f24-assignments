using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotionController : MonoBehaviour
{
    // Animator component for handling animations
    private Animator animator;
    // CharacterController component for handling movement and collisions
    private CharacterController controller;
    // Movement speed multiplier
    public float movementSpeed = 500f;
    // Reference to the main camera
    public Transform cameraTransform;

    void Start()
    {
        // Get the Animator component attached to the player
        animator = GetComponent<Animator>();
        // Get the CharacterController component attached to the player
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Check user input for movement
        float inputHorizontal = Input.GetAxis("Horizontal"); // Get horizontal input (A/D or Left/Right)
        float inputVertical = Input.GetAxis("Vertical");     // Get vertical input (W/S or Up/Down)

        // Get camera's forward and right directions
        Vector3 forward = cameraTransform.forward;
        forward.y = 0; // Ignore Y-axis for horizontal movement
        forward.Normalize();

        Vector3 right = cameraTransform.right;
        right.y = 0; // Ignore Y-axis for horizontal movement
        right.Normalize();

        // Calculate movement direction relative to the camera
        Vector3 movement = (forward * inputVertical + right * inputHorizontal).normalized;

        // Apply movement speed
        Vector3 move = movement * movementSpeed * Time.deltaTime;

        // Move the player using CharacterController
        controller.Move(move);

        // Set the Speed parameter in the Animator to control animation states
        animator.SetFloat("Speed", movement.magnitude);
    }
}
