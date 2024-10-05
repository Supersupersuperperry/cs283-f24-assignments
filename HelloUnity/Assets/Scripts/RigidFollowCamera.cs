using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidFollowCamera : MonoBehaviour
{
    public Transform target;  // Target player
    public float horizontalDistance = 5f;  // Horizontal distance between camera and player
    public float verticalDistance = 2f;    // Vertical distance between camera and player

    void LateUpdate()
    {
        // Get the target's position, forward, and up vectors
        Vector3 targetPos = target.position;
        Vector3 targetForward = target.forward;
        Vector3 targetUp = target.up;

        // Calculate the desired camera position
        Vector3 desiredPosition = targetPos - targetForward * horizontalDistance + targetUp * verticalDistance;
        transform.position = desiredPosition;

        // Calculate the direction the camera should point
        Vector3 cameraForward = targetPos - desiredPosition;
        transform.rotation = Quaternion.LookRotation(cameraForward);
    }
}

