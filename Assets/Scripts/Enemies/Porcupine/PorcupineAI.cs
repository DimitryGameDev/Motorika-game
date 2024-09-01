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
    public float projectileSpeed;  // �������� �������
    public float fireRate; 

    private Vector3 startPosition;
    private bool movingForward = true; // ����������� �������� �� ��� Z
    private bool isPaused = false;
    private EnemyState currentState = EnemyState.Patrolling;
    private Destructable playerHealth; // ������ �� ��������� Destructable
    private Coroutine fireCoroutine;

    void Start()
    {
        startPosition = transform.position;

        // ����� ������ ������ � �������� ��������� Destructable
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
            return; // ���� ����� �� ������, ������ �� ������
        }

        Vector3 playerPosition = new Vector3(transform.position.x, transform.position.y, playerHealth.transform.position.z);
        Vector3 enemyPosition = transform.position;
        Vector3 toPlayer = playerPosition - enemyPosition;

        if (toPlayer.magnitude <= visionDistance)
        {
            // ��������� � ��������� ������, ���� ����� � ���� ���������
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

        // ��������, ��������� �� ����� �� ��������� ���� ���������
        return toPlayer.magnitude > visionDistance;
    }

    void StartDefending()
    {
        // �������� �������� ��������� � ���(!) �����������
        if (fireCoroutine == null)
        {
            fireCoroutine = StartCoroutine(FireProjectiles());
        }
    }

    void StopDefending()
    {
        // ������������� �������� � ������������ � ��������������
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
            FireProjectileInDirection(Vector3.forward);   // �������� ����� (�� ��� Z ������)
            FireProjectileInDirection(Vector3.back);      // �������� ����� (�� ��� Z �����)
            FireProjectileInDirection(Vector3.up);        // �������� ����� (�� ��� Y �����)

            yield return new WaitForSeconds(1f / fireRate); // �������� ����� �������
        }
    }

    void FireProjectileInDirection(Vector3 direction)
    {
        // ������� ������
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // �������� ������ ������� � ������������� ����������� � ��������
        ProjectilePorcupine projectileScript = projectile.GetComponent<ProjectilePorcupine>();
        if (projectileScript != null)
        {
            projectileScript.SetDirection(direction); // ������������� ����������� �������
            projectileScript.SetSpeed(projectileSpeed); // ������������� �������� �������
        }

        // ������������ ������ � ����������� �� �����������
        if (direction == Vector3.up)
        {
            // ������ ���� �����
            projectile.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (direction == Vector3.forward)
        {
            // ������ ���� ������
            projectile.transform.rotation = Quaternion.Euler(0, 90, 90);
        }
        else if (direction == Vector3.back)
        {
            // ������ ���� �����
            projectile.transform.rotation = Quaternion.Euler(0, 90, 90);
        }
        else if (direction == Vector3.left)
        {
            // ������ ���� �����
            projectile.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else if (direction == Vector3.right)
        {
            // ������ ���� ������
            projectile.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, visionDistance);
    }
}