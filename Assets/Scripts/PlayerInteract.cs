using System.Collections.Generic; // Dibutuhkan untuk "Key Ring"
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    // Mengganti 'ObjectiveItem' dengan 'IInteractable'
    private IInteractable currentInteractable;

    // "Key Ring" virtual milik player untuk menyimpan ID kunci
    private HashSet<string> collectedKeys = new HashSet<string>();

    void Update()
    {
        // Logika tidak berubah, tapi sekarang lebih fleksibel
        if (Input.GetKeyDown(KeyCode.F) && currentInteractable != null)
        {
            // Memanggil fungsi 'Interact' dari objek apapun yang ada di dekatnya
            currentInteractable.Interact(this);
            currentInteractable = null; // Hapus referensi setelah berinteraksi (jika perlu)
        }
    }

    // --- FUNGSI UNTUK KEY RING ---

    // Fungsi untuk menambah kunci ke 'Key Ring'
    public void AddKey(string keyID)
    {
        collectedKeys.Add(keyID);
        Debug.Log("Kunci didapat: " + keyID);
    }

    // Fungsi untuk mengecek apakah player punya kunci tertentu
    public bool HasKey(string keyID)
    {
        return collectedKeys.Contains(keyID);
    }

    // --- LOGIKA TRIGGER ---

    // Fungsi ini dipanggil saat masuk trigger
    private void OnTriggerEnter(Collider other)
    {
        // Mencari komponen APAPUN yang memakai interface IInteractable
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            currentInteractable = interactable;
            Debug.Log("Dalam jangkauan: " + other.name);
            // Di sini kamu bisa tampilkan UI "Tekan F"
        }
    }

    // Fungsi ini dipanggil saat keluar trigger
    private void OnTriggerExit(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null && interactable == currentInteractable)
        {
            currentInteractable = null;
            Debug.Log("Keluar jangkauan: " + other.name);
            // Di sini kamu bisa sembunyikan UI "Tekan F"
        }
    }
}