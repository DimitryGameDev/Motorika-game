using UnityEngine;

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
    private const string Null = " ";
    private const string Time = "�����-������";
    private const string Dash = "���������";
    private const string ProjectileBase = "��������";
    private const string ProjectileFrize = "����-�������";
    private const string ProjectileAim = "����������";

    [SerializeField] private AbilityName abilityName;
    [SerializeField] private Sprite icon;
    private string description;

    public AbilityName AbilityName => abilityName;
    public Sprite Icon => icon;
    public string Description => description;

    public string SetText(int index)
    {
        if (index == 0) description = Null;
        if (index == 1) description = Time;
        if (index == 2) description = Dash;
        if (index == 3) description = ProjectileBase;
        if (index == 4) description = ProjectileFrize;
        if (index == 5) description = ProjectileAim;

        return description;

        /*
        switch (abilityName)
        {
            case AbilityName.Null:
                description = Null;
                return;
            case AbilityName.Time:
                description = Time;
                return;
            case AbilityName.Dash:
                description = Dash;
                return;
            case AbilityName.ProjectileBase:
                description = ProjectileBase;
                return;
            case AbilityName.ProjectileFrize:
                description = ProjectileFrize;
                return;
            case AbilityName.ProjectileAim:
                description = ProjectileAim;
                return;
            default:
                description = Null;
                return;
        }*/
    }
}