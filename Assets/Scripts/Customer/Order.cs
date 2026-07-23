using UnityEngine;

public class Order : MonoBehaviour
{
    public enum OrderStatus
    {
        Waiting,
        Brewing,
        Ready,
        Completed
    }
    
    public Customer Customer { get; private set; }
    public CoffeeCup AssignedCup { get; private set; }
    public OrderStatus Status { get; private set; }
    public System.Action<Order> OnOrderReady;

    public Order(Customer customer)
    {
        Customer = customer;
    }

    public void ChangeStatus(OrderStatus newStatus)
    {
        Status = newStatus;
        Debug.Log("Order changed to " + Status);
    }

    public void AssignCoffeeCup(CoffeeCup cup)
    {
        AssignedCup = cup;
        Status = OrderStatus.Ready;

        OnOrderReady?.Invoke(this);
    }
}
