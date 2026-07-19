using UnityEngine;

namespace Interaction
{
    public class TestInteractable : MonoBehaviour, IInteractable
    {
        public void Interact()
        {
            Debug.Log("I interacted with: " + gameObject.name);
        }
    }
}