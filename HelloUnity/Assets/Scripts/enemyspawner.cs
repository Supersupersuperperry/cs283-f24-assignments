using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab of the enemy
    public Transform player; // Reference to the player
    public float spawnInterval = 2f; // Time interval between spawns
    public int maxEnemies = 5; // Maximum number of enemies in the scene

    private int currentEnemyCount = 0; // Track the current number of enemies

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if (currentEnemyCount < maxEnemies)
            {
                // Spawn an enemy
                GameObject enemy = Instantiate(enemyPrefab, GetRandomSpawnPosition(), Quaternion.identity);

                // Assign player reference to the enemy
                if (enemy.TryGetComponent(out EnemyBehavior enemyBehavior))
                {
                    enemyBehavior.player = player;
                }

                currentEnemyCount++;
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        float spawnRadius = 10f;
        Vector3 randomPos = Random.insideUnitSphere * spawnRadius;
        randomPos.y = 0; // Keep the enemy on the ground
        return transform.position + randomPos;
    }

    public void OnEnemyDefeated()
    {
        currentEnemyCount--;
    }
}
