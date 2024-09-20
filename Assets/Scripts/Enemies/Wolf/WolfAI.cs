using System.Collections;
using UnityEngine;

public class WolfAI : Enemy
{
    public enum EnemyState
    {
        Patrolling,
        Attacking
    }

    [SerializeField] private float moveDistance;
    [SerializeField] private float pauseDuration;
    [SerializeField] private float stopDistance;
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackInterval;
    [SerializeField] private float verticalTolerance;

    private Parry parry;
    private Vector3 startPosition;
    private bool movingForward = true;
    private bool isPaused = false;
    private EnemyState currentState = EnemyState.Patrolling;
    private bool isAttacking = false;

    private Player player;
    private Destructible destructible;

    private void Start()
    {
        startPosition = transform.position;
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

        Vector3 newPosition = transform.position + moveDirection * MoveSpeed * Time.deltaTime;

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
        Collider[] hits = Physics.OverlapSphere(transform.position, VisionDistance);

        foreach (Collider hit in hits)
        {
            Player playerComponent = hit.GetComponentInParent<Player>();
            if (playerComponent != null)
            {
                player = playerComponent;
                destructible = player.GetComponent<Destructible>();
                parry = player.GetComponent<Parry>();
                currentState = EnemyState.Attacking;
                return;
            }
        }

        if (destructible == null)
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
            transform.position += direction * MoveSpeed * Time.deltaTime;
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
    // ????? TODO: 
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
        Gizmos.DrawWireSphere(transform.position, VisionDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
#endif
}
