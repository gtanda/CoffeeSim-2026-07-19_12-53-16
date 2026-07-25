using UnityEngine;

public class OrderManager : MonoBehaviour
{
    [SerializeField] private QueueManager queueManager;
    [SerializeField] private Transform orderPoint;
    [SerializeField] private CoffeeMachine coffeeMachine;
    [SerializeField] private Transform coffeePickupPoint;
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
            customer.ReachedOrderPoint += CreateOrder;
            customer.GoToOrderPoint(orderPoint);
        }
    }

    private void CreateOrder(Customer customer)
    {
        currentOrder = new Order(customer);
        customer.AssignOrder(currentOrder);
        customer.StartWaitingForCoffee();

        coffeeMachine.BrewOrder(currentOrder);
    }
}