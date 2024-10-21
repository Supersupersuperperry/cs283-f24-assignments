using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoLinkController : MonoBehaviour
{
    public Transform target;            // The target for the end effector to reach
    public Transform endEffector;       // The end effector (e.g., hand or foot)
    public Transform midJoint;          // The middle joint (e.g., elbow or knee)
    public Transform baseJoint;         // The base joint (e.g., shoulder or hip)
    public float bendFactor = 0.5f;     // Factor controlling the bend of the mid joint

    void Update()
    {
        if (target != null && endEffector != null && midJoint != null && baseJoint != null)
        {
            // Calculate the direction from base joint to target
            Vector3 baseToTarget = target.position - baseJoint.position;

            // Adjust the middle joint (e.g., elbow or knee) position for a bend effect
            Vector3 bendDirection = Vector3.Lerp(baseToTarget.normalized, (target.position - midJoint.position).normalized, bendFactor);
            float distance = Vector3.Distance(baseJoint.position, target.position);
            midJoint.position = baseJoint.position + bendDirection * distance * bendFactor;

            // Move the end effector (e.g., hand or foot) to the target position
            endEffector.position = target.position;

            // Optional: Visualize the connections with Debug.DrawLine
            Debug.DrawLine(baseJoint.position, midJoint.position, Color.green);
            Debug.DrawLine(midJoint.position, endEffector.position, Color.blue);
        }
    }
}
