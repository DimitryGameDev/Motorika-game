using System.Collections;
using UnityEngine;

public class MedvekulaAI : MonoBehaviour
{
    public enum EnemyState
    {
        Patrolling,
        Attacking,
        UnderGround
    }

    [SerializeField] private float moveDistance;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float pauseDuration;
    [SerializeField] private float visionDistance;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackInterval;

    private Vector3 startPosition;
    private bool movingForward = true;
    private bool isPaused = false;
    private EnemyState currentState = EnemyState.Patrolling;
    private bool isAttacking = false;

    private GameObject player;

    private void Start()
    {
        startPosition = transform.position;
        player = GameObject.Find(playerPrefab.name);
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Patrolling:
                if (!isPaused)
                {
                    Move();
                }
                DetectPlayer();
                break;

            case EnemyState.Attacking:
                // ChasePlayer(); // Implement if needed
                DetectPlayer();
                break;

            case EnemyState.UnderGround:
                // Implement underground behavior if needed
                break;
        }
    }

    private void Move()
    {
        Vector3 moveDirection = movingForward ? Vector3.forward : Vector3.back;
        Vector3 newPosition = transform.position + moveDirection * moveSpeed * Time.deltaTime;

        if (Mathf.Abs(newPosition.z - startPosition.z) >= moveDistance)
        {
            movingForward = !movingForward;
            isPaused = true;
            Invoke(nameof(ResumeMovement), pauseDuration);
        }
        else
        {
            transform.position = newPosition;
        }
    }

    private void ResumeMovement()
    {
        isPaused = false;
    }

    private void DetectPlayer()
    {
        if (player == null)
        {
            return;
        }

        Vector3 direction = movingForward ? Vector3.forward : Vector3.back;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit, visionDistance))
        {
            if (hit.collider.gameObject == player)
            {
                currentState = EnemyState.Attacking;
            }
        }
        else
        {
            currentState = EnemyState.Patrolling;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (movingForward ? Vector3.forward : Vector3.back) * visionDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, visionDistance);
    }
}
