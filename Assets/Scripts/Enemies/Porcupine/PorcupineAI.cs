using UnityEngine;
using System.Collections;

public class PorcupineAI : MonoBehaviour
{
    public enum EnemyState
    {
        Patrolling,
        Defending
    }

    [SerializeField] private float moveDistance;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float pauseDuration;
    [SerializeField] private float visionDistance;
    [SerializeField] private GameObject playerPrefab; // Передаваемый префаб игрока
    [SerializeField] private GameObject projectilePrefab; // Префаб снаряда
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float fireRate;

    private Vector3 startPosition;
    private bool movingForward = true;
    private bool isPaused = false;
    private EnemyState currentState = EnemyState.Patrolling;
    private Coroutine fireCoroutine;
    private GameObject player; // Игрок
    private Destructible playerHealth; 

    private void Start()
    {
        startPosition = transform.position;

        // Ищем игрока через передаваемый префаб
        player = GameObject.Find(playerPrefab.name);
        if (player != null)
        {
            //Берём у него здоровье
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

        if (toPlayer.magnitude <= visionDistance)
        {
            currentState = EnemyState.Defending;
            StartDefending();
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

        return toPlayer.magnitude > visionDistance;
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
            FireProjectileInDirection(Vector3.forward);     // Прямо вперед
            FireProjectileInDirection(Vector3.back);        // Прямо назад
            FireProjectileInDirection(Vector3.up);          // Прямо вверх

            FireProjectileInDirection((Vector3.forward + Vector3.up).normalized); // Вправо-вверх
            FireProjectileInDirection((Vector3.back + Vector3.up).normalized);    // Влево-вверх

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
            projectileScript.SetSpeed(projectileSpeed);
        }

        // Поворот снаряда в зависимости от направления
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
        else if (direction == Vector3.left)
        {
            projectile.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else if (direction == Vector3.right)
        {
            projectile.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (direction == (Vector3.forward + Vector3.up).normalized)  // Вправо-вверх
        {
            projectile.transform.rotation = Quaternion.Euler(45, 45, 0);
        }
        else if (direction == (Vector3.back + Vector3.up).normalized)     // Влево-вверх
        {
            projectile.transform.rotation = Quaternion.Euler(-45, -45, 0);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, visionDistance);
    }
#endif
}
