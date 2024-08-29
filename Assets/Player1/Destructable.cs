using UnityEngine;
using UnityEngine.Events;

public class Destructable : MonoBehaviour
{
    [SerializeField] private UnityEvent m_EventOnDeath;
    public UnityEvent EventOnDeath => m_EventOnDeath;

    [SerializeField] private bool m_Indestructable;
    public bool IsIndestructable => m_Indestructable;

    [SerializeField] private int m_HitPoints;
    public int MaxHitPoints => m_HitPoints;

    private int m_CurrentHitPoints;
    public int HitPoints => m_CurrentHitPoints;

    private void Start()
    {
        m_CurrentHitPoints = m_HitPoints;
    }
    
    public void ApplyDamage(int damage)
    {
        if (m_Indestructable) return;

        m_CurrentHitPoints -= damage;

        if (m_CurrentHitPoints <= 0)
            OnDeath();
    }

    public void BlockDamage(float blockTime)
    {
        if ((blockTime += Time.time) >= Time.time)
        {
            m_Indestructable = true;
            m_CurrentHitPoints = m_HitPoints;
        }
    }
    public void GetDamage()
    {
        m_Indestructable = false;
    }

    public void OnDeath()
    {
        Destroy(gameObject);

        m_EventOnDeath?.Invoke();
    }
}