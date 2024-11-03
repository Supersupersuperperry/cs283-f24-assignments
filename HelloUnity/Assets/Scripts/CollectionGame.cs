using UnityEngine;
using TMPro; // 使用 TextMeshPro 命名空间

public class CollectionGame : MonoBehaviour
{
    public TMP_Text collectionCountText; 
    private int collectionCount = 0; // Initial collection count

    private void Start()
    {
        UpdateCollectionCount(); // Initialize UI display
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            collectionCount++; // Increase the collection count
            UpdateCollectionCount(); // Update the UI display
            other.gameObject.SetActive(false); // Hide the collected object
            PlayCollectionEffect(other.transform.position); // Play collection effect
        }
    }

    private void UpdateCollectionCount()
    {
        collectionCountText.text = "Collected: " + collectionCount;
    }

    private void PlayCollectionEffect(Vector3 position)
    {
        // Placeholder for collection effect
    }
}
