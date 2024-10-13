using UnityEngine;
using UnityEngine.Serialization;

public class ImpactEffect : MonoBehaviour
{
   [SerializeField] private float lifetime;

    private float timer;

    private void Update()
    {
        if (timer < lifetime)
            timer += Time.deltaTime;
        else
            Destroy(gameObject);
    }
}