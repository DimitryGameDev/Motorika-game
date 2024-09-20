using System;
using UnityEngine;
using UnityEngine.ProBuilder;

public class Turret : MonoBehaviour
{
    [SerializeField] private TurretMode mode;
    public TurretMode Mode => mode;



    [SerializeField] private TurretProperties turretProperties;

    private float refireTimer; // Cooldown
    public bool CanFire => refireTimer <= 0; // State of Colldown

    private Player player; // Parent object
    private Destructible destructable; // Parent object


    /*[SerializeField] private UpgradeAsset countTurretUpgrade;
    private int levelOfTurret;

    private void Awake()
    {
        if (countTurretUpgrade)
            levelOfTurret = Upgrades.GetUpgradeLevel(countTurretUpgrade);
    } */

    private void Start()
    {
        player = transform.root.GetComponent<Player>();
    }

    private void Update()
    {
        if (refireTimer > 0)
            refireTimer -= Time.deltaTime;

        //else if (Mode == TurretMode.Auto)
        //    Fire();
    }

    public void Fire()
    {
        if (refireTimer > 0)
            return;

        if (turretProperties == null)
            return;

        if (player)
        {
            if (!player.DrawEnergy(turretProperties.EnergyUsage))
                return;

            if (!player.DrawAmmo(turretProperties.AmmoUsage))
                return;
        }

        /* if (mode == TurretMode.AutoAiming)
         {
             Destructible nearestTarget = FindNearestTarget();
             if (nearestTarget != null)
             {
                 transform.forward = (nearestTarget.transform.position - transform.position).normalized;
                 Fire();
             }
         } */


        if (mode == TurretMode.Lightning)
        {
            CreateLightningProjectille();
        }

        if(mode == TurretMode.Freezing)
        {
            CreateFreezingProjectille();
        } 

        if(mode == TurretMode.AutoAiming)
        {
            CreateAimingProjectille();
        }

        refireTimer = turretProperties.RateOfFire;

        {
            // SFX
        }
    }

    public void AssignLoadout(TurretProperties props)
    {
        if (mode != props.Mode)
            return;

        turretProperties = props;
        refireTimer = 0;
    }

    private void CreateLightningProjectille()
    {
        var projectile = Instantiate(turretProperties.ProjectilePrefab.gameObject).GetComponent<Projectile>();
        projectile.transform.position = transform.position;
        projectile.transform.forward = transform.forward;

        projectile.SetParentShooter(destructable);
        projectile.LightningProjectile();
    }

    private void CreateFreezingProjectille()
    {
        var projectile = Instantiate(turretProperties.ProjectilePrefab.gameObject).GetComponent<Projectile>();
        projectile.transform.position = transform.position;
        projectile.transform.forward = transform.forward;

        projectile.SetParentShooter(destructable);
        projectile.FreezingProjectile();
    }

    private void CreateAimingProjectille()
    {
        var projectile = Instantiate(turretProperties.ProjectilePrefab.gameObject).GetComponent<Projectile>();
        projectile.transform.position = transform.position;
        projectile.transform.forward = transform.forward;

        projectile.SetParentShooter(destructable);
        projectile.AimingProjectile();
    }
    /*
        private Destructible FindNearestTarget()
        {
            // Поиск ближайшего врага с компонентом Destructible
            Collider[] colliders = Physics.OverlapSphere(transform.position, 15f);
            Destructible nearestTarget = null;
            float nearestDistance = 30;

            foreach (var collider in colliders)
            {
                Destructible destructible = collider.transform.root.GetComponent<Destructible>();
                if (destructible != null && destructible != player && destructible != this.destructable)
                {
                    float distance = Vector3.Distance(transform.position, destructible.transform.position);
                    if (distance < nearestDistance)
                    {
                        nearestTarget = destructible;
                        nearestDistance = distance;
                    }
                }
            }

            return nearestTarget;
        }*/
}

/*
 using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private TurretMode mode;
    public TurretMode Mode => mode;

    [SerializeField] private TurretProperties turretProperties;

    private float refireTimer; // Cooldown
    public bool CanFire => refireTimer <= 0; // State of Colldown

    private Player player; // Parent object
    private Destructible destructable; // Parent object

    /*[SerializeField] private UpgradeAsset countTurretUpgrade;
    private int levelOfTurret;

    private void Awake()
    {
        if (countTurretUpgrade)
            levelOfTurret = Upgrades.GetUpgradeLevel(countTurretUpgrade);
    } 

private void Start()
{
    player = transform.root.GetComponent<Player>();
}

private void Update()
{
    if (refireTimer > 0)
        refireTimer -= Time.deltaTime;

    //else if (Mode == TurretMode.Auto)
    //    Fire();
}

public void Fire()
{
    if (refireTimer > 0)
        return;

    if (turretProperties == null)
        return;

    if (player)
    {
        if (!player.DrawEnergy(turretProperties.EnergyUsage))
            return;

        if (!player.DrawAmmo(turretProperties.AmmoUsage))
            return;
    }

    if (mode == TurretMode.AutoAiming)
    {
        Destructible nearestTarget = FindNearestTarget();
        if (nearestTarget != null)
        {
            transform.forward = (nearestTarget.transform.position - transform.position).normalized;
            Fire();
        }
    }

    CreateProjectille();

    refireTimer = turretProperties.RateOfFire;

    {
        // SFX
    }
}

public void AssignLoadout(TurretProperties props)
{
    if (mode != props.Mode)
        return;

    turretProperties = props;
    refireTimer = 0;
}

private void CreateProjectille()
{
    var projectile = Instantiate(turretProperties.ProjectilePrefab.gameObject).GetComponent<Projectile>();
    projectile.transform.position = transform.position;
    projectile.transform.forward = transform.forward;

    projectile.SetParentShooter(destructable);
}

private Destructible FindNearestTarget()
{
    // Поиск ближайшего врага с компонентом Destructible
    Collider[] colliders = Physics.OverlapSphere(transform.position, 15f);
    Destructible nearestTarget = null;
    float nearestDistance = 30;

    foreach (var collider in colliders)
    {
        Destructible destructible = collider.transform.root.GetComponent<Destructible>();
        if (destructible != null && destructible != player && destructible != this.destructable)
        {
            float distance = Vector3.Distance(transform.position, destructible.transform.position);
            if (distance < nearestDistance)
            {
                nearestTarget = destructible;
                nearestDistance = distance;
            }
        }
    }

    return nearestTarget;
}
}



private void FireFreezeProjectile()
{
    var projectile = Instantiate(turretProperties.FreezeProjectilePrefab.gameObject).GetComponent<FreezeProjectile>();
    projectile.transform.position = transform.position;
    projectile.transform.forward = transform.forward;
    projectile.SetParentShooter(destructable);
}




public class FreezeProjectile : Projectile
{
    [SerializeField] private float freezeDuration = 3f;
    [SerializeField] private float freezeSpeedReduction = 0.5f;

    protected override void OnHit(RaycastHit hit)
    {
        base.OnHit(hit);

        Destructible destructible = hit.collider.transform.root.GetComponent<Destructible>();
        if (destructible != null && destructible != parent)
        {
            StartCoroutine(FreezeTarget(destructible));
        }
    }

    private IEnumerator FreezeTarget(Destructible target)
    {
        float originalSpeed = target.MoveSpeed;
        target.MoveSpeed *= freezeSpeedReduction;

        yield return new WaitForSeconds(freezeDuration);

        target.MoveSpeed = originalSpeed;
    }
}


private void FireAutoAimProjectile()
{
    Destructible nearestTarget = FindNearestTarget();
    if (nearestTarget != null)
    {
        transform.forward = (nearestTarget.transform.position - transform.position).normalized;
        CreateProjectille();
    }
}

Логику для TurretMode.Lightning и TurretMode.Freezing нужно реализовывать в классе Projectile, а логику для TurretMode.AutoAiming нужно реализовывать в классе Turret.
 */