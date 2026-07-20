using System;
using TMPro;
using UnityEngine;

namespace Interaction
{
    public class PlayerInteraction : MonoBehaviour
    {
        public float interactDistance = 3f;
        private PlayerControls _playerControls;
        private IHoldable _heldObject;
        [SerializeField] private Camera playerCamera;

        [SerializeField] private GameObject interactionPrompt;
        [SerializeField] private TextMeshProUGUI promptText;

        [SerializeField] private Transform holdPoint;

        private void Awake()
        {
            _playerControls = new PlayerControls();

            _playerControls.Player.Interact.performed += ctx => TryInteract();
            _playerControls.Player.Drop.performed += ctx => DropObject();
        }

        private void Update()
        {
            CheckForInteractable();
        }

        private void OnEnable()
        {
            _playerControls.Enable();
        }

        private void OnDisable()
        {
            _playerControls.Disable();
        }

        private void OnDrawGizmos()
        {
            if (playerCamera == null)
                return;

            Gizmos.color = Color.yellow;

            Vector3 start = playerCamera.transform.position;
            Vector3 end = start + playerCamera.transform.forward * interactDistance;

            Gizmos.DrawLine(start, end);

            Gizmos.DrawWireSphere(end, 0.3f);
        }

        private bool TryGetInteractable(out IInteractable interactable)
        {
            interactable = null;

            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

            if (Physics.SphereCast(ray, 0.3f, out RaycastHit hit, interactDistance))
            {
                interactable = hit.collider.GetComponent<IInteractable>();
                return interactable != null;
            }

            return false;
        }

        private void CheckForInteractable()
        {
            if (TryGetInteractable(out IInteractable interactable))
            {
                interactionPrompt.SetActive(true);
                promptText.text = interactable.GetInteractionText();
            }
            else
            {
                interactionPrompt.SetActive(false);
            }
        }

        private void TryInteract()
        {
            if (!TryGetInteractable(out IInteractable interactable))
                return;


            // If we are holding something, try to place it
            if (_heldObject != null)
            {
                if (interactable is IPlaceable placeable)
                {
                    _heldObject.Place(placeable.GetPlacePoint());
                    _heldObject = null;
                }

                return;
            }


            // If our hands are empty, try picking something up
            if (interactable is IHoldable holdable)
            {
                _heldObject = holdable;
                holdable.PickUp(holdPoint);

                return;
            }


            // Otherwise, use the object's normal interaction
            interactable.Interact();
        }


        private void DropObject()
        {
            if (_heldObject != null)
            {
                _heldObject.Drop();
                _heldObject = null;
            }
        }
    }
}