using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public Transform player; // Reference to the player
    public float speed = 3f; // Movement speed
    public float attackDistance = 2f; // Distance to stop moving and attack
    public int health = 15; // Enemy health, requiring 15 hits to die
    private Animator animator; // Animator for controlling animations

    void Start()
    {
        // Get the Animator component attached to the enemy
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player)
        {
            // Calculate the distance to the player
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer > attackDistance)
            {
                // Move towards the player
                Vector3 direction = (player.position - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;

                // Rotate the enemy to face the player
                transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

                // Set Animator parameter for walking
                if (animator) animator.SetFloat("Speed", speed);
            }
            else
            {
                // Stop moving and prepare to attack
                if (animator) animator.SetFloat("Speed", 0);

                // Optional: Add attack logic here (e.g., play attack animation)
            }
        }
    }

    // Method to handle taking damage
    public void TakeDamage(int damage)
    {
        health -= damage; // Subtract damage from health
        Debug.Log(gameObject.name + " took damage. Remaining health: " + health); // Debug log for health

        if (health <= 0)
        {
            Die(); // Call the Die method if health is 0 or below
        }
    }

    // Method to handle the enemy's death
    private void Die()
    {
        Debug.Log(gameObject.name + " has died."); // Debug log for death

        // Notify the EnemySpawner
        EnemySpawner spawner = Object.FindFirstObjectByType<EnemySpawner>();
        if (spawner != null)
        {
            spawner.OnEnemyDefeated();
        }

        // Destroy the enemy GameObject
        Destroy(gameObject);
    }
}
