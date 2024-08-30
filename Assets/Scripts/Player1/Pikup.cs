using UnityEngine;

public class Pikup : MonoBehaviour
{
    protected virtual void OnTriggerEnter(Collider other)
    {
        Player player = GetComponent<Player>();

        if(player != null)
        {
            Destroy(gameObject);
        }
    }
}
