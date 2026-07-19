using System;
using UnityEngine;

namespace Interaction
{
    public class PlayerInteraction : MonoBehaviour
    {
        public float interactDistance = 3f;
        private PlayerControls _playerControls;
        [SerializeField] private Camera playerCamera;

        private void Awake()
        {
            _playerControls = new PlayerControls();
            
            _playerControls.Player.Interact.performed += ctx => TryInteract();
        }

        private void OnEnable()
        {
            _playerControls.Enable();
        }

        private void OnDisable()
        {
            _playerControls.Disable();
        }

        private void TryInteract()
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            
            if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }
    }
}