using UnityEngine;

namespace Interaction
{
    public class PlaceableSurface : MonoBehaviour, IInteractable, IPlaceable
    {
        [SerializeField] private Transform placePoint;


        public Transform GetPlacePoint()
        {
            return placePoint;
        }


        public virtual void Interact()
        {
            Debug.Log("Place item here");
        }


        public virtual string GetInteractionText()
        {
            return "Press E to place";
        }
    }
}