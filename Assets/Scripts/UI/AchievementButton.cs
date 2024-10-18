using UnityEngine;
using UnityEngine.UI;

public class AchievementButton : MonoBehaviour
{
    public AchievementData achievementData; 
    public Image background;  

    private Button button;
    private AchievementsMenuManager menuManager;

    private void Awake()
    {
        button = GetComponent<Button>();
        menuManager = FindObjectOfType<AchievementsMenuManager>();
        button.onClick.AddListener(OnButtonClick);

        SetButtonTransparency(achievementData.achieved ? 1.0f : 0.5f);
    }

    private void OnButtonClick()
    {
        menuManager.DisplayAchievementDetails(achievementData);
    }

    private void SetButtonTransparency(float alpha)
    {
        if (background != null)
        {
            var color = background.color;
            color.a = alpha;
            background.color = color;
        }
    }
}
