using UnityEngine;
using UnityEngine.AI;

public class Wander : MonoBehaviour
{
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SetRandomDestination(); 
    }

    void Update()
    {
        
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            SetRandomDestination();
        }
    }


    void SetRandomDestination()
{
    Vector3 randomDirection = Random.insideUnitSphere * 200; 
    randomDirection += transform.position;
    NavMeshHit hit;
    if (NavMesh.SamplePosition(randomDirection, out hit, 200, 1))
    {
        Vector3 finalPosition = hit.position;
        agent.SetDestination(finalPosition);
        Debug.Log("New destination set to: " + finalPosition); 
    }
    else
    {
        Debug.Log("Failed to find a valid NavMesh position.");
    }
}

}
