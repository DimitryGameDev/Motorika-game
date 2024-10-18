using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementsMenuManager : MonoBehaviour
{
    public List<AchievementData> achievements;
    public GameObject achievementPanelPrefab;
    public Transform achievementMenuPanel;
    public GameObject achievementDetailsPanel; // Панель для отображения подробной информации об ачивке
    public TMP_Text detailsTitleText;
    public TMP_Text detailsDescriptionText;
    public Image detailsImage; 

    private void Start()
    {
        DisplayAchievements();
    }

    private void DisplayAchievements()
    {
        foreach (var achievement in achievements)
        {
            GameObject achievementPanel = Instantiate(achievementPanelPrefab, achievementMenuPanel);
            var achievementButton = achievementPanel.GetComponent<AchievementButton>();

            if (achievementButton != null)
            {
                achievementButton.achievementData = achievement;
                achievementButton.background = achievementPanel.GetComponent<Image>();
            }
        }
    }

    public void DisplayAchievementDetails(AchievementData achievementData)
    {
        achievementDetailsPanel.SetActive(true);
        detailsTitleText.text = achievementData.title;
        detailsDescriptionText.text = achievementData.description;
        detailsImage.sprite = achievementData.image;
    }
}
