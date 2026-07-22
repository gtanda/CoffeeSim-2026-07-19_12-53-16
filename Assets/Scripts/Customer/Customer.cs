using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    [SerializeField] private Transform target;

    private NavMeshAgent navMeshAgent;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void MoveTo(Vector3 destination)
    {
        navMeshAgent.SetDestination(destination);
    }

    
}
