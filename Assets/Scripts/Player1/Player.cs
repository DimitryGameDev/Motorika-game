using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public event UnityAction RunEvent;
    public event UnityAction JumpEvent;
    public event UnityAction SlideEvent;

    [SerializeField] private float runSpeed = 5f; //???????? ????
    [SerializeField] private float jumpForce = 3f; // ???? ??????
    [SerializeField] private float jumpControlTime = 0.6f; // ???????????? ????? ??????
    [SerializeField] private float slideSpeed = 10f; // ???????? ??????????
    [SerializeField] private float slideControlTime = 0.6f; // ???????????? ????? ??????????

    private bool isGrounded; // ???????? ?? ????? ?? ????????
    private bool isJumping; // ????????, ?????? ?? ?????? ??????
    private bool isSliding; // ????????, ??????? ?? ??????????

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

        Slide();

        Jump();
    }

    private void Run(float speed)
    {
        RunEvent?.Invoke();

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
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
                rb.AddForce(Vector3.up * jumpForce / (jumpTime * 10));
            }
        }
        else
        {
            jumpTime = 0;
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