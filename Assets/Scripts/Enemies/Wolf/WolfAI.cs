using System.Collections;
using UnityEngine;

//
public class WolfAI : Destructible
{
    public enum EnemyState
    {
        Patrolling,
        Attacking
    }

    [SerializeField] private float moveDistance;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float pauseDuration;
    [SerializeField] private float visionDistance;
    [SerializeField] private float stopDistance;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackInterval;
    [SerializeField] private float verticalTolerance;

    private Parry parry;
    private Vector3 startPosition;
    private bool movingForward = true;
    private bool isPaused = false;
    private EnemyState currentState = EnemyState.Patrolling;
    private bool isAttacking = false;

    private GameObject player;
    private Destructible destructible;

    private void Start()
    {
        startPosition = transform.position;

        player = GameObject.Find(playerPrefab.name);
        if (player != null)
        {
            destructible = player.GetComponent<Destructible>();
            parry = player.GetComponent<Parry>();
        }
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
                ChasePlayer();
                DetectPlayer();
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
        if (destructible == null)
        {
            return;
        }

        Vector3 direction = movingForward ? Vector3.forward : Vector3.back;
        Vector3 playerPosition = new Vector3(transform.position.x, transform.position.y, destructible.transform.position.z);
        Vector3 enemyPosition = transform.position;
        Vector3 toPlayer = playerPosition - enemyPosition;

        float verticalDistance = Mathf.Abs(destructible.transform.position.y - transform.position.y);
        if (verticalDistance <= verticalTolerance && Vector3.Dot(toPlayer.normalized, direction) > 0 && toPlayer.magnitude <= visionDistance)
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

    private void ChasePlayer()
    {
        if (destructible == null)
        {
            return;
        }

        Vector3 playerPosition = new Vector3(transform.position.x, transform.position.y, destructible.transform.position.z);
        Vector3 enemyPosition = transform.position;
        float distanceToPlayer = Vector3.Distance(playerPosition, enemyPosition);

        float verticalDistance = Mathf.Abs(destructible.transform.position.y - transform.position.y);
        if (verticalDistance <= verticalTolerance && distanceToPlayer > stopDistance)
        {
            Vector3 direction = (playerPosition - enemyPosition).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
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

    private IEnumerator AttackPlayer()
    {
        if (parry != null && parry.ParryTimer > 0)
        {
            isAttacking = true;
            while (isAttacking)
            {
                if (destructible != null && !destructible.IsIndestructable)
                {
                    destructible.ApplyDamage((int)attackDamage);
                }

                yield return new WaitForSeconds(attackInterval);
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 direction = movingForward ? Vector3.forward : Vector3.back;
        Gizmos.DrawLine(transform.position, transform.position + direction * visionDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
#endif
}
