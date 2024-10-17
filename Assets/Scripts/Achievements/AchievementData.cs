using UnityEngine;
using System;
[CreateAssetMenu(fileName = "AchievementData", menuName = "Achievements/AchievementData", order = 1)]
public class AchievementData : ScriptableObject
{
    public string title;
    public string description;
    public Sprite image;
    public Predicate<object> requirement; // Сюда будем добавлять условие через код.

   // [HideInInspector]
    public bool achieved;
}
