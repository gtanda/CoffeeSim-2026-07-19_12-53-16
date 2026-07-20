using UnityEngine;

namespace Interaction
{
    public class HoldableObject : MonoBehaviour, IHoldable
    {
        private Rigidbody rb;

        private Vector3 originalScale;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            originalScale = transform.localScale;
        }


        public virtual void PickUp(Transform holdPoint)
        {
            rb.isKinematic = true;

            transform.SetParent(holdPoint);

            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = originalScale;
        }


        public virtual void Drop()
        {
            transform.SetParent(null);

            rb.isKinematic = false;
        }


        public virtual void Place(Transform placePoint)
        {
            rb.isKinematic = true;

            transform.SetParent(null);

            transform.position = placePoint.position;
            transform.rotation = placePoint.rotation;
            transform.localScale = originalScale;
        }
    }
}