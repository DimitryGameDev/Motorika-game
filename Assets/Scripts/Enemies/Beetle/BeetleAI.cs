using UnityEngine;

public class BeetleAI : MonoBehaviour
{
    public float moveForwardDistance;  // Движение по оси Z вперёд (вправо)
    public float moveDownDistance;     // Движение по оси Y вниз
    public float moveBackwardDistance; // Движение по оси Z назад (влево)
    public float moveUpDistance;       // Движение по оси Y вверх

    public float speed;
    public int damage;
    public float knockbackForce;

    public GameObject playerPrefab;  // Ссылка на префаб игрока

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
        // Обновляем положение жука
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Проверяем, достиг ли жук цели
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            SetNextTarget();
        }

        // Поворачиваем жука в направлении движения, игнорируя вращение по оси X и Y
        Vector3 directionOfMovement = (targetPosition - startPosition).normalized;
        directionOfMovement.y = 0; // Игнорируем вертикальную составляющую для предотвращения вращения по оси X и Y
        if (directionOfMovement != Vector3.zero)
        {
            transform.forward = directionOfMovement;
        }
    }

    void SetNextTarget()
    {
        switch (currentDirection)
        {
            case 0: // Движение назад по оси Z (влево)
                targetPosition = startPosition + new Vector3(0, 0, -moveBackwardDistance);
                break;
            case 1: // Движение вниз по оси Y
                targetPosition = startPosition + new Vector3(0, -moveDownDistance, -moveBackwardDistance);
                break;
            case 2: // Движение вперёд по оси Z (вправо)
                targetPosition = startPosition + new Vector3(0, -moveDownDistance, 0);
                break;
            case 3: // Движение вверх по оси Y
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
        GameObject collidedObject = collision.gameObject;

        if (collidedObject == playerPrefab)
        {
            // Получаем компонент Destructable у игрока
            Destructable playerDestructable = collidedObject.GetComponent<Destructable>();
            if (playerDestructable != null)
            {
                // Наносим урон игроку
                playerDestructable.ApplyDamage(damage);

                // Рассчитываем направление отталкивания
                Vector3 directionToPlayer = collidedObject.transform.position - transform.position;
                Vector3 beetleForward = transform.forward;

                // Проверяем, находится ли игрок в направлении движения жука
                if (IsPlayerInFront(beetleForward, directionToPlayer))
                {
                    // Отталкиваем игрока в сторону движения жука
                    Rigidbody playerRb = collidedObject.GetComponent<Rigidbody>();
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
        // Проверяем, находится ли игрок перед жуком по направлению его движения
        float angle = Vector3.Angle(beetleForward, directionToPlayer);
        return angle < 45f;  // Игрок должен находиться перед жуком
    }
}
