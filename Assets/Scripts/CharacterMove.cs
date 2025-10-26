using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    // Ganti nama 'moveSpeed' jadi 'walkSpeed' agar lebih jelas
    public float walkSpeed = 5f;
    // Tambahkan variabel baru untuk kecepatan lari
    public float runSpeed = 10f;
    public float rotationSpeed = 180f; // kecepatan putar (derajat per detik)

    private CharacterController controller;
    private Animator animator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A/D → belok
        float vertical = Input.GetAxis("Vertical");     // W/S → maju/mundur

        // === LOGIKA UNTUK LARI ===
        // Cek apakah tombol LeftShift ditekan DAN karakter sedang bergerak maju.
        // Biasanya, karakter tidak bisa lari saat mundur.
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && vertical > 0;

        // Tentukan kecepatan yang digunakan: lari atau jalan.
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        // === GERAKAN & ROTASI ===
        // Rotasi karakter
        transform.Rotate(Vector3.up * horizontal * rotationSpeed * Time.deltaTime);

        // Gerak maju/mundur dengan kecepatan yang sudah ditentukan (currentSpeed)
        Vector3 moveDirection = transform.forward * vertical;
        controller.Move(moveDirection * currentSpeed * Time.deltaTime);

        // === UPDATE ANIMATOR ===
        // Atur nilai parameter "Speed" di Animator.
        // Nilai ini akan kita gunakan untuk transisi animasi Idle-Walk-Run.
        float animationSpeedValue = 0f;

        if (vertical > 0) // Bergerak maju
        {
            // Jika isRunning true, nilainya 2. Jika false, nilainya 1.
            animationSpeedValue = isRunning ? 2f : 1f;
        }
        else if (vertical < 0) // Bergerak mundur
        {
            // Saat mundur, selalu pakai animasi jalan (nilai 1).
            animationSpeedValue = 1f;
        }
        // Jika vertical == 0, nilainya tetap 0 (Idle).

        animator.SetFloat("Speed", animationSpeedValue);
    }
}