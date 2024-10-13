using UnityEngine;

public class TurretSecond : MonoBehaviour
{
    [SerializeField] private AbilitiesChanger abilitiesChanger;
    [SerializeField] private TurretMode mode;
    public TurretMode Mode => mode;

    [SerializeField] private TurretProperties turretProperties;

    [SerializeField] private TurretProperties[] AllTurrets;

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

        PlayerInputController.Instance.SecondAbilityEvent += FireSecond;
    }

    private void OnDestroy()
    {
        PlayerInputController.Instance.SecondAbilityEvent -= FireSecond;
    }

    private void Update()
    {
        if (refireTimer > 0)
            refireTimer -= Time.deltaTime;

        //else if (Mode == TurretMode.Auto)
        //    Fire();
    }


    public void FireSecond()
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

        if (abilitiesChanger.PrevoiusSecondIndex != 3 || abilitiesChanger.PrevoiusSecondIndex != 4 || abilitiesChanger.PrevoiusSecondIndex != 5)
        {
            mode = TurretMode.Null;
            turretProperties = AllTurrets[3];
        }

        if (abilitiesChanger.PrevoiusSecondIndex == 3)
        {
            mode = TurretMode.Lightning;
            turretProperties = AllTurrets[0];
        }
        if (abilitiesChanger.PrevoiusSecondIndex == 4)
        {
            mode = TurretMode.Freezing;
            turretProperties = AllTurrets[1];
        }
        if (abilitiesChanger.PrevoiusSecondIndex == 5)
        {
            mode = TurretMode.AutoAiming;
            turretProperties = AllTurrets[2];
        }

        CreateLightningProjectille();
        CreateFreezingProjectille();
        CreateAimingProjectille();

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
        if (mode != TurretMode.Lightning) return;

        var projectile = Instantiate(turretProperties.ProjectilePrefab.gameObject).GetComponent<Projectile>();
        projectile.transform.position = transform.position;
        projectile.transform.forward = transform.forward;

        projectile.SetParentShooter(destructable);
        projectile.LightningProjectile();
    }

    private void CreateFreezingProjectille()
    {
        if (mode != TurretMode.Freezing) return;

        var projectile = Instantiate(turretProperties.ProjectilePrefab.gameObject).GetComponent<Projectile>();
        projectile.transform.position = transform.position;
        projectile.transform.forward = transform.forward;

        projectile.SetParentShooter(destructable);
        projectile.FreezingProjectile();
    }

    private void CreateAimingProjectille()
    {
        if (mode != TurretMode.AutoAiming) return;

        var projectile = Instantiate(turretProperties.ProjectilePrefab.gameObject).GetComponent<Projectile>();
        projectile.transform.position = transform.position;
        projectile.transform.forward = transform.forward;

        projectile.SetParentShooter(destructable);
        projectile.AimingProjectile();
    }
}