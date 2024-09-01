using UnityEngine;
using System.Collections;

public class PorcupineAI : MonoBehaviour
{
    public enum EnemyState
    {
        Patrolling,
        Defending
    }

    public float moveDistance;
    public float moveSpeed;
    public float pauseDuration;
    public float visionDistance;
    public GameObject playerPrefab;
    public GameObject projectilePrefab;
    public float projectileSpeed;  // Скорость снаряда
    public float fireRate; 

    private Vector3 startPosition;
    private bool movingForward = true; // направление движения по оси Z
    private bool isPaused = false;
    private EnemyState currentState = EnemyState.Patrolling;
    private Destructable playerHealth; // Ссылка на компонент Destructable
    private Coroutine fireCoroutine;

    void Start()
    {
        startPosition = transform.position;

        // Найти объект игрока и получить компонент Destructable
        GameObject player = GameObject.Find(playerPrefab.name);
        if (player != null)
        {
            playerHealth = player.GetComponent<Destructable>();
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

            case EnemyState.Defending:
                if (IsPlayerOutOfSight())
                {
                    StopDefending();
                }
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
        if (playerHealth == null)
        {
            return; // Если игрок не найден, ничего не делаем
        }

        Vector3 playerPosition = new Vector3(transform.position.x, transform.position.y, playerHealth.transform.position.z);
        Vector3 enemyPosition = transform.position;
        Vector3 toPlayer = playerPosition - enemyPosition;

        if (toPlayer.magnitude <= visionDistance)
        {
            // Переходим в состояние защиты, если игрок в зоне видимости
            currentState = EnemyState.Defending;
            StartDefending();
        }
    }

    bool IsPlayerOutOfSight()
    {
        if (playerHealth == null)
        {
            return true;
        }

        Vector3 playerPosition = new Vector3(transform.position.x, transform.position.y, playerHealth.transform.position.z);
        Vector3 toPlayer = playerPosition - transform.position;

        // Проверка, находится ли игрок за пределами зоны видимости
        return toPlayer.magnitude > visionDistance;
    }

    void StartDefending()
    {
        // Начинаем стрелять снарядами в три(!) направления
        if (fireCoroutine == null)
        {
            fireCoroutine = StartCoroutine(FireProjectiles());
        }
    }

    void StopDefending()
    {
        // Останавливаем стрельбу и возвращаемся к патрулированию
        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
            fireCoroutine = null;
        }
        currentState = EnemyState.Patrolling;
    }

    IEnumerator FireProjectiles()
    {
        while (currentState == EnemyState.Defending)
        {
            FireProjectileInDirection(Vector3.forward);   // Стрельба вперёд (по оси Z вперед)
            FireProjectileInDirection(Vector3.back);      // Стрельба назад (по оси Z назад)
            FireProjectileInDirection(Vector3.up);        // Стрельба вверх (по оси Y вверх)

            yield return new WaitForSeconds(1f / fireRate); // Задержка между залпами
        }
    }

    void FireProjectileInDirection(Vector3 direction)
    {
        // Создаем снаряд
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Получаем скрипт снаряда и устанавливаем направление и скорость
        ProjectilePorcupine projectileScript = projectile.GetComponent<ProjectilePorcupine>();
        if (projectileScript != null)
        {
            projectileScript.SetDirection(direction); // Устанавливаем направление снаряда
            projectileScript.SetSpeed(projectileSpeed); // Устанавливаем скорость снаряда
        }

        // Поворачиваем снаряд в зависимости от направления
        if (direction == Vector3.up)
        {
            // Снаряд идет вверх
            projectile.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (direction == Vector3.forward)
        {
            // Снаряд идет вперед
            projectile.transform.rotation = Quaternion.Euler(0, 90, 90);
        }
        else if (direction == Vector3.back)
        {
            // Снаряд идет назад
            projectile.transform.rotation = Quaternion.Euler(0, 90, 90);
        }
        else if (direction == Vector3.left)
        {
            // Снаряд идет влево
            projectile.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else if (direction == Vector3.right)
        {
            // Снаряд идет вправо
            projectile.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, visionDistance);
    }
}