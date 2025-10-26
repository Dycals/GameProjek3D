using UnityEngine;

// Perhatikan tambahan ", IInteractable" di sini
public class ObjectiveItem : MonoBehaviour, IInteractable
{
    public string objectiveName = "Objektif Baru";

    // Ini adalah fungsi yang WAJIB ada dari interface IInteractable
    // Logika dari 'Collect()' kita pindah ke sini
    public void Interact(PlayerInteract player)
    {
        Debug.Log(objectiveName + " telah dikumpulkan!");

        // Hilangkan objek ini dari scene
        gameObject.SetActive(false);
    }

    // Fungsi OnTriggerEnter dan OnTriggerExit kita HAPUS dari sini.
    // Kenapa? Karena PlayerInteract sekarang yang menangani pendeteksian trigger.
    // Ini membuat skrip ObjectiveItem lebih bersih.
}