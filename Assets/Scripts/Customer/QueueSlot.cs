using UnityEngine;

public class QueueSlot : MonoBehaviour
{
    private Customer currentCustomer;
    public bool IsAvailable() => currentCustomer == null;
    public Customer GetCustomer() => currentCustomer;
    public Transform GetPosition() => transform;

    public void AssignCustomer(Customer customer)
    {
        currentCustomer = customer;
    }
    

    public void ClearSlot()
    {
        currentCustomer = null;
    }

    public Customer RemoveCustomer()
    {
        Customer customer = currentCustomer;
        currentCustomer = null; 
        return customer;
    }
}
