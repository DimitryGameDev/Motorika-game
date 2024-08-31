using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private TurretMode mode;
    public TurretMode Mode => mode;

    [SerializeField] private TurretProperties turretProperties;

    private float refireTimer; // Cooldown
    public bool CanFire => refireTimer <= 0; // State of Colldown

    private Player player; // Parent object
    private Destructable destructable; // Parent object

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
}