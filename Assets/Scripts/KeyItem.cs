using UnityEngine;

// Kunci juga "menandatangani kontrak" IInteractable
public class KeyItem : MonoBehaviour, IInteractable
{
    // ID unik untuk kunci ini, harus SAMA dengan ID di pintu
    public string keyID = "OfficeKey";

    // Fungsi wajib dari interface
    public void Interact(PlayerInteract player)
    {
        // Saat di-interaksi, tambahkan ID kunci ini ke 'Key Ring' player
        player.AddKey(keyID);

        // Hilangkan kunci dari scene
        gameObject.SetActive(false);
    }
}