using System; // Diperlukan untuk Action
using System.Collections;
using UnityEngine;

public class CCTV : MonoBehaviour
{
    public static event Action<Vector3> OnPlayerSpotted;

    public Vector3[] rotationAngles;
    public float rotationSpeed = 20f;
    public float pauseDuration = 2f;

    public Transform player;
    public float viewRadius = 15f;
    [Range(0, 360)]
    public float viewAngle = 120f;
    public LayerMask playerMask;
    public LayerMask obstacleMask;

    private Quaternion[] targetRotations;
    private int currentRotationIndex = 0;
    private float alertCooldown = 2f;
    private float lastAlertTime;

    void Start()
    {
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
            if (Time.time > lastAlertTime + alertCooldown)
            {
                lastAlertTime = Time.time;
                Debug.Log("CCTV: Player Spotted!");

                OnPlayerSpotted?.Invoke(player.position);
            }
        }
    }

    IEnumerator RotateCCTV()
    {
        while (true)
        {
            Quaternion target = targetRotations[currentRotationIndex];

            while (Quaternion.Angle(transform.rotation, target) > 0.1f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, target, rotationSpeed * Time.deltaTime);
                yield return null;
            }

            transform.rotation = target;

            yield return new WaitForSeconds(pauseDuration);

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
                    return true;
                }
            }
        }
        return false;
    }

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