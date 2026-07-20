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

    [Header("Cup")] [SerializeField] private GameObject cupPrefab;
    [SerializeField] private Transform cupSpawnPoint;

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


    private IEnumerator BrewCoffee()
    {
        ChangeState(MachineState.Brewing);

        yield return new WaitForSeconds(brewTime);

        SpawnCup();

        ChangeState(MachineState.Idle);
    }


    private void SpawnCup()
    {
        Instantiate(
            cupPrefab,
            cupSpawnPoint.position,
            cupSpawnPoint.rotation
        );
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