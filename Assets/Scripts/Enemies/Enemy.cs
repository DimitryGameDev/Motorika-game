using UnityEngine;

public enum Parried
{
    On,
    Off
}

public enum Sound
{
    Wolf,
    Ezh,
    Gnom
}

public class Enemy : Destructible
{
    [SerializeField] private GameObject deathSFX;
    [SerializeField] private Sound typeSound;
    public Sound TypeSound => typeSound; 
    
    [SerializeField] private GameObject parrySphere;
    private GameObject spawnSphere;

    [SerializeField] private int damage;
    public int Damage => damage;

    [SerializeField] private Parried parried;
    public Parried Parried => parried;

    [SerializeField] private float moveSpeed;
    public float MoveSpeed => moveSpeed;

    [SerializeField] private float projectileSpeed;
    public float ProjectileSpeed => projectileSpeed;

    [SerializeField] private int projectileDamage;
    public int ProjectileDamage => projectileDamage;

    [SerializeField] private float visionDistance;
    public float VisionDistance => visionDistance;
    
    [SerializeField] private float knockbackForce;
    public float KnockbackForce => knockbackForce;

    public void SetZeroSpeed(float timer)
    {
        var move = moveSpeed;
        var projectile = projectileSpeed;

        if (timer > 0)
        {
            moveSpeed = 0;
            projectileSpeed = 0;
        }
        else
        {
            moveSpeed = move;
            projectileSpeed = projectile;
        }
    }

    public void ParrySphere(bool state)
    {
        if (parrySphere == null) return;

        if (state)
           spawnSphere  = Instantiate(parrySphere, transform);
        else
            Destroy(spawnSphere);
    }

    public override void OnDeath()
    {
        base.OnDeath();

        if (deathSFX != null)
        {
            GameObject deathImpact = Instantiate(deathSFX, transform.position, Quaternion.identity);
        }

        AchievementManager.Instance.OnEnemyDeath();
    }
}