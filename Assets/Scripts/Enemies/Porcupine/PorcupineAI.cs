using UnityEngine;
using System.Collections;

public class PorcupineAI : Enemy
{
    public enum EnemyState
    {
        Patrolling,
        Defending
    }

    [SerializeField] private float moveDistance;
    [SerializeField] private float pauseDuration;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float fireRate;

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
                DetectPlayer();
                break;

            case EnemyState.Defending:
                if (IsPlayerOutOfSight())
                {
                    StopDefending();  
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
        Collider[] hits = Physics.OverlapSphere(transform.position, VisionDistance);

        foreach (Collider hit in hits)
        {
            Player playerComponent = hit.GetComponentInParent<Player>();
            if (playerComponent != null)
            {
                player = playerComponent;
                currentState = EnemyState.Defending;
                StartDefending();
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

    private void StartDefending()
    {
        if (fireCoroutine == null)
        {
            fireCoroutine = StartCoroutine(FireProjectiles());
        }
    }

    private void StopDefending()
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
        while (currentState == EnemyState.Defending)
        {
            FireProjectileInDirection(Vector3.forward);
            FireProjectileInDirection(Vector3.back);
            FireProjectileInDirection(Vector3.up);
            FireProjectileInDirection((Vector3.forward + Vector3.up).normalized);
            FireProjectileInDirection((Vector3.back + Vector3.up).normalized);

            yield return new WaitForSeconds(1f / fireRate);
        }
    }

    private void FireProjectileInDirection(Vector3 direction)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        ProjectilePorcupine projectileScript = projectile.GetComponent<ProjectilePorcupine>();
        if (projectileScript != null)
        {
            projectileScript.SetDirection(direction);
            projectileScript.SetSpeed(ProjectileSpeed);
        }

        if (direction == Vector3.up)
        {
            projectile.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (direction == Vector3.forward)
        {
            projectile.transform.rotation = Quaternion.Euler(0, 90, 90);
        }
        else if (direction == Vector3.back)
        {
            projectile.transform.rotation = Quaternion.Euler(0, 90, 90);
        }
        else if (direction == (Vector3.forward + Vector3.up).normalized)
        {
            projectile.transform.rotation = Quaternion.Euler(45, 45, 0);
        }
        else if (direction == (Vector3.back + Vector3.up).normalized)
        {
            projectile.transform.rotation = Quaternion.Euler(-45, -45, 0);
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
