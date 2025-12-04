using System.Collections.Generic; // Dibutuhkan untuk "Key Ring"
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private IInteractable currentInteractable;

    private HashSet<string> collectedKeys = new HashSet<string>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && currentInteractable != null)
        {
            currentInteractable.Interact(this);
            currentInteractable = null;
        }
    }

    public void AddKey(string keyID)
    {
        collectedKeys.Add(keyID);
        Debug.Log("Kunci didapat: " + keyID);
    }

    public bool HasKey(string keyID)
    {
        return collectedKeys.Contains(keyID);
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            currentInteractable = interactable;
            Debug.Log("Dalam jangkauan: " + other.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null && interactable == currentInteractable)
        {
            currentInteractable = null;
            Debug.Log("Keluar jangkauan: " + other.name);
        }
    }
}