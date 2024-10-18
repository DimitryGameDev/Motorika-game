using UnityEngine;

public class Player : MonoSingleton<Player>
{
    [Header("Run/Slide")] [SerializeField] private float runSpeed = 5f; //run velocity
    [SerializeField] private float slideSpeed = 10f; // slide speed
    [SerializeField] private float slideControlTime = 0.6f; // max slide time
    [Header("Dash")] [SerializeField] private float dashForce = 10f;
    [Header("Jump")] [SerializeField] private float maxJumpForce; // additional max jump force
    [SerializeField] private float chargeRate; // max jump time

    [Header("Raycast")] [SerializeField]
    private float raycastDistanceForward = 1.5f; // Raycast distance from player to value;

    [SerializeField] private float raycastDistanceDown = 1.5f; // Raycast distance from player to value;
    [SerializeField] private float rayPositionTop = 0.5f; // Start position Ray on Top 
    [SerializeField] private float rayPositionBottom = 0.5f; // Start position Ray on Bottom 
    [Header("Collider")] [SerializeField] private Collider mainCollider;
    [SerializeField] private Collider slideCollider;

    [SerializeField] private AbilitiesChanger abilitiesChanger;

    [SerializeField] private GameObject DashSFX;

    private Parry parry;

    private int dashLevel;

    //private float currentJumpForce = 0f;
    private float slideTime = 0;

    private Rigidbody rb;
    private Animator animator;
    private Vector3 raycastDownPosition;
    private Vector3 raycastTopPosition;
    private Vector3 raycastMiddlePosition1;
    private Vector3 raycastMiddlePosition2;
    private Vector3 raycastBottomPosition;

    public bool isSlide;
    public bool isJump;
    
    private void Start()
    {
        dashLevel = PlayerPrefs.GetInt("Ability2");

        parry = GetComponent<Parry>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

        PlayerInputController.Instance.RunEvent += Run;
        PlayerInputController.Instance.FirstAbilityEvent += DashFirst;
        PlayerInputController.Instance.SecondAbilityEvent += DashSecond;
        PlayerInputController.Instance.JumpEvent += Jump;
        PlayerInputController.Instance.SlideEvent += Slide;
    }

    private void OnDestroy()
    {
        PlayerInputController.Instance.FirstAbilityEvent -= DashFirst;
        PlayerInputController.Instance.SecondAbilityEvent -= DashSecond;
        PlayerInputController.Instance.RunEvent -= Run;
        PlayerInputController.Instance.JumpEvent -= Jump;
        PlayerInputController.Instance.SlideEvent -= Slide;
    }

    private void Update()
    {
        rb.freezeRotation = true;
        transform.up = Vector3.up;
        transform.position = new Vector3(0, transform.position.y, transform.position.z);

        if (!isSlide)
        {
            mainCollider.enabled = true;
            slideCollider.enabled = false;
        }
    }

    private void Run()
    {
        isSlide = false;

        if (!IsBarrier())
        {
            transform.Translate(transform.forward * runSpeed * Time.deltaTime);

            if (IsGround())
            {
                animator.SetTrigger("Run");
            }
        }
        else if (IsGround() && !isSlide && !isJump)
            animator.SetTrigger("Idle");
    }

    private void Slide()
    {
        if (IsGround() && !parry.IsParry)
        {
            isSlide = true;
            animator.SetTrigger("Slide");

            slideTime = 0;

            mainCollider.enabled = false;
            slideCollider.enabled = true;

            slideTime += Time.deltaTime;

            if (slideTime < slideControlTime)
            {
                rb.velocity = new Vector3(0, rb.velocity.y, slideSpeed);
            }
        }
    }

    private void DashFirst()
    {
        if (IsGround()) return;
        if (abilitiesChanger.PreviousFirstIndex == 2)
        {
            rb.AddForce(transform.forward * (dashForce * dashLevel), ForceMode.Impulse);
            if (DashSFX != null)
            {
                GameObject sfx = Instantiate(DashSFX, transform.position, Quaternion.identity);
            }
        }
    }

    private void DashSecond()
    {
        if (IsGround()) return;
        if (abilitiesChanger.PrevoiusSecondIndex == 2)
        {
            rb.AddForce(transform.forward * (dashForce * dashLevel), ForceMode.Impulse);
            if (DashSFX != null)
            {
                GameObject sfx = Instantiate(DashSFX, transform.position, Quaternion.identity);
            }
        }
    }

    private void Jump()
    {
        if (IsGround())
        {
            isJump = true;

            animator.SetTrigger("Jump");

            rb.AddForce(Vector3.up * maxJumpForce * Time.deltaTime, ForceMode.Impulse);
            
            /* currentJumpForce = 0f;

             if (currentJumpForce < maxJumpForce)
             {
                 currentJumpForce += Time.deltaTime * chargeRate;
                 rb.AddForce(Vector3.up * currentJumpForce, ForceMode.Impulse);
             }*/
        }
        else
            isJump = false;
    }

    public void Parry(float parryForce)
    {
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);
        rb.AddForce(-transform.forward * parryForce, ForceMode.Impulse);
    }

    public bool IsBarrier()
    {
        raycastTopPosition = new(transform.position.x, transform.position.y + rayPositionTop, transform.position.z);
        raycastMiddlePosition1 = new(transform.position.x, transform.position.y + rayPositionTop * 0.7f,
            transform.position.z);
        raycastMiddlePosition2 = new(transform.position.x, transform.position.y + rayPositionTop * 0.3f,
            transform.position.z);
        raycastBottomPosition =
            new(transform.position.x, transform.position.y + rayPositionBottom, transform.position.z);
        bool hitBottom = Physics.Raycast(raycastBottomPosition, transform.forward, out _, raycastDistanceForward);
        bool hitMiddle1 = Physics.Raycast(raycastMiddlePosition1, transform.forward, out _, raycastDistanceForward);
        bool hitMiddle2 = Physics.Raycast(raycastMiddlePosition2, transform.forward, out _, raycastDistanceForward);
        bool hitTop = Physics.Raycast(raycastTopPosition, transform.forward, out _, raycastDistanceForward);

        if (hitBottom || hitMiddle1 || hitMiddle2 || hitTop)
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

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        //Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y + transform.localScale.y + 0.3f, transform.position.z + transform.localScale.z - 0.3f), new Vector3(2f, 2f, 0.5f));
        Gizmos.DrawRay(raycastDownPosition, -transform.up * raycastDistanceDown);
        Gizmos.DrawRay(raycastTopPosition, transform.forward * raycastDistanceForward);
        Gizmos.DrawRay(raycastMiddlePosition1, transform.forward * raycastDistanceForward);
        Gizmos.DrawRay(raycastMiddlePosition2, transform.forward * raycastDistanceForward);
        Gizmos.DrawRay(raycastBottomPosition, transform.forward * raycastDistanceForward);
    }
#endif
}