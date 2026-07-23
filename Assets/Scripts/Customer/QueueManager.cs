using System;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    [SerializeField] private QueueSlot[] queueSlots;
    private PlayerControls _playerControls;
    
    [SerializeField] private Transform exitPosition;


    private void Awake()
    {
        _playerControls = new PlayerControls();
        _playerControls.Player.RemoveCustomer.performed += ctx => RemoveFirstCustomer();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }


    private void RemoveFirstCustomer()
    {
        foreach (QueueSlot slot in queueSlots)
        {
            if (!slot.IsAvailable())
            {
                RemoveCustomerFromQueue(slot.GetCustomer());
                return;
            }
        }
    }

    public bool TryAssignCustomer(Customer customer)
    {
        foreach (QueueSlot slot in queueSlots)
        {
            if (slot.IsAvailable())
            {
                slot.AssignCustomer(customer);
                customer.MoveTo(slot.transform.position, Customer.CustomerState.WalkingToQueue);
                return true;
            }
        }

        return false;
    }

    public void RemoveCustomerFromQueue(Customer customer)
    {
        if (TryGetCustomerSlot(customer, out QueueSlot slot))
        {
            slot.ClearSlot();
            ShiftQueueForward();
        }
    }

    public Customer GetFirstCustomer()
    {
        Customer customer = queueSlots[0].GetCustomer();
        if (customer != null && customer.CurrentState == Customer.CustomerState.Waiting)
        {
            return customer;
        }

        return null;
    }

    private bool TryGetCustomerSlot(Customer customer, out QueueSlot slot)
    {
        foreach (QueueSlot currentSlot in queueSlots)
        {
            if (currentSlot.GetCustomer() == customer)
            {
                slot = currentSlot;
                return true;
            }
        }

        slot = null;
        return false;
    }

    private void ShiftQueueForward()
    {
        for (int i = 0; i < queueSlots.Length - 1; i++)
        {
            QueueSlot currentSlot = queueSlots[i];
            if (currentSlot.IsAvailable())
            {
                QueueSlot nextSlot = queueSlots[i + 1];
                Customer customer = nextSlot.RemoveCustomer();

                if (customer != null)
                {
                    currentSlot.AssignCustomer(customer);
                    customer.MoveTo(currentSlot.transform.position, Customer.CustomerState.WalkingToQueue);
                }
            }
        }
    }
}