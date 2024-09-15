using UnityEngine;
using System.Collections;

public class MedvekulaAI : Enemy
{
    public enum EnemyState
    {
        Patrolling,
        Attacking,
        Chasing
    }

    [SerializeField] private float moveDistance;
    [SerializeField] private float pauseDuration;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float fireRate;
    [SerializeField] private float projectileSpawnYOffset;
    [SerializeField] private float undergroundSpeed;
    [SerializeField] private float undergroundYOffset;
    [SerializeField] private float additionalOffset;

    private Vector3 startPosition;
    private bool movingForward = true;
    private bool isPaused = false;
    private EnemyState currentState = EnemyState.Patrolling;
    private Coroutine fireCoroutine;
    private Player player;

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
                LookForPlayer();  
                break;

            case EnemyState.Attacking:
                if (player != null && IsPlayerOutOfSight())
                {
                    DetermineNextAction(); 
                }
                break;

            case EnemyState.Chasing:
                ChasePlayer();
                break;
        }
    }

    private void Move()
    {
        if (movingForward)
        {
            transform.position += Vector3.forward * MoveSpeed * Time.deltaTime;

            if (transform.position.z >= startPosition.z + moveDistance)
            {
                StartCoroutine(PauseBeforeChangeDirection());
            }
        }
        else
        {
            transform.position += Vector3.back * MoveSpeed * Time.deltaTime;

            if (transform.position.z <= startPosition.z - moveDistance)
            {
                StartCoroutine(PauseBeforeChangeDirection());
            }
        }
    }

    private IEnumerator PauseBeforeChangeDirection()
    {
        isPaused = true;
        yield return new WaitForSeconds(pauseDuration);
        movingForward = !movingForward;
        isPaused = false;
    }

    private void LookForPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, VisionDistance);

        foreach (Collider hit in hits)
        {
            Player playerComponent = hit.GetComponentInParent<Player>();
            if (playerComponent != null)
            {
                player = playerComponent;
                currentState = EnemyState.Attacking;
                StartAttacking();
                break;
            }
        }
    }

    private bool IsPlayerOutOfSight()
    {
        if (player == null)
        {
            return true;
        }

        Vector3 toPlayer = player.transform.position - transform.position;
        return toPlayer.magnitude > VisionDistance;
    }

    private void DetermineNextAction()
    {
        if (player.transform.position.z > transform.position.z)
        {
            currentState = EnemyState.Chasing;
        }
        else
        {
            StartAttacking();
        }
    }

    private void StartAttacking()
    {
        if (fireCoroutine == null)
        {
            fireCoroutine = StartCoroutine(FireProjectiles());
        }
    }

    private void StopAttacking()
    {
        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
            fireCoroutine = null;
        }
    }

    private IEnumerator FireProjectiles()
    {
        while (currentState == EnemyState.Attacking)
        {
            FireProjectileInDirection(Vector3.back);
            yield return new WaitForSeconds(1f / fireRate);
        }
    }

    private void FireProjectileInDirection(Vector3 direction)
    {
        Vector3 spawnPosition = transform.position + new Vector3(0, projectileSpawnYOffset, 0);
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

        ProjectilePorcupine projectileScript = projectile.GetComponent<ProjectilePorcupine>();
        if (projectileScript != null)
        {
            projectileScript.SetDirection(direction);
            projectileScript.SetSpeed(ProjectileSpeed);
        }

        if (direction == Vector3.back)
        {
            projectile.transform.rotation = Quaternion.Euler(0, 90, 90);
        }
        else if (direction == Vector3.left)
        {
            projectile.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
    }

    private void ChasePlayer()
    {
        Vector3 undergroundPosition = new Vector3(transform.position.x, -10f, transform.position.z);
        transform.position = undergroundPosition;

        StartCoroutine(MoveToPlayerRightSide());
    }

    private IEnumerator MoveToPlayerRightSide()
    {
        yield return new WaitForSeconds(0.5f);

        if (player != null)
        {
            Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, player.transform.position.z + additionalOffset);
            transform.position = newPosition;

            transform.position = new Vector3(transform.position.x, startPosition.y, transform.position.z);
        }

        currentState = EnemyState.Attacking;
        StartAttacking();
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, VisionDistance);
    }
#endif
}
