using UnityEngine;
using UnityEngine.Events;

public class Player : MonoSingleton<Player>
{
    public event UnityAction RunEvent; // event
    public event UnityAction JumpEvent;
    public event UnityAction SlideEvent;

    [SerializeField] private float runSpeed = 5f; //run velocity
    [SerializeField] private float jumpForce = 3f; // jump force
    [SerializeField] private float jumpControlTime = 0.6f; // max jump time
    [SerializeField] private float slideSpeed = 10f; // slide speed
    [SerializeField] private float slideControlTime = 0.6f; // max slide time

    [SerializeField] private int damage = 10;

    [SerializeField] private float raycastDistance = 1.5f; // Raycast distance from player to value;
    [SerializeField] private float rayOffset = 0.5f; //distance Ray from object center 

    private bool isGrounded; // check ground
    private bool isJumping; // check jump
    private bool isSliding; // check slide

    private float jumpTime = 0;
    private float slideTime = 0;

    private Rigidbody rb;
    private Collider playerCollider;
    private Turret turret;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponentInChildren<Collider>();
        turret = GetComponentInChildren<Turret>();
    }

    private void FixedUpdate()
    {
        rb.freezeRotation = true;
        transform.up = Vector3.up;
        transform.position = new Vector3(0, transform.position.y, transform.position.z);

        if (turret != null)
        {
            if (Input.GetMouseButton(0))
            {
                turret.Fire();
            }
        }

        Slide();
        Jump();
    }

    private void Run(float speed)
    {
        RunEvent?.Invoke();

        if (IsBarrier())
            speed = 0f;
        else
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
                transform.localScale = new Vector3(1, 0.5f, 1);
                Run(slideSpeed);
            }
        }
        else
        {
            slideTime = 0;
            transform.localScale = new Vector3(1, 1, 1);
        }
        Run(runSpeed);
    }

    private void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && IsBarrier())
        {
            rb.velocity = new Vector3(0f, jumpForce, rb.velocity.z);
            isJumping = true;
        }

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
        RaycastHit hit;
        RaycastHit hit2;

        Vector3 raycastTopPosition = new Vector3(playerCollider.bounds.center.x, playerCollider.bounds.max.y - rayOffset, playerCollider.bounds.max.z);
        Vector3 raycastBottomPosition = new Vector3(playerCollider.bounds.center.x, playerCollider.bounds.min.y + rayOffset, playerCollider.bounds.max.z);
#if UNITY_EDITOR
        Debug.DrawRay(raycastTopPosition, transform.forward * raycastDistance, Color.red);
        Debug.DrawRay(raycastBottomPosition, transform.forward * raycastDistance, Color.red);
#endif

        bool hitBottom = Physics.Raycast(raycastBottomPosition, transform.forward, out hit, raycastDistance);
        bool hitTop = Physics.Raycast(raycastTopPosition, transform.forward, out hit2, raycastDistance);

        if (hitBottom || hitTop)
        {
            RaycastHit usedHit = hitBottom ? hit : hit2;
            Destructable destructable = usedHit.collider.GetComponent<Destructable>();

            TakeDamage(destructable);

            //Debug.Log("Hit");
            return true;
        }
        else
        {
            //Debug.Log("No hit");
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

    private void TakeDamage(Destructable destructable)
    {
        if (destructable != null)
        {
            destructable.transform.root.GetComponent<Destructable>();

            if (isSliding)
            {
                destructable.ApplyDamage(damage);
            }
        }
    }

    public void Fire(TurretMode mode)
    {
        return;
    }

    public bool DrawEnergy(int count)
    {
        return true;
    }

    public bool DrawAmmo(int count)
    {
        return true;
    }
}