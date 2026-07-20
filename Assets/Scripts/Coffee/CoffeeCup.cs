using Interaction;
using UnityEngine;

public class CoffeeCup : HoldableObject, IInteractable
{
    private PlaceableSlot currentSlot;

    public void Interact()
    {
    }

    public void SetSlot(PlaceableSlot slot)
    {
        currentSlot = slot;
    }

    public override void PickUp(Transform holdPoint)
    {
        if (currentSlot != null)
        {
            currentSlot.Clear();
            currentSlot = null;
        }
        base.PickUp(holdPoint);
    }
    
    public string GetInteractionText()
    {
        return "Press E to pick up";
    }
}