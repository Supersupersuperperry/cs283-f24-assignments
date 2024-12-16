using UnityEngine;
using UnityEngine.UI; // To use Canvas and CanvasScaler
using TMPro; // To use TextMeshProUGUI

public class PlayerHealth : MonoBehaviour
{
    public int health = 20; // Player health
    public GameObject gameOverUI; // Reference to the Game Over UI

    private TextMeshProUGUI healthText; // Health display text
    private Transform mainCamera; // Reference to the main camera
    private GameObject canvasObject; // Player's canvas for health text

    void Start()
    {
        // Find and assign the Main Camera
        if (Camera.main != null)
        {
            mainCamera = Camera.main.transform;
        }
        else
        {
            Debug.LogError("Main Camera not found! Please ensure a camera exists and is tagged as 'MainCamera'.");
            return;
        }

        // Create a new Canvas to display health
        canvasObject = new GameObject("PlayerCanvas");
        canvasObject.transform.SetParent(transform);
        canvasObject.transform.localPosition = new Vector3(0, 150f, 0); // Position above the player
        canvasObject.transform.localScale = Vector3.one * 0.01f; // Adjust size to be visible in world space

        // Add Canvas component
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace; // Render the Canvas in world space

        // Add CanvasScaler to handle scaling
        CanvasScaler canvasScaler = canvasObject.AddComponent<CanvasScaler>();
        canvasScaler.dynamicPixelsPerUnit = 10;

        // Create the Text Object for health display
        GameObject textObject = new GameObject("HealthText");
        textObject.transform.SetParent(canvasObject.transform);
        textObject.transform.localPosition = Vector3.zero; // Center text in canvas

        // Add TextMeshProUGUI and configure it
        healthText = textObject.AddComponent<TextMeshProUGUI>();
        healthText.text = $"HP: {health}";
        healthText.fontSize = 20;
        healthText.color = Color.green;
        healthText.alignment = TextAlignmentOptions.Center;

        // Assign default font to avoid missing font issues
        healthText.font = TMP_Settings.defaultFontAsset;

        // Hide the Game Over UI initially
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }

        Debug.Log("Player health UI initialized successfully.");
    }

    void LateUpdate()
    {
        // Ensure the canvas always faces the main camera
        if (canvasObject != null && mainCamera != null)
        {
            canvasObject.transform.LookAt(mainCamera);
            canvasObject.transform.Rotate(0, 180, 0);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage; // Reduce health by damage amount

        // Update the displayed health text
        if (healthText != null)
        {
            healthText.text = $"HP: {health}";
        }

        Debug.Log($"Player took {damage} damage. Remaining health: {health}");

        // Check for death
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player has died.");

        // Show the Game Over UI
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }

        // Destroy the health display canvas and player object
        if (canvasObject != null)
        {
            Destroy(canvasObject);
        }
        Destroy(gameObject);
    }

    // Heal the player by a specified amount
    public void Heal(int healAmount)
    {
        health += healAmount; // Increase health by healAmount

        // Ensure health does not exceed maximum value
        if (health > 20)
        {
            health = 20;
        }

        // Update the displayed health text
        if (healthText != null)
        {
            healthText.text = $"HP: {health}";
        }

        Debug.Log($"Player healed by {healAmount}. Current health: {health}");
    }
}
