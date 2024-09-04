using UnityEngine;
using UnityEngine.Events;

public class Player : MonoSingleton<Player>
{
    public event UnityAction RunEvent; // event
    public event UnityAction JumpEvent;
    public event UnityAction SlideEvent;

    [Header("Run/Slide")]
    [SerializeField] private float runSpeed = 5f; //run velocity
    [SerializeField] private float slideSpeed = 10f; // slide speed
    [SerializeField] private float slideControlTime = 0.6f; // max slide time
    [Header("Jump")]
    [SerializeField] private float minJumpForce; // base jump force
    [SerializeField] private float maxJumpForce; // additional max jump force
    [SerializeField] private float chargeRate = 0.6f; // max jump time
    [Header("Damage")]
    [SerializeField] private int damage = 10;
    [Header("Raycast")]
    [SerializeField] private float raycastDistanceForward = 1.5f; // Raycast distance from player to value;
    [SerializeField] private float raycastDistanceDown = 1.5f; // Raycast distance from player to value;
    [SerializeField] private float rayOffset = 0.5f; //distance Ray from object center 

    private bool isGrounded; // check ground
    private bool isJumping; // check jump
    private bool isSliding; // check slide

    private float currentJumpForce = 0f;
    private float slideTime = 0;

    private Animator animator;
    private Rigidbody rb;
    private Collider playerCollider;
    private Turret turret;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponentInChildren<Collider>();
        turret = GetComponentInChildren<Turret>();
    }

    private void Update()
    {
        CheckGround();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isJumping = true;
            JumpEvent?.Invoke();
            animator.SetBool("isJump", true);
        }

        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            currentJumpForce += chargeRate * Time.deltaTime;
            currentJumpForce = Mathf.Clamp(currentJumpForce, minJumpForce, maxJumpForce);

            if (currentJumpForce != maxJumpForce)
            {
                Jump();
            }
            else
            {
                animator.SetBool("isJump", false);
                currentJumpForce = minJumpForce;
                isJumping = false;
            }
        }
    }

    private void FixedUpdate()
    {
        rb.freezeRotation = true;
        transform.up = Vector3.up;
        transform.position = new Vector3(0, transform.position.y, transform.position.z);

        if (Input.GetMouseButton(0) && turret != null)
        {
            turret.Fire();
        }

        if (Input.GetKey(KeyCode.S) && isGrounded)
        {
            Slide();
        }
        else if (!Input.GetKey(KeyCode.S))
        {
            RunEvent?.Invoke();
            Run(runSpeed);
            ResetAnimations();
        }
    }

    public void Run(float speed)
    {
        if (!IsBarrier())
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            animator.SetBool("isRun", true);
            animator.SetBool("isIdle", false);
        }
        else
        {
            if(IsBarrier()) 
            animator.SetBool("isIdle", true);

            animator.SetBool("isRun", false);
        }
    }

    private void Slide()
    {
        isSliding = true;
        slideTime += Time.deltaTime;

        if (slideTime < slideControlTime)
        {
            SlideEvent?.Invoke();
            animator.SetBool("isSlide", true);
            Run(slideSpeed);
        }
        else
        {
            isSliding = false;
            slideTime = 0;
        }
        animator.SetBool("isSlide", isSliding);
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up * currentJumpForce, ForceMode.Impulse);
        isGrounded = false;
        //animator.SetTrigger("Jump");
    }

    private void ResetAnimations()
    {
        animator.SetBool("isJump", false);
        animator.SetBool("isSlide", false);
    }

    private bool IsBarrier()
    {
        Vector3 raycastTopPosition = new(playerCollider.bounds.center.x, playerCollider.bounds.max.y - rayOffset, playerCollider.bounds.max.z);
        Vector3 raycastBottomPosition = new(playerCollider.bounds.center.x, playerCollider.bounds.min.y + rayOffset, playerCollider.bounds.max.z);
#if UNITY_EDITOR
        Debug.DrawRay(raycastTopPosition, transform.forward * raycastDistanceForward, Color.red);
        Debug.DrawRay(raycastBottomPosition, transform.forward * raycastDistanceForward, Color.red);
#endif
        bool hitBottom = Physics.Raycast(raycastBottomPosition, transform.forward, out RaycastHit hit, raycastDistanceForward);
        bool hitTop = Physics.Raycast(raycastTopPosition, transform.forward, out RaycastHit hit2, raycastDistanceForward);

        if (hitBottom || hitTop)
        {
            RaycastHit usedHit = hitBottom ? hit : hit2;
            Destructible destructible = usedHit.collider.GetComponentInChildren<Destructible>();

            TakeDamage(destructible);

            return true;
        }
        else
        {
            return false;
        }
    }

    private void CheckGround()
    {
        Vector3 raycastDownPosition = new(playerCollider.bounds.center.x, playerCollider.bounds.min.y + rayOffset, playerCollider.bounds.max.z);

        Debug.DrawRay(raycastDownPosition, -transform.up * raycastDistanceDown, Color.blue);
        isGrounded = Physics.Raycast(raycastDownPosition, -transform.up, out _, raycastDistanceDown);
    }

    public void TakeDamage(Destructible destructable)
    {
        if (destructable != null)
        {
            destructable.transform.root.GetComponent<Destructible>();

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