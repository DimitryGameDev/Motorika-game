using UnityEngine;

public class ProjectilePorcupine : MonoBehaviour
{
    private float timer;
    private NewPorcupine parent;

    private void Update()
    {
        RaycastHit hit;
        float stepLength = Time.deltaTime * parent.ProjectileSpeed;

        Debug.DrawRay(transform.position, transform.up * stepLength, Color.green);

        if (Physics.Raycast(transform.position, transform.up, out hit, stepLength))
        {
            OnHit(hit);
        }

        timer += Time.deltaTime;

        if (timer > parent.Lifetime)
        {
            //Debug.Log($"Projectile {gameObject.name} destroyed after {timer} seconds.");
            OnProjectileLifeEnd();
        }
    }

    protected virtual void OnHit(RaycastHit hit)
    {
        var destructible = hit.collider.transform.root.GetComponent<Destructible>();

        if (destructible != null && destructible != parent)
        {
            destructible.ApplyDamage(parent.ProjectileDamage);
            Destroy(gameObject);
            if (parent != null)
            {
                parent.NotifyProjectileDestroyed(this);
            }
        }
    }

    private void OnProjectileLifeEnd()
    {
        if (parent != null)
        {
            parent.NotifyProjectileDestroyed(this);
        }
        Destroy(gameObject);
    }

    public void SetParent(NewPorcupine parent)
    {
        this.parent = parent;
    }
}