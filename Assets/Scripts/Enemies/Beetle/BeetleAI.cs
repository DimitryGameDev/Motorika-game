using UnityEngine;

//
public class BeetleAI : Destructible
{
    [SerializeField] private float moveForwardDistance;
    [SerializeField] private float moveDownDistance;
    [SerializeField] private float moveBackwardDistance;
    [SerializeField] private float moveUpDistance;

    [SerializeField] private float speed;
    [SerializeField] private int damage;
    [SerializeField] private float knockbackForce;

    public GameObject playerPrefab;

    private Destructible destructible;
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

        Vector3 directionOfMovement = (targetPosition - startPosition).normalized;
        directionOfMovement.y = 0;
        if (directionOfMovement != Vector3.zero)
        {
            transform.forward = directionOfMovement;
        }
    }

    void SetNextTarget()
    {
        switch (currentDirection)
        {
            case 0:
                targetPosition = startPosition + new Vector3(0, 0, -moveBackwardDistance);
                break;
            case 1:
                targetPosition = startPosition + new Vector3(0, -moveDownDistance, -moveBackwardDistance);
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

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == playerPrefab)
        {
            Destructible playerDestructable = other.GetComponent<Destructible>();
            if (playerDestructable != null)
            {
                playerDestructable.ApplyDamage(damage);

                Vector3 directionToPlayer = other.transform.position - transform.position;
                Vector3 beetleForward = transform.forward;

                if (IsPlayerInFront(beetleForward, directionToPlayer))
                {
                    Rigidbody playerRb = other.GetComponent<Rigidbody>();
                    if (playerRb != null)
                    {
                        playerRb.AddForce(beetleForward * knockbackForce, ForceMode.Impulse);
                    }
                }
            }
        }
    }

    bool IsPlayerInFront(Vector3 beetleForward, Vector3 directionToPlayer)
    {
        float angle = Vector3.Angle(beetleForward, directionToPlayer);
        return angle < 45f;
    }
}
