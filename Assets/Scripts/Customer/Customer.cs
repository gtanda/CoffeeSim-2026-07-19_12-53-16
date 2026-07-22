using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    public enum CustomerState
    {
        WalkingToQueue,
        Waiting,
        WalkingToOrderPoint,
        AtOrderPoint,
        WaitingForCoffee,
        Leaving
    }

    private CustomerState currentState;

    private NavMeshAgent navMeshAgent;

    public System.Action<Customer> ReachedOrderPoint;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public CustomerState CurrentState => currentState;

    void Update()
    {
        Debug.Log("Customer state: " + CurrentState);
        if (currentState == CustomerState.WalkingToQueue)
        {
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                currentState = CustomerState.Waiting;
            }
        }
        else if (currentState == CustomerState.WalkingToOrderPoint)
        {
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                currentState = CustomerState.AtOrderPoint;
                ReachedOrderPoint?.Invoke(this);
            }
        }
        else if (currentState == CustomerState.Leaving)
        {
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                Destroy(gameObject);
            }
        }
    }

    public void MoveTo(Vector3 destination)
    {
        navMeshAgent.SetDestination(destination);
        currentState = CustomerState.WalkingToQueue;
    }

    public void GoToOrderPoint(Transform orderPoint)
    {
        currentState = CustomerState.WalkingToOrderPoint;
        navMeshAgent.SetDestination(orderPoint.position);
    }

    public void Leave(Vector3 exitPosition)
    {
        currentState = CustomerState.Leaving;
        navMeshAgent.SetDestination(exitPosition);
    }
}