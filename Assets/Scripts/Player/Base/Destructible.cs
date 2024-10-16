using System;
using UnityEngine;
using UnityEngine.Events;

public class Destructible : MonoBehaviour
{
    public event UnityAction OnEnemyDeath;

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

        if (currentHitPoints > hitPoints)
            currentHitPoints = hitPoints;
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

    public virtual void OnDeath()
    {
       
        Destroy(gameObject);

        eventOnDeath?.Invoke();
        OnEnemyDeath?.Invoke();
    }

    
}