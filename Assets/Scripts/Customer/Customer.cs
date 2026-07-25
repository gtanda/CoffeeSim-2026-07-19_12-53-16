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
    private Transform coffeePickupPoint;

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
                ChangeState(CustomerState.Waiting);
            }
        }
        else if (currentState == CustomerState.WalkingToOrderPoint)
        {
            if (HasReachedDestination())
            {
                ChangeState(CustomerState.AtOrderPoint);
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
                ChangeState(CustomerState.Leaving);
            }
        }
    }

    public void SetCoffeePickupPoint(Transform pickupPoint)
    {
        coffeePickupPoint = pickupPoint;
    }

    private void ChangeState(CustomerState newState)
    {
        Debug.Log($"{name}: {currentState} -> {newState}");
        currentState = newState;
    }

    public bool HasReachedDestination()
    {
        return !navMeshAgent.pathPending &&
               navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance;
    }

    public void StartWaitingForCoffee()
    {
        ChangeState(CustomerState.WaitingForCoffee);
    }

    public void AssignOrder(Order order)
    {
        currentOrder = order;
        currentOrder.OnOrderReady += HandleOrderReady;
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

    private void HandleOrderReady(Order order)
    {
        GoToCoffeePickup(coffeePickupPoint);
    }

    public void Leave(Vector3 exitPosition)
    {
        MoveTo(exitPosition, CustomerState.Leaving);
    }
    
    private void OnDestroy()
    {
        if (currentOrder != null)
        {
            currentOrder.OnOrderReady -= HandleOrderReady;
        }
    }
}