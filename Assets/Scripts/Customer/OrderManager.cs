using UnityEngine;

public class OrderManager : MonoBehaviour
{
    [SerializeField] private QueueManager queueManager;
    [SerializeField] private Transform orderPoint;
    [SerializeField] private CoffeeMachine coffeeMachine;
    private Customer currentCustomer;
    private Order currentOrder;

    // Update is called once per frame
    void Update()
    {
        if (currentCustomer != null) return;
        
        Customer customer = queueManager.GetFirstCustomer();

        if (customer != null && customer.CurrentState == Customer.CustomerState.Waiting)
        {
            currentCustomer = customer;
            queueManager.RemoveCustomerFromQueue(customer);
            customer.GoToOrderPoint(orderPoint);
            customer.ReachedOrderPoint += CreateOrder;
        }
    }

    private void CreateOrder(Customer customer)
    {
        currentOrder = new Order(customer);
        Debug.Log(
            "Created order for " + customer.name +
            " Status: " + currentOrder.Status
        );
        
        coffeeMachine.BrewOrder(currentOrder);
    }
}
