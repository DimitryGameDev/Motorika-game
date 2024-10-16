using UnityEngine;

public class AddHP : MonoBehaviour
{
    [SerializeField] private int hitPoints;

    private void OnTriggerEnter(Collider other)
    {
        var destructible = other.transform.root.GetComponent<Destructible>();

        if (destructible != null)
        {
            destructible.ApplyDamage(-hitPoints);
            Destroy(gameObject);
        }
    }
}