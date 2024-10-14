using UnityEngine;

public class AnomaliesSpawner : MonoBehaviour
{
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private GameObject anomalyPrefab;

    private Destructible destructable;

    private void Start()
    {
        destructable = GetComponent<Destructible>();

        if (destructable != null)
            destructable.OnEnemyDeath += SpawnAnomalies;
    }

    private void OnDestroy()
    {
        destructable.OnEnemyDeath -= SpawnAnomalies;
    }

    private void SpawnAnomalies()
    {
        if (anomalyPrefab != null)
            Instantiate(anomalyPrefab, transform.position, Quaternion.identity);
    }
}