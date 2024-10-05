using UnityEngine;

public class SpringFollowCamera : MonoBehaviour
{
    public Transform target;  // Target player
    public float horizontalDistance = 5f;  // Desired horizontal distance from the player
    public float verticalDistance = 2f;    // Desired vertical distance from the player
    public float springConstant = 50f;     // Spring constant (controls how fast the camera adjusts)
    public float dampConstant = 10f;       // Damping constant (controls how much the spring motion is dampened)

    private Vector3 velocity = Vector3.zero;  // Camera's velocity for smooth motion
    private GameObject lastHitObject = null;  // Store the last object that was made transparent
    private Material originalMaterial;        // Store the original material of the object

    void LateUpdate()
    {
        // If the spacebar is pressed, immediately reset the camera position behind the player
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetCameraPosition();
            return; // Exit the update early to prevent further spring dynamics calculations this frame
        }

        // Smooth follow logic (same as before)
        Vector3 targetPos = target.position;
        Vector3 targetForward = target.forward;
        Vector3 targetUp = target.up;

        Vector3 idealPosition = targetPos - targetForward * horizontalDistance + targetUp * verticalDistance;
        Vector3 displacement = transform.position - idealPosition;
        Vector3 springAcceleration = (-springConstant * displacement) - (dampConstant * velocity);

        velocity += springAcceleration * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;

        Vector3 cameraForward = targetPos - transform.position;
        transform.rotation = Quaternion.LookRotation(cameraForward);

        // Raycasting to detect any object blocking the camera's view
        RaycastHit hit;
        if (Physics.Raycast(transform.position, targetPos - transform.position, out hit))
        {
            if (hit.collider.gameObject != target.gameObject)
            {
                // If the object is not the player, make it transparent
                MakeObjectTransparent(hit.collider.gameObject);
            }
        }
        else if (lastHitObject != null)
        {
            // Restore the original material if there's no object blocking the view
            RestoreObjectTransparency();
        }
    }

    // Function to reset the camera's position behind the player
    void ResetCameraPosition()
    {
        // Immediately place the camera behind the player
        Vector3 targetPos = target.position;
        Vector3 targetForward = target.forward;
        Vector3 targetUp = target.up;

        // Set the camera's position directly behind the player
        Vector3 resetPosition = targetPos - targetForward * horizontalDistance + targetUp * verticalDistance;
        transform.position = resetPosition;

        // Set the camera's rotation to look directly at the player
        Vector3 cameraForward = targetPos - resetPosition;
        transform.rotation = Quaternion.LookRotation(cameraForward);

        // Reset velocity to prevent spring effects after reset
        velocity = Vector3.zero;
    }

    void MakeObjectTransparent(GameObject obj)
    {
        Debug.Log("Making object transparent: " + obj.name);  // 调试输出，显示当前要变透明的物体

        // If there is already an object made transparent, restore its original material first
        if (lastHitObject != null && lastHitObject != obj)
        {
            RestoreObjectTransparency();
        }

        // Change the object's material to a transparent one
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            originalMaterial = renderer.material;
            Material transparentMaterial = new Material(originalMaterial);

            // Check if the material uses the Standard Shader
            if (transparentMaterial.shader == Shader.Find("Standard"))
            {
                // Set the render mode of the material to transparent
                transparentMaterial.SetFloat("_Mode", 3);  // Transparent mode
                transparentMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                transparentMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                transparentMaterial.SetInt("_ZWrite", 0);  // Disable depth writing for transparency
                transparentMaterial.DisableKeyword("_ALPHATEST_ON");
                transparentMaterial.EnableKeyword("_ALPHABLEND_ON");
                transparentMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                transparentMaterial.renderQueue = 3000;  // Ensure it renders after opaque objects

                // Set color to 30% opacity
                Color color = transparentMaterial.color;
                transparentMaterial.color = new Color(color.r, color.g, color.b, 0.3f);
                Debug.Log("Material successfully set to transparent.");  // 确认材质被修改
            }

            renderer.material = transparentMaterial;  // Apply the transparent material
            lastHitObject = obj;
        }
        else
        {
            Debug.Log("Renderer not found for object: " + obj.name);  // 调试信息，确保物体有 Renderer 组件
        }
    }


    void RestoreObjectTransparency()
    {
        if (lastHitObject != null)
        {
            Renderer renderer = lastHitObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = originalMaterial;  // Restore original material
            }
            lastHitObject = null;
        }
    }
}
