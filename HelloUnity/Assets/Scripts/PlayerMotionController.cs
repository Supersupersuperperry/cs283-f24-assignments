using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotionController : MonoBehaviour
{
    // Animator component for handling animations
    private Animator animator;
    // CharacterController component for handling movement and collisions
    private CharacterController controller;
    // Variable to track the player's movement speed
    private float speed = 0f;
    // Movement speed multiplier
    public float movementSpeed = 500f;
    // Jump force (if needed)
    public float jumpHeight = 2.0f;
    // Ground check
    private bool isGrounded;

    void Start()
    {
        // Get the Animator component attached to the player
        animator = GetComponent<Animator>();
        // Get the CharacterController component attached to the player
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Check if the player is on the ground
        isGrounded = controller.isGrounded;

        // Check user input for movement
        float moveForward = Input.GetAxis("Vertical"); // Get forward/backward movement input
        float moveSide = Input.GetAxis("Horizontal");  // Get left/right movement input

        // Calculate movement direction
        Vector3 movement = new Vector3(moveSide, 0, moveForward);
        speed = movement.magnitude; // Calculate the movement speed based on input

        // Set the Speed parameter in the Animator to control animation states
        animator.SetFloat("Speed", speed);

        // Normalize movement direction and apply speed
        Vector3 move = movement.normalized * movementSpeed * Time.deltaTime;

        // Jump input (if you want to keep jump functionality)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            move.y = jumpHeight; // Apply upward movement for jump
        }

        // Move the player using CharacterController
        controller.Move(move);
    }
}
