using UnityEngine;

public class MouseTargetController : MonoBehaviour
{
    // Ensure this field is public or serialized so it appears in the Inspector
    public Camera mainCamera;  // This should be visible in the Inspector

    public float depth = 10.0f; // Depth at which the target will follow the mouse

    void Update()
    {
        // Get the mouse position in screen space
        Vector3 mousePos = Input.mousePosition;

        // Convert the mouse position to world space at a specific depth
        mousePos.z = depth;
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);

        // Move the target object to the mouse's world position
        transform.position = worldPos;
    }
}
