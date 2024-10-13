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

    private void Update()
    {
        LightningProjectile();
        FreezingProjectile();
        AimingProjectile();
        EnemyProjectile();
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
    public void EnemyProjectile()
    {
        RaycastHit hit;

        float stepLength = Time.deltaTime * velocity;
        Vector3 step = transform.forward * stepLength;

        Debug.DrawRay(transform.position, transform.forward * stepLength, Color.green);
        if (Physics.Raycast(transform.position, transform.forward, out hit, stepLength))
        {
            OnHitEnemy(hit);
           
        }

        timer += Time.deltaTime;

        if (timer > lifetime)
            Destroy(gameObject);

        transform.position += step;
    }
    public void FreezingProjectile()
    {
        RaycastHit hit1;

        float stepLength = Time.deltaTime * velocity;
        Vector3 step = transform.forward * stepLength;

        Debug.DrawRay(transform.position, transform.forward * stepLength, Color.green);
        if (Physics.Raycast(transform.position, transform.forward, out hit1, stepLength))
        {
            freezeTimer = freezeTime;

            OnFreeze(hit1);
            OnProjectileLifeEnd(hit1.collider, hit1.point);
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
            var player = enemyCollider.GetComponent<Player>();

            if (player == null)
            {
                var destructible = enemyCollider.GetComponent<Destructible>();

                if (destructible != null)
                {
                    Vector3 direction = (destructible.transform.position - transform.position).normalized;

                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, stepLength * 2);
                    transform.position = Vector3.MoveTowards(transform.position, destructible.transform.position, stepLength);

                    if (Vector3.Distance(transform.position, destructible.transform.position) < 0.1f)
                    {
                        destructible.ApplyDamage(damage);
                        Destroy(gameObject);
                        return;
                    }
                }

                timer += Time.deltaTime;

                if (timer > lifetime)
                    Destroy(gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, findRange);
    }

    private void OnHitEnemy(RaycastHit hit)
    {
        var player = hit.collider.transform.root.GetComponent<Player>();
        if (player)
        {
            var destructible = hit.collider.GetComponent<Destructible>();
            if (destructible != null)
            {
                destructible.ApplyDamage(damage);
                Destroy(gameObject);
            }
         
        }
    }
    protected virtual void OnHit(RaycastHit hit)
    {
        if (hit.collider == null) return;
        Debug.Log(hit.collider.name);
        var destructible = hit.collider.transform.root.GetComponent<Destructible>();
        var destructible1 = hit.collider.transform.GetComponent<Destructible>();
        
        if ((destructible != null && destructible != parent) ||
        (destructible1 != null && destructible1 != parent))
        {
            if (destructible1 != null)
            {
                destructible1.ApplyDamage(damage);
                return;
            }
            if (destructible != null)
            {
                destructible.ApplyDamage(damage);
              
            }
           

            // #Score
            if (Player.Instance != null)
            {
                if (destructible != null && destructible.HitPoints < 0)
                {
                    //Player.Instance.AddScore(destructible.ScoreValue);
                }
                if (destructible1 != null && destructible1.HitPoints < 0)
                {
                    //Player.Instance.AddScore(destructible1.ScoreValue);
                }
            }
        }
    }

    private void OnFreeze(RaycastHit hit)
    {
        var enemy = hit.collider.transform.GetComponent<Enemy>();

        if (enemy != null)
            enemy.SetZeroSpeed(freezeTimer);
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
}