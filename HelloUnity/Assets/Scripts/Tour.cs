using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tour : MonoBehaviour
{
    public Transform[] pointsOfInterest; // Array of POI transforms
    public float moveSpeed = 2.0f; // Speed of camera movement between POIs
    public float rotationSpeed = 2.0f; // Speed of camera rotation between POIs

    private int currentPOIIndex = 0; // Index of the current POI
    private bool isMoving = false; // Flag to indicate if the camera is currently moving
    private Vector3 targetPosition; // Target position of the camera
    private Quaternion targetRotation; // Target rotation of the camera

    void Update()
    {
        // Check for 'N' key press to move to the next POI
        if (Input.GetKeyDown(KeyCode.N) && !isMoving)
        {
            // Increment POI index and wrap around if necessary
            currentPOIIndex = (currentPOIIndex + 1) % pointsOfInterest.Length;
            // Set target position and rotation
            targetPosition = pointsOfInterest[currentPOIIndex].position;
            targetRotation = pointsOfInterest[currentPOIIndex].rotation;
            isMoving = true; // Start moving
        }

        // If the camera is moving, perform the transition
        if (isMoving)
        {
            // Move towards the target position using Lerp
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Rotate towards the target rotation using Slerp
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Check if the camera has reached the target position and rotation
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f && Quaternion.Angle(transform.rotation, targetRotation) < 1.0f)
            {
                isMoving = false; // Stop moving once the target is reached
            }
        }
    }
}
