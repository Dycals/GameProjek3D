using UnityEngine;
using System;

// Perhatikan tambahan ", IInteractable" di sini
public class ObjectiveItem : MonoBehaviour, IInteractable
{
    public string objectiveName = "Objektif Baru";

    public static event Action<string> OnObjectiveCollected;

    public void Interact(PlayerInteract player)
    {
        OnObjectiveCollected?.Invoke(objectiveName);
        Debug.Log(objectiveName + " telah dikumpulkan!");

        gameObject.SetActive(false);
    }
}