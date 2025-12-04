using UnityEngine;

public class KeyItem : MonoBehaviour, IInteractable
{
    public string keyID = "OfficeKey";

    public void Interact(PlayerInteract player)
    {
        player.AddKey(keyID);

        gameObject.SetActive(false);
    }
}