using UnityEngine;

namespace Interaction
{
    public class PlaceableSlot : MonoBehaviour
    {
        private IHoldable currentObject;


        public bool IsAvailable()
        {
            return currentObject == null;
        }


        public void Occupy(IHoldable obj)
        {
            currentObject = obj;

            if (obj is CoffeeCup cup)
            {
                cup.SetSlot(this);
            }
        }


        public void Clear()
        {
            currentObject = null;
        }
    }
}