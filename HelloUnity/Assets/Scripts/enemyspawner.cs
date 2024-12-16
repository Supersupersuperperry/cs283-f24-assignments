using System.Collections;
using UnityEngine;
using TMPro; // Use TextMeshPro for UI

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // The prefab of the enemy to spawn
    public Transform player; // Reference to the player
    public TextMeshProUGUI defeatCounterText; // TextMeshProUGUI for defeat count display
    public float spawnInterval = 3f; // Interval between enemy spawns

    private int defeatedEnemies = 0; // Counter for the number of defeated enemies

    void Start()
    {
        // Initialize the defeat counter display
        UpdateDefeatCounter();

        // Start spawning enemies repeatedly
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true) // Infinite loop to spawn enemies
        {
            SpawnEnemy(); // Spawn a new enemy
            yield return new WaitForSeconds(spawnInterval); // Wait for the specified interval before spawning the next one
        }
    }

    void SpawnEnemy()
    {
        // Instantiate a new enemy at the spawner's position
        GameObject newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

        // Get the EnemyBehavior component from the spawned enemy
        EnemyBehavior enemyBehavior = newEnemy.GetComponent<EnemyBehavior>();
        if (enemyBehavior != null)
        {
            enemyBehavior.player = player; // Assign the player as the enemy's target
            enemyBehavior.onEnemyDefeated += OnEnemyDefeated; // Subscribe to the onEnemyDefeated event
        }
    }

    public void OnEnemyDefeated()
    {
        // Increment the count of defeated enemies
        defeatedEnemies++;

        // Update the defeat counter display
        UpdateDefeatCounter();
    }

    void UpdateDefeatCounter()
    {
        // If the defeat counter text is assigned, update its display
        if (defeatCounterText != null)
        {
            defeatCounterText.text = $"Defeated: {defeatedEnemies}";
        }
    }
}
