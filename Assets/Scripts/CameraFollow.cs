using UnityEngine;

public class CameraFixedAngle : MonoBehaviour
{
    public Transform target; // Target yang akan diikuti (Player)
    public float smoothSpeed = 0.125f; // Kecepatan pergerakan kamera

    // 'offset' sekarang kita buat 'private', karena akan dihitung otomatis
    private Vector3 offset;

    // Fungsi Start() akan dijalankan sekali saat game dimulai
    void Start()
    {
        // Pastikan target sudah di-set di Inspector
        if (target == null)
        {
            Debug.LogError("Target untuk kamera belum diatur!");
            return;
        }

        // INI BAGIAN PENTING YANG BARU:
        // Menghitung dan menyimpan jarak awal antara kamera dan player.
        // Inilah yang 'mengunci' sudut pandang kamera.
        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Tentukan posisi tujuan kamera dengan menambahkan offset yang sudah 'dikunci'
        Vector3 desiredPosition = target.position + offset;

        // Gerakkan kamera secara mulus ke posisi tujuan
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Baris transform.LookAt(target) yang lama sudah kita HAPUS.
        // Jadi, rotasi kamera tidak akan berubah lagi.
    }
}