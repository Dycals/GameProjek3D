using System; // Diperlukan untuk Action
using System.Collections;
using UnityEngine;

public class CCTV : MonoBehaviour
{
    // Event 'Broadcast' yang akan didengarkan oleh para bodyguard
    public static event Action<Vector3> OnPlayerSpotted;

    // Pengaturan Rotasi
    public Vector3[] rotationAngles; // Array sudut rotasi yang diinginkan (Euler Angles)
    public float rotationSpeed = 20f;
    public float pauseDuration = 2f; // Jeda di setiap titik rotasi

    // Pengaturan Field of View (FOV)
    public Transform player;
    public float viewRadius = 15f;
    [Range(0, 360)]
    public float viewAngle = 120f;
    public LayerMask playerMask;
    public LayerMask obstacleMask;

    private Quaternion[] targetRotations;
    private int currentRotationIndex = 0;
    private float alertCooldown = 2f; // Cooldown agar tidak spam alert
    private float lastAlertTime;

    void Start()
    {
        // Konversi Euler Angles dari Inspector menjadi Quaternion untuk rotasi
        targetRotations = new Quaternion[rotationAngles.Length];
        for (int i = 0; i < rotationAngles.Length; i++)
        {
            targetRotations[i] = Quaternion.Euler(rotationAngles[i]);
        }

        StartCoroutine(RotateCCTV());
    }

    void Update()
    {
        if (CheckFieldOfView())
        {
            // Jika player terlihat dan cooldown sudah selesai
            if (Time.time > lastAlertTime + alertCooldown)
            {
                lastAlertTime = Time.time;
                Debug.Log("CCTV: Player Spotted!");

                // Broadcast event ke semua bodyguard yang mendengarkan
                OnPlayerSpotted?.Invoke(player.position);
            }
        }
    }

    IEnumerator RotateCCTV()
    {
        while (true) // Loop selamanya
        {
            Quaternion target = targetRotations[currentRotationIndex];

            // Berputar menuju target rotasi
            while (Quaternion.Angle(transform.rotation, target) > 0.1f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, target, rotationSpeed * Time.deltaTime);
                yield return null; // Tunggu frame berikutnya
            }

            transform.rotation = target; // Pastikan rotasi pas

            // Jeda sejenak
            yield return new WaitForSeconds(pauseDuration);

            // Pindah ke target rotasi berikutnya
            currentRotationIndex = (currentRotationIndex + 1) % targetRotations.Length;
        }
    }

    bool CheckFieldOfView()
    {
        if (player == null) return false;

        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);
        if (playerInRange.Length > 0)
        {
            Transform target = playerInRange[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
                {
                    return true; // Player terlihat!
                }
            }
        }
        return false; // Player tidak terlihat
    }

    // (Opsional) Visualisasi FOV di Scene Editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector3 viewAngleA = DirFromAngle(-viewAngle / 2, false);
        Vector3 viewAngleB = DirFromAngle(viewAngle / 2, false);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleA * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB * viewRadius);
    }
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal) { angleInDegrees += transform.eulerAngles.y; }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}