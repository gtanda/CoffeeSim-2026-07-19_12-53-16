using Interaction;
using UnityEngine;

public class CoffeeCup : HoldableObject, IInteractable
{

    public void Interact()
    {
    }
    
    public string GetInteractionText()
    {
        return "Press E to pick up";
    }
}