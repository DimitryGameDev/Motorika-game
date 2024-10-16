using System;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoSingleton<AchievementManager>
{
    public static List<Achievement> achievements;

    [HideInInspector] public int NumKILLS = 0;
    [HideInInspector] public float NumDEATHS = 0;

    public void OnEnemyDeath()
    {
        NumKILLS ++;
    }

    public void OnPlayerDeath()
    {
        NumDEATHS ++;
    }
    public bool AchievementUnlocked(string achievementName)
    {
        bool result = false;

        if (achievements == null)
            return false;

        Achievement[] achievementsArray = achievements.ToArray();
        Achievement a = Array.Find(achievementsArray, ach => achievementName == ach.title);

        if (a == null)
            return false;

        result = a.achieved;

        return result;
    }

    private void Start()
    { 
        InitializeAchievements();
    }

    private void InitializeAchievements() 
    {
        if (achievements != null)
            return;

        achievements = new List<Achievement>();
        achievements.Add(new Achievement("PLEASURE TO KILL", "KILL FOR FIRST TIME", (object o) => NumKILLS >= 1));
        achievements.Add(new Achievement("Oh, Shit!", "DIE FOR FIRST TIME", (object o) => NumDEATHS >= 1));
    }

    private void Update()
    {
        CheckAchievementCompletion();
    }

    private void CheckAchievementCompletion()
    {
        if (achievements == null)
            return;

        foreach (var achievement in achievements)
        {
            achievement.UpdateCompletion();
        }
    }
}

public class Achievement
{
    public Achievement(string title, string description, Predicate<object> requirement)
    {
        this.title = title;
        this.description = description;
        this.requirement = requirement;
    }

    public string title;
    public string description;
    public Predicate<object> requirement;

    public bool achieved;

    public void UpdateCompletion()
    {
        if (achieved)
            return;

        if (RequirementsMet())
        {
            Debug.Log($"{title}: {description}");
            achieved = true;
        }
    }

    public bool RequirementsMet()
    {
        return requirement.Invoke(null);
    }
}