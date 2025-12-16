using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CharacterMove : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float turnSmoothTime = 0.05f;
    float turnSmoothVelocity;

    public float gravity = -9.81f;
    private Vector3 velocityY;

    [Header("Input Settings")]
    public VirtualJoystick joystick;
    public float minInputForMove = 0.1f;

    [Header("Audio Settings")]
    public AudioSource footstepSource;
    public AudioClip walkClip;
    public AudioClip runClip;

    [Range(0.1f, 1f)]
    public float walkStepInterval = 0.5f;
    [Range(0.1f, 1f)]
    public float runStepInterval = 0.3f;

    private float nextStepTime = 0f;

    private CharacterController controller;
    private Animator animator;
    private float currentSpeed;
    private Transform cameraTransform;

    public static event System.Action<Vector3, float> OnNoiseMade;
    public float minRunSpeedForNoise = 7.0f;
    public float noiseRadius = 15f;

    public float CurrentSpeed { get { return currentSpeed; } }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (footstepSource == null) footstepSource = GetComponent<AudioSource>();
        if (joystick == null) joystick = FindObjectOfType<VirtualJoystick>();
        if (Camera.main != null) cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        if (!GameManager.IsGameActive)
        {
            if (animator != null) animator.SetFloat("Speed", 0f);
            return;
        }

        bool isGrounded = controller.isGrounded;

        if (isGrounded && velocityY.y < 0)
        {
            velocityY.y = -2f;
        }

        velocityY.y += gravity * Time.deltaTime;

        float horizontal = 0f;
        float vertical = 0f;
        bool isInputFromJoystick = false;

        if (joystick != null && (Mathf.Abs(joystick.Horizontal()) > 0 || Mathf.Abs(joystick.Vertical()) > 0))
        {
            horizontal = joystick.Horizontal();
            vertical = joystick.Vertical();
            isInputFromJoystick = true;
        }
        else
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
            isInputFromJoystick = false;
        }

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // 3. MOVEMENT
        if (direction.magnitude >= minInputForMove)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            if (cameraTransform != null) targetAngle += cameraTransform.eulerAngles.y;

            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            bool isRunning = false;
            if (!isInputFromJoystick)
            {
                if (Input.GetKey(KeyCode.LeftShift)) isRunning = true;
            }
            else
            {
                if (direction.magnitude > 0.8f) isRunning = true;
            }

            currentSpeed = isRunning ? runSpeed : walkSpeed;

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // Gerakkan karakter (Horizontal)
            controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);

            HandleFootsteps(isRunning);
        }
        else
        {
            currentSpeed = 0f;
        }

        controller.Move(velocityY * Time.deltaTime);

        if (currentSpeed >= minRunSpeedForNoise)
        {
            OnNoiseMade?.Invoke(transform.position, noiseRadius);
        }

        UpdateAnimation(direction.magnitude);
    }

    void HandleFootsteps(bool isRunning)
    {
        // Sekarang cek isGrounded jauh lebih stabil karena ada gaya tekan ke bawah
        if (!controller.isGrounded) return;

        if (Time.time >= nextStepTime)
        {
            AudioClip clipToPlay = isRunning ? runClip : walkClip;
            float interval = isRunning ? runStepInterval : walkStepInterval;

            if (clipToPlay != null && footstepSource != null)
            {
                footstepSource.pitch = Random.Range(0.9f, 1.1f);
                footstepSource.PlayOneShot(clipToPlay);
            }

            nextStepTime = Time.time + interval;
        }
    }

    void UpdateAnimation(float inputMagnitude)
    {
        if (animator == null) return;
        float animSpeed = 0f;

        if (inputMagnitude >= minInputForMove)
        {
            animSpeed = (currentSpeed > walkSpeed + 0.5f) ? 2f : 1f;
        }
        animator.SetFloat("Speed", animSpeed, 0.1f, Time.deltaTime);
    }
}