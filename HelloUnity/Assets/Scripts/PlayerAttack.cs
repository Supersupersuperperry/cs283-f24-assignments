using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 2f; // The range within which the player can attack
    public KeyCode attackKey = KeyCode.Space; // The key to trigger the attack
    public int attackDamage = 1; // Damage dealt to the enemy per attack

    void Update()
    {
        // Check if the player presses the attack key
        if (Input.GetKeyDown(attackKey))
        {
            PerformAttack();
        }
    }

    void PerformAttack()
    {
        // Detect all colliders within the attack range
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);

        foreach (Collider collider in hitColliders)
        {
            // Check if the collider belongs to an enemy
            if (collider.CompareTag("Enemy"))
            {
                // Attempt to get the EnemyBehavior component
                EnemyBehavior enemy = collider.GetComponent<EnemyBehavior>();
                if (enemy != null)
                {
                    // Deal damage to the enemy
                    enemy.TakeDamage(attackDamage);
                    Debug.Log("Enemy hit: " + collider.gameObject.name + ", Damage: " + attackDamage);
                }
                else
                {
                    Debug.LogWarning("Detected an object with the 'Enemy' tag but no EnemyBehavior component.");
                }

                // Optional: Stop after hitting one enemy
                break;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the attack range in the scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
