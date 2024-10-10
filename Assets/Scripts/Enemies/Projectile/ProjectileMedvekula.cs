using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMedvekula : MonoBehaviour
{
    private float timer;
    [SerializeField] private NewMedvekula parent;

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
            Destroy(gameObject);
        }
    }

    protected virtual void OnHit(RaycastHit hit)
    {
        var destructible = hit.collider.transform.root.GetComponent<Destructible>();

        if (destructible != null && destructible != parent)
        {
            destructible.ApplyDamage(parent.ProjectileDamage);
            Destroy(gameObject);
         
        }
    }

 

    public void SetParent(NewMedvekula parent)
    {
        this.parent = parent;
    }
}
