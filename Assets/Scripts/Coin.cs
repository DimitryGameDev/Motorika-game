using UnityEngine;

public enum Name
{
    Coin,
    Anomaly
}

public class Coin : Pikup
{
    [SerializeField] private GameObject impactEffect;

    [SerializeField] private Name coinName;
    [SerializeField] private int coinCount;
    [SerializeField] private int anomalyCount;

    [Header("Anomalies")]
    [SerializeField] private float radius;
    [SerializeField] private float speed;

    private Player player;
    private Collider[] colliders;

    private void Update()
    {
        SearchTarget();

        if (coinName == Name.Anomaly && player != null)
            Movement(player.transform);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        Bag bag = other.GetComponent<Bag>();

        if (bag != null)
        {
            bag.AddCoin(coinCount);
            bag.AddAnomalies(anomalyCount);

            if (impactEffect != null)
                Instantiate(impactEffect, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }

    private void SearchTarget()
    {
        colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (var collider in colliders)
        {
            if (collider == null) return;

            if (collider.TryGetComponent(out Player detectedPlayer))
            {
                player = detectedPlayer;
                return;
            }
        }

        player = null;
    }

    private void Movement(Transform target)
    {
        transform.position = Vector3.MoveTowards(transform.position, 
            new Vector3(target.position.x, target.position.y + 0.5f, target.position.z), speed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}