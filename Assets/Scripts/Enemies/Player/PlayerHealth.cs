using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f; // Максимальное количество здоровья
    private float currentHealth;

    public float moveSpeed = 5f; // Скорость передвижения игрока
    public float jumpForce = 10f; // Сила прыжка игрока
    private bool isGrounded = true; // Флаг для проверки, находится ли игрок на земле

    private Rigidbody rb; // Ссылка на Rigidbody игрока

    void Start()
    {
        currentHealth = maxHealth; // Инициализация здоровья
        rb = GetComponent<Rigidbody>(); // Получаем компонент Rigidbody
    }

    void Update()
    {
        // Управление движением игрока
        Move();

        // Проверяем, если игрок нажимает пробел и находится на земле, то он прыгает
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal"); // Получаем ось для горизонтального движения (A/D или стрелки)
        Vector3 movement = new Vector3(moveInput * moveSpeed, rb.velocity.y, 0); // Двигаем игрока по оси X
        rb.velocity = movement; // Применяем движение к Rigidbody
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z); // Задаем скорость по Y для прыжка
        isGrounded = false; // Считаем, что игрок в прыжке
    }

    void OnCollisionEnter(Collision collision)
    {
        // Если игрок касается земли, то он снова может прыгать
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage; // Уменьшаем здоровье на значение урона
        Debug.Log("Player Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player has died!");
        // Здесь можно добавить логику смерти игрока (например, перезапуск уровня)
    }
}
