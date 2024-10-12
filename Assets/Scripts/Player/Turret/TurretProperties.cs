using UnityEngine;

public enum TurretMode
{
    Lightning,
    Freezing,
    AutoAiming,
    Null
}

[CreateAssetMenu]
public sealed class TurretProperties : ScriptableObject
{
    [SerializeField] private TurretMode mode;
    public TurretMode Mode => mode;

    [SerializeField] private Projectile projectilePrefab; 
    public Projectile ProjectilePrefab => projectilePrefab;

    [SerializeField] public float rateOfFire;
    public float RateOfFire => rateOfFire;

    [SerializeField] private int energyUsage; // Energy amount per shot
    public int EnergyUsage => energyUsage;

    [SerializeField] private int ammoUsage; // Ammo amount per shot
    public int AmmoUsage => ammoUsage;

    [SerializeField] private AudioClip launchSFX; // Sound
    public AudioClip LaunchSFX => launchSFX;
}