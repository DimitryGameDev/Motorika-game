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

    [SerializeField] private ImpactEffect impactEffectPrefab;

    private float timer;

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