using System.Collections;
using Interaction;
using UnityEngine;

public class CoffeeMachine : MonoBehaviour, IInteractable
{
    private enum MachineState
    {
        Idle,
        Brewing
    }

    [SerializeField] private float brewTime = 5f;

    private Order currentOrder;

    [Header("Cup")] [SerializeField] private GameObject cupPrefab;
    [SerializeField] private Counter counter;

    [Header("Visuals")] [SerializeField] private Renderer statusLight;

    private MachineState currentState;


    private void Start()
    {
        ChangeState(MachineState.Idle);
    }


    public void Interact()
    {
        switch (currentState)
        {
            case MachineState.Idle:
                StartCoroutine(BrewCoffee());
                break;

            case MachineState.Brewing:
                Debug.Log("Coffee is still brewing...");
                break;
        }
    }

    public void BrewOrder(Order order)
    {
        if (currentState == MachineState.Brewing)
        {
            Debug.Log("machine is already brewing");
            return;
        }
        currentOrder = order; 
        StartCoroutine(BrewCoffee());
    }


    private IEnumerator BrewCoffee()
    {
        ChangeState(MachineState.Brewing);

        yield return new WaitForSeconds(brewTime);

        CoffeeCup cup = SpawnCup();
        if (cup != null && currentOrder != null)
        {
            currentOrder.AssignCoffeeCup(cup);
        }
        currentOrder = null;
        ChangeState(MachineState.Idle);
    }


    private CoffeeCup SpawnCup()
    {
        if (counter.TryGetAvailableSlot(out PlaceableSlot slot))
        {
            CoffeeCup cup = Instantiate(cupPrefab, slot.transform.position, slot.transform.rotation)
                .GetComponent<CoffeeCup>();
            slot.Occupy(cup);
            return cup;
        }

        Debug.Log("No available counter space!");
        return null;
    }


    private void ChangeState(MachineState newState)
    {
        currentState = newState;
        
        switch (currentState)
        {
            case MachineState.Idle:
                SetLightColor(Color.green);
                break;

            case MachineState.Brewing:
                SetLightColor(Color.yellow);
                break;
        }
    }


    private void SetLightColor(Color color)
    {
        if (statusLight != null)
        {
            statusLight.material.color = color;
        }
    }


    public string GetInteractionText()
    {
        switch (currentState)
        {
            case MachineState.Idle:
                return "Press E to brew coffee";

            case MachineState.Brewing:
                return "Brewing coffee...";

            default:
                return "";
        }
    }
}