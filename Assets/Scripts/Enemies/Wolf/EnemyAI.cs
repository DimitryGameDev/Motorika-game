using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState
    {
        Patrolling,
        Attacking
    }

    public float moveDistance;
    public float moveSpeed;


    //
    public float pauseDuration;
    public float visionDistance;
    public float stopDistance;
    public GameObject playerPrefab;
    public float attackDamage;
    public float attackInterval;
    public float verticalTolerance;

    private Vector3 startPosition;
    private bool movingRight = true;
    private bool isPaused = false;
    private EnemyState currentState = EnemyState.Patrolling;
    private bool isAttacking = false;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
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
                ChasePlayer();
                DetectPlayer();
                break;
        }
    }

    void Move()
    {
        if (movingRight)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;

            if (transform.position.x >= startPosition.x + moveDistance)
            {
                StartCoroutine(PauseBeforeChangeDirection());
            }
        }
        else
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;

            if (transform.position.x <= startPosition.x - moveDistance)
            {
                StartCoroutine(PauseBeforeChangeDirection());
            }
        }
    }

    IEnumerator PauseBeforeChangeDirection()
    {
        isPaused = true;
        yield return new WaitForSeconds(pauseDuration);
        movingRight = !movingRight;
        isPaused = false;
    }

    void DetectPlayer()
    {
        Vector2 direction = movingRight ? Vector2.right : Vector2.left;

        GameObject player = GameObject.Find(playerPrefab.name);

        if (player != null)
        {
            Vector2 playerPosition = new Vector2(player.transform.position.x, transform.position.y);
            Vector2 enemyPosition = transform.position;
            Vector2 toPlayer = playerPosition - enemyPosition;

            float verticalDistance = Mathf.Abs(player.transform.position.y - transform.position.y);
            if (verticalDistance <= verticalTolerance && Vector2.Dot(toPlayer.normalized, direction) > 0 && toPlayer.magnitude <= visionDistance)
            {
                currentState = EnemyState.Attacking;
            }
            else
            {
                currentState = EnemyState.Patrolling;
                StopCoroutine(AttackPlayer());
                isAttacking = false;
            }
        }
    }

    void ChasePlayer()
    {
        GameObject player = GameObject.Find(playerPrefab.name);

        if (player != null)
        {
            Vector2 playerPosition = new Vector2(player.transform.position.x, transform.position.y);
            Vector2 enemyPosition = transform.position;
            float distanceToPlayer = Vector2.Distance(playerPosition, enemyPosition);

            float verticalDistance = Mathf.Abs(player.transform.position.y - transform.position.y);
            if (verticalDistance <= verticalTolerance && distanceToPlayer > stopDistance)
            {
                Vector2 direction = (playerPosition - enemyPosition).normalized;
                transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
            }
            else if (distanceToPlayer <= stopDistance && verticalDistance <= verticalTolerance)
            {
                if (!isAttacking)
                {
                    StartCoroutine(AttackPlayer());
                }
            }
            else
            {
                StopCoroutine(AttackPlayer());
                isAttacking = false;
                currentState = EnemyState.Patrolling;
            }
        }
    }

    IEnumerator AttackPlayer()
    {
        isAttacking = true;
        while (isAttacking)
        {
            GameObject player = GameObject.Find(playerPrefab.name);

            if (player != null)
            {
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(attackDamage);
                }
            }

            yield return new WaitForSeconds(attackInterval);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 direction = movingRight ? Vector3.right : Vector3.left;
        Gizmos.DrawLine(transform.position, transform.position + direction * visionDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}
