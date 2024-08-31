using UnityEngine;

public class ImpactEffect : MonoBehaviour
{
    [SerializeField] private float lfetime;

    private float timer;

    private void Update()
    {
        if (timer < lfetime)
            timer += Time.deltaTime;
        else
            Destroy(gameObject);
    }
}