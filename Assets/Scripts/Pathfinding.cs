using UnityEngine;
using UnityEngine.AI;

public class Pathfinding : MonoBehaviour
{
    public void SetDestination(Vector3 position)
    {
        var agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(position);
    }
}
