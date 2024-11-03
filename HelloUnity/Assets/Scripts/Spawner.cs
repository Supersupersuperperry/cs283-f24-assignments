using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject collectablePrefab; // Prefab for the collectable object
    public float spawnRange = 5.0f; // Range within which objects will spawn
    public int maxSpawnCount = 10; // Maximum number of spawned objects
    public float spawnInterval = 2.0f; // Interval time (in seconds) between each spawn

    private int currentSpawnCount = 0; // Current count of spawned objects
    private float spawnTimer = 0f; // Timer to track time for next spawn

    private void Update()
    {
        if (currentSpawnCount < maxSpawnCount)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnInterval)
            {
                SpawnCollectable();
                spawnTimer = 0f; // Reset timer
            }
        }

        foreach (Transform child in transform)
        {
            if (!child.gameObject.activeInHierarchy) // If object has been collected (inactive)
            {
                Vector3 newPosition = GetRandomPosition();
                child.position = newPosition;
                child.gameObject.SetActive(true);
            }
        }
    }

    private void SpawnCollectable()
    {
        Vector3 spawnPosition = GetRandomPosition();
        GameObject collectable = Instantiate(collectablePrefab, spawnPosition, Quaternion.identity);
        
        // Set the tag of the spawned object to "Collectable"
        collectable.tag = "Collectable";
        
        collectable.SetActive(true); // Ensure the object is visible
        currentSpawnCount++; // Increase current count
    }

    private Vector3 GetRandomPosition()
    {
        float x = Random.Range(-spawnRange, spawnRange);
        float z = Random.Range(-spawnRange, spawnRange);
        return new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
    }
}
