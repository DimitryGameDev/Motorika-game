using UnityEngine;

public class DamageArea : MonoBehaviour
{
    [SerializeField] private int damage;

    private void OnTriggerEnter(Collider other)
    {
        var destructible = GetComponent<Destructible>();

        if (destructible != null)
            destructible.ApplyDamage(damage);
    }
}
