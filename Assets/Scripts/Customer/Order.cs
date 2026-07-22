using UnityEditor.Rendering;
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
    public CoffeeCup Coffee { get; private set; }
    public OrderStatus Status { get; private set; }

    public Order(Customer customer)
    {
        Customer = customer;
    }

    public void ChangeStatus(OrderStatus newStatus)
    {
        Status = newStatus;
        Debug.Log("Order changed to " + Status);
    }

    public void AssignCoffeeCup(CoffeeCup coffee)
    {
        Coffee = coffee;
        Status = OrderStatus.Ready;
    }
}
