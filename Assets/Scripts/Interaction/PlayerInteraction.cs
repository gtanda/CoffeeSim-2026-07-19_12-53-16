using System;
using TMPro;
using UnityEngine;

namespace Interaction
{
    public class PlayerInteraction : MonoBehaviour
    {
        public float interactDistance = 3f;
        private PlayerControls _playerControls;
        [SerializeField] private Camera playerCamera;

        [SerializeField] private GameObject interactionPrompt;
        [SerializeField] private TextMeshProUGUI promptText;

        private void Awake()
        {
            _playerControls = new PlayerControls();

            _playerControls.Player.Interact.performed += ctx => TryInteract();
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
                if (interactable != null)
                {
                    interactionPrompt.SetActive(true);
                    promptText.text = "Press E to interact";
                }
                else
                {
                    interactionPrompt.SetActive(false);
                }
            }
        }

        private void TryInteract()
        {
            if (TryGetInteractable(out IInteractable interactable))
            {
                interactable.Interact();
            }
        }
    }
}