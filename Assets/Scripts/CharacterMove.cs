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

    public float rotationDeadZone = 0.1f;

    private CharacterController controller;
    private Animator animator;
    private float currentSpeed;

    public static event System.Action<Vector3, float> OnNoiseMade;
    public float minRunSpeedForNoise = 7.0f;
    public float noiseRadius = 15f;

    public float CurrentSpeed
    {
        get { return currentSpeed; }
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        
        if (gameObject.tag != "Player")
        {
            Debug.LogWarning("Objek Player tidak memiliki tag 'Player'. Deteksi Kebisingan mungkin tidak berfungsi");
        }
    }

    void Update()
    {
        if (!GameManager.IsGameActive)
        {
            if (animator != null)
            {
                animator.SetFloat("Speed", 0f);
            }
            return;
        }

        float horizontal = Input.GetAxis("Horizontal"); // A/D → belok
        float vertical = Input.GetAxis("Vertical");     // W/S → maju/mundur

        // === LOGIKA UNTUK LARI ===
        // Cek apakah tombol LeftShift ditekan DAN karakter sedang bergerak maju.
        // Biasanya, karakter tidak bisa lari saat mundur.
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && vertical > 0;

        // Tentukan kecepatan yang digunakan: lari atau jalan.
        currentSpeed = isRunning ? runSpeed : walkSpeed;

        if (Mathf.Abs(horizontal) > rotationDeadZone)
        {
            transform.Rotate(Vector3.up * horizontal * rotationSpeed * Time.deltaTime);
        }

        Vector3 moveDirection = Vector3.zero;

        if (vertical != 0)
        {
            moveDirection = transform.forward * vertical;
            controller.Move(moveDirection * currentSpeed * Time.deltaTime);
        }

        if (vertical == 0)
        {
            currentSpeed = 0f;
        }

        if (currentSpeed >= minRunSpeedForNoise)
        {
            {
                OnNoiseMade?.Invoke(transform.position, noiseRadius);
            }
        }

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