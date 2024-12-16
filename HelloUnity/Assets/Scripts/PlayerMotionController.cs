using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotionController : MonoBehaviour
{
    private Animator animator; // Animator for controlling animations
    private CharacterController controller; // Movement and collision control
    public float movementSpeed = 500f; // Movement speed
    public Transform cameraTransform; // Main camera reference
    public Transform minBoundary; // Minimum boundary
    public Transform maxBoundary; // Maximum boundary

    public Transform sword; // Reference to the player's sword
    public float attackCooldown = 1f; // Time between attacks
    private float lastAttackTime = -1f; // Last attack time

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleMovement();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AttemptAttack();
        }
    }

    void HandleMovement()
    {
        float inputHorizontal = Input.GetAxis("Horizontal");
        float inputVertical = Input.GetAxis("Vertical");

        Vector3 forward = cameraTransform.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = cameraTransform.right;
        right.y = 0;
        right.Normalize();

        Vector3 movement = (forward * inputVertical + right * inputHorizontal).normalized;
        Vector3 move = movement * movementSpeed * Time.deltaTime;
        controller.Move(move);

        Vector3 clampedPosition = transform.position;
        if (minBoundary != null && maxBoundary != null)
        {
            clampedPosition.x = Mathf.Clamp(transform.position.x, minBoundary.position.x, maxBoundary.position.x);
            clampedPosition.z = Mathf.Clamp(transform.position.z, minBoundary.position.z, maxBoundary.position.z);
        }
        transform.position = clampedPosition;
        animator.SetFloat("Speed", movement.magnitude);
    }

    void AttemptAttack()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            PerformAttack();
            lastAttackTime = Time.time;
        }
    }

    void PerformAttack()
    {
        if (animator)
        {
            animator.SetTrigger("Attack");
        }

        // Rotate the sword
        if (sword)
        {
            StartCoroutine(RotateSword());
        }
    }

    IEnumerator RotateSword()
    {
        float rotationTime = 0.5f; // Time for the sword to rotate
        float elapsedTime = 0f;

        while (elapsedTime < rotationTime)
        {
            sword.Rotate(0, 720 * Time.deltaTime, 0); // Rotate sword around Y-axis
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset sword rotation
        sword.localRotation = Quaternion.identity;
    }
}
