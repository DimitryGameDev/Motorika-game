using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public event UnityAction RunEvent;
    public event UnityAction JumpEvent;
    public event UnityAction SlideEvent;

    [SerializeField] private float runSpeed = 5f; //Скорость бега
    [SerializeField] private float jumpForce = 3f; // Сила прыжка
    [SerializeField] private float jumpControlTime = 0.6f; // Максимальное время прыжка
    [SerializeField] private float slideSpeed = 10f; // Скорость скольжения
    [SerializeField] private float slideControlTime = 0.6f; // Максимальное время скольжения

    [SerializeField] private Transform rayTransform; // пустой объект, дочерний от игрока, расположен внизу модели
    [SerializeField] private float raycastDistance = 1.5f; // Расстояние, на которое будет выпущен рейкаст

    private bool isGrounded; // Проверка на земле ли персонаж
    private bool isJumping; // Проверка, нажата ли кнопка прыжка
    private bool isSliding; // Проверка, активен ли скольжение

    private float jumpTime = 0;
    private float slideTime = 0;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        transform.up = Vector3.up;
        transform.position = new Vector3(0, transform.position.y, transform.position.z);

        if (IsBarrier() == false)
            Slide();

        Jump();
    }

    private void Run(float speed)
    {
        RunEvent?.Invoke();

        rb.velocity = new Vector3(0f, rb.velocity.y, speed);
        //transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void Slide()
    {
        if (Input.GetKey(KeyCode.S))
        {
            if (isGrounded)
                isSliding = true;
        }
        else
        {
            isSliding = false;
        }

        if (isSliding)
        {
            SlideEvent?.Invoke();
            if ((slideTime += Time.deltaTime) < slideControlTime)
            {
                gameObject.transform.localScale = new Vector3(1, 0.5f, 1);
                Run(slideSpeed);
            }
        }
        else
        {
            slideTime = 0;
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        Run(runSpeed);
    }

    private void Jump()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (isGrounded)
                isJumping = true;
        }
        else
        {
            isJumping = false;
        }

        if (isJumping)
        {
            JumpEvent?.Invoke();
            if ((jumpTime += Time.deltaTime) < jumpControlTime)
            {
                rb.velocity = new Vector3(0f, jumpForce / (jumpTime * 10), rb.velocity.z);
                //rb.AddForce(Vector3.up * jumpForce / (jumpTime * 10));
            }
        }
        else
        {
            jumpTime = 0;
        }
    }
    private bool IsBarrier()
    {
        // Выпускаем рейкаст
        RaycastHit hit;
#if UNITY_EDITOR
        // Визуализация рейкаста (для отладки)
        Debug.DrawRay(rayTransform.transform.position, transform.forward * raycastDistance, Color.red);
#endif

        if (Physics.Raycast(rayTransform.transform.position, transform.forward, out hit, raycastDistance))
        {
            // Если рейкаст попал в коллайдер
            Debug.Log("Hit object: " + hit.collider.name);
            return true;
        }
        else
        {
            // Если рейкаст не попал в коллайдер
            Debug.Log("No hit");
            return false;
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
            isGrounded = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}