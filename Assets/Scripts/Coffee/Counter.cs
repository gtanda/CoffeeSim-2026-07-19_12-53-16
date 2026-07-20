using Interaction;
using UnityEngine;

public class Counter : PlaceableSurface
{
    [SerializeField] private PlaceableSlot[] slots;


    public bool TryGetAvailableSlot(out PlaceableSlot slot)
    {
        foreach (PlaceableSlot currentSlot in slots)
        {
            if (currentSlot.IsAvailable())
            {
                slot = currentSlot;
                return true;
            }
        }

        slot = null;
        return false;
    }
}