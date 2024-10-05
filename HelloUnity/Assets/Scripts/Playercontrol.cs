using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public float moveSpeed = 5f;   // Control the movement speed
    public float turnSpeed = 200f; // Control the turning speed

    void Update()
    {
        // Get horizontal and vertical input (WASD keys)
        float horizontal = Input.GetAxis("Horizontal");  // A and D keys
        float vertical = Input.GetAxis("Vertical");      // W and S keys

        // Move forward and backward
        Vector3 movement = transform.forward * vertical * moveSpeed * Time.deltaTime;
        transform.position += movement;

        // Rotate left and right
        transform.Rotate(0, horizontal * turnSpeed * Time.deltaTime, 0);
    }
}

