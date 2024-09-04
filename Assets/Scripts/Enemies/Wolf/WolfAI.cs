using UnityEngine;
using System.Collections;

public class WolfAI : Destructible
{
    public enum EnemyState
    {
        Patrolling,
        Attacking
    }

    public float moveDistance;
    public float moveSpeed;
    public float pauseDuration;
    public float visionDistance;
    public float stopDistance;
    public GameObject playerPrefab;
    public float attackDamage;
    public float attackInterval;
    public float verticalTolerance;
    private Parry parry;
    private Vector3 startPosition;
    private bool movingForward = true; // ??????????? ???????? ?? ??? Z
    private bool isPaused = false;
    private EnemyState currentState = EnemyState.Patrolling;
    private bool isAttacking = false;
    private GameObject player;
    private Destructible destructible; // ?????? ?? ????????? Destructable

    void Start()
    {
        startPosition = transform.position;

        // ????? ?????? ?????? ? ???????? ????????? Destructable
        player = GameObject.Find(playerPrefab.name);
        if (player != null)
        {
            destructible = player.GetComponent<Destructible>();
            parry = player.GetComponent<Parry>();
        }
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
        if (movingForward)
        {
            transform.position += Vector3.forward * moveSpeed * Time.deltaTime;

            if (transform.position.z >= startPosition.z + moveDistance)
            {
                StartCoroutine(PauseBeforeChangeDirection());
            }
        }
        else
        {
            transform.position += Vector3.back * moveSpeed * Time.deltaTime;

            if (transform.position.z <= startPosition.z - moveDistance)
            {
                StartCoroutine(PauseBeforeChangeDirection());
            }
        }
    }

    IEnumerator PauseBeforeChangeDirection()
    {
        isPaused = true;
        yield return new WaitForSeconds(pauseDuration);
        movingForward = !movingForward;
        isPaused = false;
    }

    void DetectPlayer()
    {
        if (destructible == null)
        {
            return; // ???? ????? ?? ??????, ?????? ?? ??????
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

    void ChasePlayer()
    {
        if (destructible == null)
        {
            return; // ???? ????? ?? ??????, ?????? ?? ??????
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

    IEnumerator AttackPlayer()
    {
       
        if (parry.ParryTimer > 0)
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
   
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 direction = movingForward ? Vector3.forward : Vector3.back;
        Gizmos.DrawLine(transform.position, transform.position + direction * visionDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}
