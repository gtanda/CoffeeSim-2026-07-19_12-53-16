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
        WalkingToCoffeePickup,
        Leaving
    }

    private CustomerState currentState;
    private Order currentOrder;

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
            if (HasReachedDestination())
            {
                currentState = CustomerState.Waiting;
            }
        }
        else if (currentState == CustomerState.WalkingToOrderPoint)
        {
            if (HasReachedDestination())
            {
                currentState = CustomerState.AtOrderPoint;
                ReachedOrderPoint?.Invoke(this);
            }
        }
        else if (currentState == CustomerState.Leaving)
        {
            if (HasReachedDestination())
            {
                Destroy(gameObject);
            }
        }
        else if (currentState == CustomerState.WalkingToCoffeePickup)
        {
            if (HasReachedDestination())
            {
                Debug.Log("Reached pickup!");

                Debug.Log(currentOrder.AssignedCup);

                currentState = CustomerState.WaitingForCoffee;
            }
        }
    }

    public bool HasReachedDestination()
    {
        return !navMeshAgent.pathPending &&
               navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance;
    }


    public void AssignOrder(Order order)
    {
        currentOrder = order;
    }

    public void MoveTo(Vector3 destination, CustomerState newState)
    {
        currentState = newState;
        navMeshAgent.SetDestination(destination);
    }

    public void GoToOrderPoint(Transform orderPoint)
    {
        MoveTo(orderPoint.position, CustomerState.WalkingToOrderPoint);
    }

    public void GoToCoffeePickup(Transform pickupPoint)
    {
        MoveTo(pickupPoint.position, CustomerState.WalkingToCoffeePickup);
    }

    public void Leave(Vector3 exitPosition)
    {
        MoveTo(exitPosition, CustomerState.Leaving);
    }
}