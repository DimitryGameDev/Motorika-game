using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DestroySound : MonoBehaviour
{
    [SerializeField] private AudioSource SFX;

    private float timer = 5;
    private void Start()
    {
        SFX.Play();
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        
        if(timer<= 0)
            Destroy(gameObject);
    }


    private void BittleDeathSFX()
    {
        SFX.Play();
    }
}