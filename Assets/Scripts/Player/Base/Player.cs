using UnityEngine;

public class Player : MonoSingleton<Player>
{
    [Header("Run/Slide")]
    [SerializeField] private float runSpeed = 5f; //run velocity
    [SerializeField] private float slideSpeed = 10f; // slide speed
    [SerializeField] private float slideControlTime = 0.6f; // max slide time
    [Header("Jump")]
    [SerializeField] private float maxJumpForce; // additional max jump force
    [SerializeField] private float chargeRate; // max jump time
    [Header("Raycast")]
    [SerializeField] private float raycastDistanceForward = 1.5f; // Raycast distance from player to value;
    [SerializeField] private float raycastDistanceDown = 1.5f; // Raycast distance from player to value;
    [SerializeField] private float rayPositionTop = 0.5f; // Start position Ray on Top 
    [SerializeField] private float rayPositionBottom = 0.5f; // Start position Ray on Bottom 
    [Header("Collider")]
    [SerializeField] private Collider mainCollider;
    [SerializeField] private Collider slideCollider;

    private float currentJumpForce = 0f;
    private float slideTime = 0;

    private Rigidbody rb;
    private Animator animator;
    private Vector3 raycastDownPosition;
    private Vector3 raycastTopPosition;
    private Vector3 raycastBottomPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

        PlayerInputController.Instance.RunEvent += Run;
        PlayerInputController.Instance.JumpEvent += Jump;
        PlayerInputController.Instance.SlideEvent += Slide;
    }

    private void OnDestroy()
    {
        PlayerInputController.Instance.RunEvent -= Run;
        PlayerInputController.Instance.JumpEvent += Jump;
        PlayerInputController.Instance.SlideEvent -= Slide;
    }

    private void FixedUpdate()
    {
        rb.freezeRotation = true;
        transform.up = Vector3.up;
        transform.position = new Vector3(0, transform.position.y, transform.position.z);

        if (!Input.GetKey(KeyCode.S))
        {
            mainCollider.enabled = true;
            slideCollider.enabled = false;
        }
    }

    private void Run()
    {
        if (!IsBarrier())
        {
            transform.Translate(Vector3.forward * runSpeed * Time.deltaTime);

            if (IsGround())
                animator.SetTrigger("Run");
        }
        else if (IsGround())
            animator.SetTrigger("Idle");
    }

    private void Slide()
    {
        if (!IsGround()) return;

        animator.SetTrigger("Slide");

        slideTime = 0;

        mainCollider.enabled = false;
        slideCollider.enabled = true;

        slideTime += Time.deltaTime;

        if (slideTime < slideControlTime)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, slideSpeed);
            //transform.Translate(Vector3.forward * slideSpeed * Time.deltaTime);
        }
    }

    private void Jump()
    {
        if (!IsGround()) return;

        animator.SetTrigger("Jump");

        currentJumpForce = 0f;

        if (currentJumpForce < maxJumpForce)
        {
            currentJumpForce += Time.deltaTime * chargeRate;
            rb.AddForce(Vector3.up * currentJumpForce, ForceMode.Impulse);
        }
    }

    public void Parry(float parryForce)
    {
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);
        rb.AddForce(-transform.forward * parryForce, ForceMode.Impulse);
    }

    public bool IsBarrier()
    {
        raycastTopPosition = new(transform.position.x, transform.position.y + rayPositionTop, transform.position.z);
        raycastBottomPosition = new(transform.position.x, transform.position.y + rayPositionBottom, transform.position.z);
        bool hitBottom = Physics.Raycast(raycastBottomPosition, transform.forward, out _, raycastDistanceForward);
        bool hitTop = Physics.Raycast(raycastTopPosition, transform.forward, out _, raycastDistanceForward);

        if (hitBottom || hitTop)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsGround()
    {
        raycastDownPosition = new(transform.position.x, transform.position.y + rayPositionBottom, transform.position.z);

        return Physics.Raycast(raycastDownPosition, -transform.up, out _, raycastDistanceDown);
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
    /*
    public void PlayerSpeedTimescale()
    {
        
        if (ts.IsSlowed && isPlayerFaster == false)
        {
          
            runSpeed = runSpeed / ts.SlowDownFactor;
            slideSpeed = slideSpeed / ts.SlowDownFactor;

          
            isPlayerFaster = true;
        }
        if (!ts.IsSlowed)
        {
            slideSpeed = normalSlideSpeed;
         
            runSpeed = normalRunSpeed;
          
            isPlayerFaster = false;
        }

    }
    */
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(raycastDownPosition, -transform.up * raycastDistanceDown);
        Gizmos.DrawRay(raycastTopPosition, transform.forward * raycastDistanceForward);
        Gizmos.DrawRay(raycastBottomPosition, transform.forward * raycastDistanceForward);
    }
#endif
}