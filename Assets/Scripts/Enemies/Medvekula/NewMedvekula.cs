using System.Collections;
using UnityEngine;

public class NewMedvekula : Enemy
{
    private Turret turret;
    public Transform player;
   
    public float teleportDistance = 2f;
   
    public float teleportDuration = 1f;

    private float timer;
 
    private Animator animator;
    [SerializeField] private float lifetime;
    public float Lifetime => lifetime;
    [SerializeField] private float teleportTime;
    [SerializeField] private float minDistance;
    private bool isTeleporting;
    //[SerializeField] private float teleportAnimationDuration = 0.5f;

    private Collider[] colliders;

    private void Start()
    {
        timer = teleportTime;
        turret = GetComponentInChildren<Turret>();
        animator = GetComponentInChildren<Animator>();
      
    }

    private void Update()
    {
        PlayerPosition();

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (animator)
        {
            animator.SetTrigger("idleTrigger");
        }

        if (distanceToPlayer <= VisionDistance && distanceToPlayer > minDistance )
        {
            
            ShootAtPlayer();
        }

        if (!isTeleporting)
        {
            if (IsPlayerBehind())
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    
                    NextPositionCalculation();
                    animator.SetTrigger("jumpupTrigger");
                    StartCoroutine(FinalPositionCalculation());
                   
                    timer = teleportTime;
                }
            }
            else
            {
                timer = teleportTime;
            }
        }
    }

    private void PlayerPosition()
    {
        colliders = Physics.OverlapSphere(transform.position, 5);

        foreach (var collider in colliders)
        {
            var p = collider.transform.root.GetComponent<Player>();
            if (p != null)
            player = p.transform;
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

    private void NextPositionCalculation()
    {
        Vector3 targetPosition = player.position + new Vector3(0, 0, 1) * teleportDistance;

        targetPosition.y = transform.position.y-2f;
        targetPosition.x = transform.position.x;
        
        transform.position = targetPosition;
        
    }

    private IEnumerator FinalPositionCalculation()
    {
        var startPosition = transform.position;
        var endPosition = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);

        var duration = 1.0f; // Duration of the movement
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;
    }
}