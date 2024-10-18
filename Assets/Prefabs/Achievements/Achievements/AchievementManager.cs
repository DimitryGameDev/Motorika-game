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
    [HideInInspector] public int NumProjAim = 0;
    [HideInInspector] public int NumProjFreeze = 0;
    [HideInInspector] public int NumProjLightning = 0;
    [HideInInspector] public int NumTimeScale = 0;
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

    public void LightningWeapon()
    {
        NumProjLightning++;
    }

    public void AimWeapon()
    {
        NumProjAim++;
    }
    public void FreezeWeapon()
    {
        NumProjFreeze++;
    }

    public void TimeScale()
    {
        NumTimeScale++;
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
            if (achievement.title == "Неплохое начало")
                achievement.requirement = o => NumKILLS >= 10;
            if (achievement.title == "Мастер")
                achievement.requirement = o => NumKILLS >= 30;
            if (achievement.title == "ЧЕМПИОН")
                achievement.requirement = o => NumKILLS >= 50;
            if (achievement.title == "ЛЕГЕНДА")
                achievement.requirement = o => NumKILLS >= 100;
            if (achievement.title == "Бывает")
                achievement.requirement = o => NumDEATHS >= 1;
            if (achievement.title == "Авто-аимер:новичок")
                achievement.requirement = o => NumProjAim >= 10; 
            if (achievement.title == "Ниже нуля:новичок")
                achievement.requirement = o => NumProjFreeze >= 10;
            if (achievement.title == "Молния:новичок")
                achievement.requirement = o => NumProjLightning >= 10;
            if (achievement.title == "Авто-аимер:мастер")
                achievement.requirement = o => NumProjAim >= 30; 
            if (achievement.title == "Ниже нуля:мастер")
                achievement.requirement = o => NumProjFreeze >= 30;
            if (achievement.title == "Молния:мастер")
                achievement.requirement = o => NumProjLightning >= 30;
            if (achievement.title == "Слоумо")
                achievement.requirement = o => NumTimeScale >= 3;
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