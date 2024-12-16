using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed = 5f; // Player movement speed
    public Transform minBoundary; // Minimum boundary (reference to one corner)
    public Transform maxBoundary; // Maximum boundary (reference to opposite corner)

    private Vector3 movement;

    void Update()
    {
        // Get input from keyboard (WASD or Arrow keys)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calculate movement direction
        movement = new Vector3(horizontal, 0, vertical).normalized;

        // Move the player
        MovePlayer();
    }

    void MovePlayer()
    {
        // Calculate the new position
        Vector3 newPosition = transform.position + movement * moveSpeed * Time.deltaTime;

        // Clamp the position within the boundaries defined by the two Transform objects
        if (minBoundary != null && maxBoundary != null)
        {
            newPosition.x = Mathf.Clamp(newPosition.x, minBoundary.position.x, maxBoundary.position.x);
            newPosition.z = Mathf.Clamp(newPosition.z, minBoundary.position.z, maxBoundary.position.z);
        }

        // Apply the new position to the player
        transform.position = newPosition;
    }
}
