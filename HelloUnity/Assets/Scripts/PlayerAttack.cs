using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 2f; // Attack range
    public KeyCode attackKey = KeyCode.Space; // Key to trigger attack
    public float attackCooldown = 1f; // Time between attacks
    private float lastAttackTime = -1f; // Tracks the last attack time

    private Animator animator; // Animator for attack animations

    void Start()
    {
        // Get the Animator component
        animator = GetComponent<Animator>();
    }

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
        // Check if the cooldown period has passed
        if (Time.time - lastAttackTime < attackCooldown)
        {
            Debug.Log("Attack is on cooldown.");
            return; // Exit if cooldown is not finished
        }

        // Update the last attack time
        lastAttackTime = Time.time;

        // Play attack animation
        if (animator)
        {
            animator.SetTrigger("Attack");
        }

        // Detect all colliders within the attack range
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);

        foreach (Collider collider in hitColliders)
        {
            // Check if the collider belongs to an enemy
            if (collider.CompareTag("Enemy"))
            {
                // Get the EnemyBehavior component
                EnemyBehavior enemy = collider.GetComponent<EnemyBehavior>();
                if (enemy != null)
                {
                    // Deal damage to the enemy
                    enemy.TakeDamage(1); // You can adjust the damage value
                }

                // Stop after hitting one enemy (optional)
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
