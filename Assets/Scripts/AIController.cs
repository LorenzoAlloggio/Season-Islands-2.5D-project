using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI; // Added for accessing UI Text component
using TMPro;

public class AIController : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public float startWaitTime = 4f;
    public float timeToRotate = 2f;
    public float speedWalk = 1f;
    public float speedRun = 5f;

    public float viewRadius = 15f;
    public float viewAngle = 90f;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public Transform[] waypoints;

    private Vector3 playerLastPosition;
    private bool isChasingPlayer;
    private float waitTime;
    private int currentWaypointIndex;

    public TMP_Text debugLogText; // Reference to the Text component for debug logs

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        waitTime = startWaitTime;
        currentWaypointIndex = 0;
        SetNextWaypointDestination();
    }

    private void Update()
    {
        //CheckEnvironment(); // Turned off due to a stupid raycast error

        if (isChasingPlayer)
            ChasePlayer();
        else
            Patrol();
    }

    private void ChasePlayer()
    {
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if (waitTime <= 0 && !IsPlayerInRange() && Vector3.Distance(transform.position, playerLastPosition) >= 6f)
                ResumePatrol();
            else if (waitTime > 0)
                waitTime -= Time.deltaTime;
            else
                navMeshAgent.SetDestination(playerLastPosition);
        }
    }

    private void Patrol()
    {
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if (waitTime <= 0)
                SetNextWaypointDestination();
            else
                waitTime -= Time.deltaTime;
        }
    }

    private void Move(float speed)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }

    private void StopMoving()
    {
        navMeshAgent.isStopped = true;
    }

    private void SetNextWaypointDestination()
    {
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
        waitTime = startWaitTime;
    }

    private void ResumePatrol()
    {
        isChasingPlayer = false;
        Move(speedWalk);
        SetNextWaypointDestination();
    }

    private void CheckEnvironment()
    {
        Collider[] obstaclesInRange = Physics.OverlapSphere(transform.position, viewRadius, obstacleMask);
        foreach (Collider obstacle in obstaclesInRange)
        {
            Vector3 directionToObstacle = (obstacle.transform.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, directionToObstacle) < viewAngle / 2)
            {
                float distanceToObstacle = Vector3.Distance(transform.position, obstacle.transform.position);
                if (!Physics.Raycast(transform.position, directionToObstacle, distanceToObstacle, obstacleMask))
                {
                    return; // There is an obstacle in the view angle, so we cannot see the player
                }
            }
        }

        Collider[] playersInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);
        foreach (Collider playerCollider in playersInRange)
        {
            Transform player = playerCollider.transform;
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, directionToPlayer) < viewAngle / 2)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);
                if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleMask))
                {
                    // No obstacles blocking the view to the player, chase the player
                    isChasingPlayer = true;
                    playerLastPosition = player.position;
                    navMeshAgent.SetDestination(playerLastPosition);
                    return;
                }
            }
        }

        // If no players found within the view angle and no obstacles blocking the view, stop chasing
        isChasingPlayer = false;
    }

    private bool IsPlayerInRange()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);
        return playerInRange.Length > 0;
    }

    private void LogToDebug(string message)
    {
        if (debugLogText != null)
        {
            debugLogText.text = message;
        }
    }
}
