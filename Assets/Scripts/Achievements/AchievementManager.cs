using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Импортируем TextMeshPro
using UnityEngine.UI; // Импортируем UnityEngine.UI для работы с Image

public class AchievementManager : MonoSingleton<AchievementManager>
{
    public List<AchievementData> achievementsData; // Список данных о достижениях (назначать в инспекторе)

    [HideInInspector] public int NumKILLS = 0;
    [HideInInspector] public float NumDEATHS = 0;

    public GameObject achievementPanel; // Панель для отображения достижений
    public TMP_Text achievementTitleText; // TextMeshPro для отображения заголовка достижения
    public TMP_Text achievementDescriptionText; // TextMeshPro для отображения описания достижения
    public Image achievementImage; // Image для отображения изображения достижения

    public void OnEnemyDeath()
    {
        NumKILLS++;
    }

    public void OnPlayerDeath()
    {
        NumDEATHS++;
    }

    public bool AchievementUnlocked(string achievementName)
    {
        if (achievementsData == null)
            return false;

        AchievementData a = achievementsData.Find(ach => achievementName == ach.title);

        return a != null && a.achieved;
    }

    private void Start()
    {
        InitializeAchievements();

        // Скрываем панель достижений при старте
        if (achievementPanel != null)
        {
            achievementPanel.SetActive(false);
        }
    }

    private void InitializeAchievements()
    {
        if (achievementsData == null)
            return;

        foreach (var achievement in achievementsData)
        {
            // Здесь назначать условия для каждого достижения, например:
            // Вы можете расширить ScriptableObject для условий и заполнить их в редакторе
            if (achievement.title == "PLEASURE TO KILL")
            {
                achievement.requirement = o => NumKILLS >= 1;
                Debug.Log(achievement.title);
            }
            else if (achievement.title == "Oh, Shit!")
                achievement.requirement = o => NumDEATHS >= 1;
        }
    }

    private void Update()
    {
        CheckAchievementCompletion();
    }

    private void CheckAchievementCompletion()
    {
        if (achievementsData == null)
            return;

        foreach (var achievement in achievementsData)
        {
            UpdateCompletion(achievement);
        }
    }

    private void UpdateCompletion(AchievementData achievement)
    {
        if (achievement.achieved)
            return;

        if (achievement.requirement?.Invoke(null) == true)
        {
            DisplayAchievement(achievement);
            achievement.achieved = true;
        }
    }

    private void DisplayAchievement(AchievementData achievement)
    {
        // Останавливаем время
        Time.timeScale = 0;

        // Отображаем панель достижений
        if (achievementPanel != null)
        {
            Debug.Log("ПОКАЗЫВАЮ");
            achievementPanel.SetActive(true);
        }

        // Устанавливаем текст и изображение достижения
        if (achievementTitleText != null)
        {
            achievementTitleText.text = achievement.title;
        }

        if (achievementDescriptionText != null)
        {
            achievementDescriptionText.text = achievement.description;
        }

        if (achievementImage != null)
        {
            achievementImage.sprite = achievement.image;
        }
    }
}