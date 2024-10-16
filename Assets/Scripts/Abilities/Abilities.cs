using UnityEngine;
using UnityEngine.UI;

public enum AbilityName
{
    Null,
    Time,
    Dash,
    ProjectileBase,
    ProjectileFrize,
    ProjectileAim
}

[CreateAssetMenu]
public class Abilities : ScriptableObject
{
    [SerializeField] private AbilityName abilityName;
    [SerializeField] private Sprite icon;
    [SerializeField] private string description;
    public AbilityName AbilityName => abilityName;
    public Sprite Icon => icon;
    public string Description => description;
}