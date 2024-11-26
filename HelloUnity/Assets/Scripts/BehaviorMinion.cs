using System.Collections;
using UnityEngine;
using UnityEngine.AI; // Required for NavMeshAgent
using BTAI;

public class BehaviorMinion : MonoBehaviour
{
    public Transform player; // Reference to the player's position
    public Transform homeArea; // Reference to the home area of the minion
    public Transform wanderRange; // Reference to the wandering range
    public float attackRange = 5f; // Distance to trigger attack behavior
    public float homeRange = 10f; // Distance to trigger retreat behavior

    private Root m_btRoot = BT.Root(); // Root of the behavior tree

    void Start()
    {
        // Build the behavior tree
        m_btRoot.OpenBranch(
            // Attack behavior
            BT.If(IsPlayerInRange).OpenBranch(
                BT.Call(AttackPlayer)
            ),
            // Retreat behavior
            BT.If(IsPlayerAtHome).OpenBranch(
                BT.Call(RetreatToHome)
            ),
            // Follow player behavior
            BT.If(ShouldFollowPlayer).OpenBranch(
                BT.Call(() => StartCoroutine(FollowPlayer()))
            ),
            // Wander behavior
            BT.Call(() => StartCoroutine(Wander()))
        );
    }

    void Update()
    {
        // Update the behavior tree every frame
        m_btRoot.Tick();
    }

    // Check if the player is within attack range
    bool IsPlayerInRange()
    {
        return Vector3.Distance(transform.position, player.position) <= attackRange;
    }

    // Check if the player is within the home area
    bool IsPlayerAtHome()
    {
        return Vector3.Distance(player.position, homeArea.position) <= homeRange;
    }

    // Check if the minion should follow the player
    bool ShouldFollowPlayer()
    {
        return !IsPlayerAtHome() && !IsPlayerInRange();
    }

    // Attack the player behavior
    void AttackPlayer()
    {
        Debug.Log("Attacking Player!"); // Debug log to confirm attack

        // Trigger the attack animation
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Attack"); // Ensure this matches the Animator parameter
        }
        else
        {
            Debug.LogWarning("No Animator found on Minion!");
        }

        // Optional: Add attack effects, such as sound or particle effects
    }

    // Retreat to the home area
    void RetreatToHome()
    {
        Debug.Log("Retreating to Home!"); // Debugging message
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(homeArea.position); // Move to the home area
    }

    // Coroutine: Follow the player
    IEnumerator FollowPlayer()
    {
        Debug.Log("Following Player..."); // Debugging message
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        while (ShouldFollowPlayer())
        {
            agent.SetDestination(player.position); // Update destination to player's position
            yield return new WaitForSeconds(0.1f); // Wait before checking again
        }
    }

    // Coroutine: Wander randomly within the area
    IEnumerator Wander()
    {
        Debug.Log("Wandering..."); // Debugging message
        NavMeshAgent agent = GetComponent<NavMeshAgent>();

        // Generate a random target position within the wander range
        Vector3 target;
        Utils.RandomPointOnTerrain(wanderRange.position, wanderRange.localScale.x, out target);
        agent.SetDestination(target); // Move to the target

        // Wait until the target is reached
        while (agent.remainingDistance > 0.1f)
        {
            yield return null; // Wait for the agent to reach the target
        }

        Debug.Log("Finished Wandering"); // Debugging message
    }
}
