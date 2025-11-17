using UnityEngine;

public class LockedDoor : MonoBehaviour, IInteractable
{
    public string requiredKeyID = "OfficeKey";

    public void Interact(PlayerInteract player)
    {
        if (player.HasKey(requiredKeyID))
        {
            Debug.Log("Pintu tidak terkunci!");

            gameObject.SetActive(false);

        }
        else
        {
            Debug.Log("Pintu terkunci! Butuh " + requiredKeyID);
        }
    }
}