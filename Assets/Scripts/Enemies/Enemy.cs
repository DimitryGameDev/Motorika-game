using UnityEngine;

public class Enemy : Destructible
{
    [SerializeField] private float moveSpeed;
    public float MoveSpeed => moveSpeed; 

    [SerializeField] private float projectileSpeed;
    public float ProjectileSpeed => projectileSpeed; 

    [SerializeField] private float visionDistance;
    public float VisionDistance => visionDistance;

    [SerializeField] private float knockbackForce;
    public float KnockbackForce => knockbackForce; 
}
