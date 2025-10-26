// Ini adalah file IInteractable.cs
// Perhatikan: ini BUKAN MonoBehaviour. Ini adalah interface.
public interface IInteractable
{
    // Setiap skrip yang memakai interface ini WAJIB memiliki fungsi Interact.
    // Kita juga mengirim 'player' agar objek tahu siapa yang berinteraksi dengannya.
    void Interact(PlayerInteract player);
}