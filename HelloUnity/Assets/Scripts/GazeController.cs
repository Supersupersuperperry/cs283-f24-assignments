using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeController : MonoBehaviour
{
    public Transform target;            // The target object to look at (your mouse-controlled cube)
    public Transform lookJoint;         // The joint that should look at the target (e.g., head or eyes)
    public float rotationSpeed = 5.0f;  // Speed of rotation towards the target

    void Update()
    {
        if (target != null && lookJoint != null)
        {
            // Calculate direction to the target
            Vector3 direction = target.position - lookJoint.position;

            // Calculate the rotation needed to look at the target
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Smoothly rotate the joint towards the target
            lookJoint.rotation = Quaternion.Slerp(lookJoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Optional: Visualize the line between the joint and the target using Debug.DrawLine
            Debug.DrawLine(lookJoint.position, target.position, Color.red);
        }
    }
}
