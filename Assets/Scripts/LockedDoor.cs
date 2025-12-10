using UnityEngine;

public class LockedDoor : MonoBehaviour, IInteractable
{
    public string requiredKeyID = "OfficeKey";

    public AudioClip unlockSound;


    public void Interact(PlayerInteract player)
    {
        if (player.HasKey(requiredKeyID))
        {
            Debug.Log("Pintu tidak terkunci!");
            if (unlockSound != null)
            {
                AudioSource.PlayClipAtPoint(unlockSound, transform.position);
            }
            gameObject.SetActive(false);

        }
        else
        {
            Debug.Log("Pintu terkunci! Butuh " + requiredKeyID);
        }
    }
}