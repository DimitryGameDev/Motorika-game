using UnityEngine;
using System.Collections;

public class MedvekulaAI : Enemy
{
    public enum EnemyState
    {
        Patrolling,
        Attacking
    }

    [SerializeField] private float moveDistance;
    [SerializeField] private float pauseDuration;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float fireRate;
    [SerializeField] private float projectileSpawnYOffset; 

    private Vector3 startPosition;
    private bool movingForward = true;
    private bool isPaused = false;
    private EnemyState currentState = EnemyState.Patrolling;
    private Coroutine fireCoroutine;
    private GameObject player;
    private Destructible playerHealth;

    private void Start()
    {
        startPosition = transform.position;
        player = GameObject.Find(playerPrefab.name);

        if (player != null)
        {
            playerHealth = player.GetComponent<Destructible>();
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
                if (IsPlayerOutOfSight())
                {
                    StopAttacking();
                }
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

    private void DetectPlayer()
    {
        if (playerHealth == null)
        {
            return;
        }

        Vector3 direction = movingForward ? Vector3.forward : Vector3.back;
        Vector3 playerPosition = new Vector3(transform.position.x, transform.position.y, playerHealth.transform.position.z);
        Vector3 enemyPosition = transform.position;
        Vector3 toPlayer = playerPosition - enemyPosition;

        if (toPlayer.magnitude <= VisionDistance)
        {
            currentState = EnemyState.Attacking;
            StartAttacking();
        }
    }

    private bool IsPlayerOutOfSight()
    {
        if (playerHealth == null)
        {
            return true;
        }

        Vector3 playerPosition = new Vector3(transform.position.x, transform.position.y, playerHealth.transform.position.z);
        Vector3 toPlayer = playerPosition - transform.position;

        return toPlayer.magnitude > VisionDistance;
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
        currentState = EnemyState.Patrolling;
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

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, VisionDistance);
    }
#endif
}
