using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    public float captureDistance = 1.5f;

    public void InvestigateLocation(Vector3 location)
    {
        currentState = EnemyState.Investigating;
        investigationSpot = location;
        agent.SetDestination(investigationSpot);
        Debug.Log("Kehilangan jejak, menginvestigasi lokasi terakhir!");
    }

    public float guardHearingRange = 10f;

    public NavMeshAgent agent;
    public Transform player;

    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;

    public float viewRadius = 10f;
    [Range(0, 360)]
    public float viewAngle = 90f;
    public LayerMask playerMask;
    public LayerMask obstacleMask;

    public enum EnemyState { Patrolling, Chasing, Investigating }
    public EnemyState currentState;

    private Vector3 investigationSpot;

    private void OnEnable()
    {
        CCTV.OnPlayerSpotted += InvestigateLocation;
        CharacterMove.OnNoiseMade += OnNoiseHeard;
    }

    private void OnDisable()
    {
        CCTV.OnPlayerSpotted -= InvestigateLocation;
        CharacterMove.OnNoiseMade -= OnNoiseHeard;
    }

    public void OnNoiseHeard(Vector3 noiseLocation, float noiseRadius)
    {
        float distanceToNoise = Vector3.Distance(transform.position, noiseLocation);

        if (distanceToNoise <= guardHearingRange)
        {
            if (currentState == EnemyState.Chasing)
            {
                return;
            }

            InvestigateLocation(noiseLocation);
            Debug.Log("Mendengar suara! Menginvestigasi lokasi : " + noiseLocation);
        }
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = EnemyState.Patrolling;
        GoToNextPatrolPoint();
    }

    void Update()
    {
        
        if (!GameManager.IsGameActive)
        {
            if (agent.enabled && agent.isStopped == false)
            {
                agent.isStopped = true;
            }
            return;
        }

        if (agent.enabled && agent.isStopped == true && GameManager.IsGameActive)
        {
            agent.isStopped = false;
        }

        bool canSeePlayer = CheckFieldOfView();

        switch (currentState)
        {
            case EnemyState.Patrolling:
                if (canSeePlayer) { currentState = EnemyState.Chasing; }
                if (!agent.pathPending && agent.remainingDistance < 0.5f) { GoToNextPatrolPoint(); }
                break;

            case EnemyState.Chasing:
                if (!canSeePlayer)
                {
                    InvestigateLocation(player.position);
                }
                else
                {
                    agent.SetDestination(player.position);

                    if (Vector3.Distance(transform.position, player.position) < captureDistance)
                    {
                        GameManager.Instance.EndGame(false);
                        currentState = EnemyState.Investigating;
                        agent.isStopped = true;
                        Debug.Log("Player Tertangkap");
                    }
                }
                break;

            case EnemyState.Investigating:
                if (canSeePlayer) { currentState = EnemyState.Chasing; }
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    currentState = EnemyState.Patrolling;
                    GoToNextPatrolPoint();
                }
                break;
        }
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    bool CheckFieldOfView()
    {
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
        Gizmos.color = Color.yellow;
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