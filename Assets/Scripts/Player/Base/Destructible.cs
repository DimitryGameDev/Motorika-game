using UnityEngine;
using UnityEngine.Events;

public class Destructible : MonoBehaviour
{
    [SerializeField] private UnityEvent eventOnDeath;
    public UnityEvent EventOnDeath => eventOnDeath;

    [SerializeField] private bool indestructable;
    public bool IsIndestructable => indestructable;

    [SerializeField] private int hitPoints;
    public int MaxHitPoints => hitPoints;

    private int currentHitPoints;
    public int HitPoints => currentHitPoints;

    private void Awake()
    {
        currentHitPoints = hitPoints;
    }
    
    public void ApplyDamage(int damage)
    {
        if (indestructable) return;

        currentHitPoints -= damage;

        if (currentHitPoints <= 0)
            OnDeath();
    }

    public void BlockDamage(float blockTime)
    {
        if ((blockTime += Time.time) >= Time.time)
        {
            indestructable = true;
            currentHitPoints = hitPoints;
        }
    }
    public void GetDamage()
    {
        indestructable = false;
    }

    public void OnDeath()
    {
        Destroy(gameObject);

        eventOnDeath?.Invoke();
    }
}