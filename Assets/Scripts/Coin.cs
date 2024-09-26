using UnityEngine;

public class Coin : Pikup
{
    [SerializeField] private GameObject impactEffect;

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        Bag bag = other.GetComponent<Bag>();

        if (bag != null)
        {
            bag.AddCoin(1);
            
            if(impactEffect != null)
            Instantiate(impactEffect);

            Destroy(gameObject);
        }
    }
}
