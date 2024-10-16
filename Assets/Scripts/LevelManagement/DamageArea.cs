using UnityEngine;

public class DamageArea : MonoBehaviour
{
    [SerializeField] private int damage;

    private void OnTriggerStay(Collider other)
    {
        var destructible = other.transform.root.GetComponent<Destructible>();

        if (destructible != null)
            destructible.ApplyDamage(damage);
    }
}