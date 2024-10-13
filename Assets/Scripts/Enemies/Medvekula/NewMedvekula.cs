using System.Collections;
using UnityEngine;

public class NewMedvekula : Enemy
{
    private Turret turret;
    public Transform player;
    public float shootingInterval = 2f;
    public float teleportDistance = 2f;
    public float shootingRange = 10f;
    public float teleportDuration = 1f;
    private float nextShootTime;
    private float timer;
    private Collider collider;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private Animator animator;
    [SerializeField] private float lifetime;
    public float Lifetime => lifetime;
    [SerializeField] private float teleportTime;

    private bool isTeleporting;
    [SerializeField] private float teleportAnimationDuration = 0.5f;

    private void Start()
    {
        timer = teleportTime;
        turret = GetComponentInChildren<Turret>();
        animator = GetComponentInChildren<Animator>();
        collider = GetComponent<Collider>(); 
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (animator)
        {
            animator.SetTrigger("idleTrigger");
        }

        ShootAtPlayer();

        if (!isTeleporting)
        {
            if (IsPlayerBehind())
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    
                    StartCoroutine(TeleportCoroutine());
                    skinnedMeshRenderer.enabled = false;
                    collider.enabled = false;
                    timer = teleportTime;
                }
            }
            else
            {
                timer = teleportTime;
            }
        }
    }

    private void ShootAtPlayer()
    {
        if (turret != null)
        {
            turret.EnemyFire();
        }
    }

    private bool IsPlayerBehind()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        
        return directionToPlayer.z > 2;
      
    }

    private IEnumerator TeleportCoroutine()
    {
        isTeleporting = true;

        // Анимация перед телепортацией
      // animator.SetTrigger("diveTrigger");

        // Ждем время, равное длительности анимации телепортации
      //  yield return new WaitForSeconds(teleportAnimationDuration);

        // Телепортируем объект
        NextPositionCalculation();

        // Ждем короткое время для завершения телепортации
      

        // Анимация после телепортации
      
        yield return new WaitForSeconds(teleportAnimationDuration);
        collider.enabled = true;
        skinnedMeshRenderer.enabled = true;
        animator.SetTrigger("jumpupTrigger");
      
        // Задержка, чтобы дождаться окончания анимации
        
       
        isTeleporting = false;
    }

    private void NextPositionCalculation()
    {
        Vector3 targetPosition = player.position + new Vector3(0, 0, 1) * teleportDistance;

        targetPosition.y = transform.position.y;
        targetPosition.x = transform.position.x;
        
        transform.position = targetPosition;
        
    }
}