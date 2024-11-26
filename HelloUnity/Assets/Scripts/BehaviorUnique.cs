using UnityEngine;
using UnityEngine.AI; // Required for NavMeshAgent

public class BehaviorUnique : MonoBehaviour
{
    public Transform player; // Reference to the player's position
    public float followDistance = 5f; // Distance to maintain from the player
    public float speed = 200f; // Speed of the companion

    private NavMeshAgent agent; // NavMeshAgent for movement

    void Start()
    {
        // Get the NavMeshAgent component
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed; // Set the speed of the companion
    }

    void Update()
    {
        FollowPlayer(); // Continuously follow the player
    }

    // Follow the player while maintaining a safe distance
    void FollowPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > followDistance)
        {
            // Move towards the player's position
            agent.SetDestination(player.position);
        }
        else
        {
            // Stop moving if within follow distance
            agent.ResetPath();
        }
    }
}
