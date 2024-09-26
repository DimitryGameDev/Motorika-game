using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Projectile : MonoBehaviour
{
    // For projectile prefab
    [SerializeField] private float velocity;
    [SerializeField] private float lifetime;
    [SerializeField] protected int damage;
    [Header("For freeze projectile")]
    [SerializeField] private float freezeTime;
    [Header("For aiming projectile")]
    [SerializeField] private float findRange;

    [SerializeField] private ImpactEffect impactEffectPrefab;

    private float timer;
    private float freezeTimer;
    private Collider[] enemiesCollider;
    public void SetFromOtherProjectile(Projectile other)
    {
        other.GetData(out velocity, out lifetime, out damage, out impactEffectPrefab);
    }

    private void GetData(out float m_Velocity, out float m_Lifetime, out int m_Damage, out ImpactEffect m_ImpactEffectPrefab)
    {
        m_Velocity = this.velocity;
        m_Lifetime = this.lifetime;
        m_Damage = this.damage;
        m_ImpactEffectPrefab = this.impactEffectPrefab;
    }

    private void Update()
    {
        //Debug.Log(freezeTimer);

        LightningProjectile();
        FreezingProjectile();
        AimingProjectile();
    }

    public void LightningProjectile()
    {
        RaycastHit hit;

        float stepLength = Time.deltaTime * velocity;
        Vector3 step = transform.forward * stepLength;

        Debug.DrawRay(transform.position, transform.forward * stepLength, Color.green);
        if (Physics.Raycast(transform.position, transform.forward, out hit, stepLength))
        {
            OnHit(hit);
            OnProjectileLifeEnd(hit.collider, hit.point);
        }

        timer += Time.deltaTime;

        if (timer > lifetime)
            Destroy(gameObject);

        transform.position += step;
    }

    public void FreezingProjectile()
    {
        RaycastHit hit;

        float stepLength = Time.deltaTime * velocity;
        Vector3 step = transform.forward * stepLength;

        Debug.DrawRay(transform.position, transform.forward * stepLength, Color.green);
        if (Physics.Raycast(transform.position, transform.forward, out hit, stepLength))
        {
            freezeTimer = freezeTime;

            //OnHit(hit);
            OnFreeze(hit);
            OnProjectileLifeEnd(hit.collider, hit.point);
        }

        timer += Time.deltaTime;

        if (freezeTimer > 0)
        {
            freezeTimer -= Time.deltaTime;
        }
        else
            freezeTimer = 0;

        if (timer > lifetime)
            Destroy(gameObject);

        transform.position += step;
    }

    public void AimingProjectile()
    { 
        enemiesCollider = Physics.OverlapSphere(transform.position, findRange);
        float stepLength = Time.deltaTime * velocity;
        foreach (var enemyCollider in enemiesCollider)
        {
            var destructible = enemyCollider.GetComponent<Enemy>();

            if (destructible != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, destructible.transform.position, stepLength);

                /*
                Vector3 direction = destructible.transform.position - transform.position;
                float angle = Mathf.Atan2(direction.z, direction.y) * Mathf.Rad2Deg - 90f;
                Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, velocity * Time.deltaTime);
                */
                if (transform.position == destructible.transform.position)
                {
                    destructible.ApplyDamage(damage);
                }
            }
            timer += Time.deltaTime;

            if (timer > lifetime)
                Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, findRange);
    }

    protected virtual void OnHit(RaycastHit hit)
    {
        var destructible = hit.collider.transform.root.GetComponent<Destructible>();

        if (destructible != null && destructible != parent)
        {
            destructible.ApplyDamage(damage);

            // #Score
            if (Player.Instance != null && destructible.HitPoints < 0)
            {
                //  Player.Instance.AddScore(destructible.ScoreValue);
            }
        }
    }

    private void OnFreeze(RaycastHit hit)
    {
        var enemy = hit.collider.transform.root.GetComponent<Enemy>();
        var destructible = hit.collider.transform.root.GetComponent<Destructible>();

        if (enemy != null)
            enemy.SetZeroSpeed(freezeTimer);
        if (destructible != null)
            Debug.Log(destructible.HitPoints);
    }

    private void OnProjectileLifeEnd(Collider collider, Vector3 pos)
    {
        if (impactEffectPrefab != null)
        {
            var impact = Instantiate(impactEffectPrefab.gameObject);
            impact.transform.position = pos;
        }

        Destroy(gameObject);
    }

    private Destructible parent;

    public void SetParentShooter(Destructible parent)
    {
        this.parent = parent;
    }

    /* TODO для самонаводящейся турели
public void SetTarget(Destructible target)
{

}
*/
}