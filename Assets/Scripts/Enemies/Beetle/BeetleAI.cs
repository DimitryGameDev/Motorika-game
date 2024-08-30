using UnityEngine;

public class BeetleAI : MonoBehaviour
{
    public float moveRightDistance;
    public float moveDownDistance;
    public float moveLeftDistance;
    public float moveUpDistance; 

    public float speed; 
    public int damage; 
    public float knockbackForce; 

    public GameObject playerPrefab;

    private Vector3 startPosition; 
    private Vector3 targetPosition; 
    private int currentDirection = 0; 

    void Start()
    {
        startPosition = transform.position;
        SetNextTarget();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            SetNextTarget();
        }
    }

    void SetNextTarget()
    {
        switch (currentDirection)
        {
            case 0: 
                targetPosition = startPosition + new Vector3(moveRightDistance, 0, 0);
                break;
            case 1: 
                targetPosition = startPosition + new Vector3(moveRightDistance, -moveDownDistance, 0);
                break;
            case 2: 
                targetPosition = startPosition + new Vector3(0, -moveDownDistance, 0);
                break;
            case 3: 
                targetPosition = startPosition + new Vector3(0, 0, 0);
                break;
        }

        currentDirection = (currentDirection + 1) % 4;

        if (currentDirection == 0) 
        {
            startPosition = targetPosition;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject player = collision.gameObject;

        if (player == playerPrefab)
        {
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

            if (IsPlayerInFront(directionToPlayer))
            {
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                }

                Rigidbody playerRb = player.GetComponent<Rigidbody>();
                if (playerRb != null)
                {
                    playerRb.AddForce(directionToPlayer * knockbackForce, ForceMode.Impulse);
                }
            }
        }
    }

    bool IsPlayerInFront(Vector3 directionToPlayer)
    {
        Vector3 beetleForward = transform.right;

        float angle = Vector3.Angle(beetleForward, directionToPlayer);

        return angle < 45f;
    }
}
