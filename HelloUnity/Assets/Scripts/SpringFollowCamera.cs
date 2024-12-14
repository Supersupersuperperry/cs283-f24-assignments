using UnityEngine;

public class TPSCameraController : MonoBehaviour
{
    public Transform player;         // Reference to the player
    public float distance = 5f;      // Distance behind the player
    public float height = 2f;        // Height above the player
    public float rotationSpeed = 5f; // Speed of camera rotation
    public float pitchMin = -30f;    // Minimum pitch angle
    public float pitchMax = 60f;     // Maximum pitch angle

    private float currentYaw = 0f;   // Current horizontal rotation (yaw)
    private float currentPitch = 0f; // Current vertical rotation (pitch)

    void LateUpdate()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X"); // Horizontal mouse movement
        float mouseY = Input.GetAxis("Mouse Y"); // Vertical mouse movement

        // Update yaw and pitch based on mouse input
        currentYaw += mouseX * rotationSpeed;
        currentPitch -= mouseY * rotationSpeed; // Subtract to invert vertical control

        // Clamp pitch to stay within specified range
        currentPitch = Mathf.Clamp(currentPitch, pitchMin, pitchMax);

        // Rotate the player to match the camera's horizontal direction
        player.rotation = Quaternion.Euler(0f, currentYaw, 0f);

        // Calculate camera position
        Vector3 targetPosition = player.position 
                                - Quaternion.Euler(0f, currentYaw, 0f) * Vector3.forward * distance
                                + Vector3.up * height;

        // Update camera position and rotation
        transform.position = targetPosition;
        transform.LookAt(player.position + Vector3.up * height / 2f); // Focus slightly above the player's position
    }
}
