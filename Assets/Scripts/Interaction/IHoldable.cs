using UnityEngine;

namespace Interaction
{
    public interface IHoldable
    {
        void PickUp(Transform holdPoint);
        void Drop();
        
        void Place(Transform placePoint);
    }
}