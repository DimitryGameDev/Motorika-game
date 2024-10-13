using System.Collections.Generic;
using UnityEngine;

public class NewPorcupine : Enemy
{
    [SerializeField] private int projectileCount;
    [SerializeField] private float projectileSpawmAngle;
    [SerializeField] private float projectileAngleOffset;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] public float rateOfFire;
    [SerializeField] private float lifetime;
    public float Lifetime => lifetime;

    private List<ProjectilePorcupine> projectiles = new List<ProjectilePorcupine>();
    private Animator animator;

    private float refireTimer;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        Parry.Instance.AttackAnimEvent += AttackAnimation;
        Parry.Instance.IdleAnimEvent += IdleAnimation;
    }
    private void OnDestroy()
    {
        Parry.Instance.AttackAnimEvent -= AttackAnimation;
        Parry.Instance.IdleAnimEvent -= IdleAnimation;
    }

    private void Update()
    {
        if (refireTimer > 0)
            refireTimer -= Time.deltaTime;

        CreatePorcupineProjectille();
        MoveProjectiles();
    }

    private void CreatePorcupineProjectille()
    {
        if (refireTimer > 0)
           return;

        refireTimer = rateOfFire;

        for (int i = 0; i < projectileCount; i++)
        {
            GameObject newProjectileObj = Instantiate(projectilePrefab, transform);
            ProjectilePorcupine newProjectile = newProjectileObj.GetComponent<ProjectilePorcupine>();

            newProjectile.transform.position = transform.position;

            Quaternion target = Quaternion.Euler(-projectileAngleOffset + (projectileSpawmAngle / projectileCount) * i, 0, 0);
            newProjectile.transform.rotation = target;

            newProjectile.SetParent(this); 
            projectiles.Add(newProjectile);
        }
    }

    private void MoveProjectiles()
    {
        for (int i =0; i <  projectiles.Count; i++)
        {
            projectiles[i].transform.position += projectiles[i].transform.up * ProjectileSpeed * Time.deltaTime;
        }
    }

    public void NotifyProjectileDestroyed(ProjectilePorcupine projectile)
    {
        projectiles.Remove(projectile);
    }

    private void AttackAnimation()
    {
        if (animator != null)
            animator.SetTrigger("Attack");
    }
    private void IdleAnimation()
    {
        if (animator != null)
            animator.SetTrigger("Walk");
    }
}