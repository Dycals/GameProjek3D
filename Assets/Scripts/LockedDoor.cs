using UnityEngine;

// Pintu juga "menandatangani kontrak" IInteractable
public class LockedDoor : MonoBehaviour, IInteractable
{
    // ID kunci yang dibutuhkan untuk membuka pintu ini
    public string requiredKeyID = "OfficeKey";

    // Fungsi wajib dari interface
    public void Interact(PlayerInteract player)
    {
        // Cek ke 'Key Ring' player apakah dia punya kuncinya
        if (player.HasKey(requiredKeyID))
        {
            // Punya kunci! Buka pintunya.
            Debug.Log("Pintu tidak terkunci!");

            // Cara simpel membuka pintu: non-aktifkan objeknya
            gameObject.SetActive(false);

            // (Alternatif: kamu bisa mainkan animasi, suara, dll. di sini)
        }
        else
        {
            // Tidak punya kunci
            Debug.Log("Pintu terkunci! Butuh " + requiredKeyID);
            // (Di sini kamu bisa mainkan suara "terkunci")
        }
    }
}