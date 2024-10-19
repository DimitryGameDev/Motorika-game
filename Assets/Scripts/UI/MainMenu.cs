using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject journalPanel;
    [SerializeField] private GameObject characterPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject snailPanel;
    [SerializeField] private GameObject airdashPanel;
    [SerializeField] private GameObject aimPanel;
    [SerializeField] private GameObject icePanel;
    [SerializeField] private GameObject lightningPanel;
    [SerializeField] private GameObject achievementPanel;
    [SerializeField] private GameObject m_MainPanel;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private AudioMute audioMute;
    
    private void Start()
    {
        ShowMainPanel();
        coinText.text = PlayerPrefs.GetInt("Coin").ToString();
    }

    public void ShowMainPanel()
    {
        m_MainPanel.SetActive(true);
        settingsPanel.SetActive(false);
        journalPanel.SetActive(false);
    }

    public void EX_ShowStartPanel()
    {
        startPanel.SetActive(true);
        m_MainPanel.SetActive(false);
    }

    public void EX_ShowSettingsPanel()
    {
        settingsPanel.SetActive(true);
        m_MainPanel.SetActive(false);
    }
    public void EX_CloseSettingsPanel()
    {
        audioMute.SetSoundVolume();
        settingsPanel.SetActive(false);
        m_MainPanel.SetActive(true);
    }
    public void EX_ShowCharacterPanel()
    {
        characterPanel.SetActive(true);
        m_MainPanel.SetActive(false);
    }
    public void EX_CloseCharacterPanel()
    {
        characterPanel.SetActive(false);
        m_MainPanel.SetActive(true);
    }
    public void EX_ShowJournalPanel()
    {
        journalPanel.SetActive(true);
        m_MainPanel.SetActive(false);
    }
    public void EX_CloseJournalPanel()
    {
        journalPanel.SetActive(false);
        m_MainPanel.SetActive(true);
    }
    public void CP_ShowSnailPanel()
    {
        snailPanel.SetActive(true);
        characterPanel.SetActive(false);
    }
    public void CP_CloseSnailPanel()
    {
        snailPanel.SetActive(false);
        characterPanel.SetActive(true);
    }
    public void CP_ShowAirDashPanel()
    {
        airdashPanel.SetActive(true);
        characterPanel.SetActive(false);
    }
    public void CP_CloseAirDashPanel()
    {
        airdashPanel.SetActive(false);
        characterPanel.SetActive(true);
    }
    public void CP_ShowAimPanel()
    {
        aimPanel.SetActive(true);
        characterPanel.SetActive(false);
    }
    public void CP_CloseAimPanel()
    {
        aimPanel.SetActive(false);
        characterPanel.SetActive(true);
    }
    public void CP_ShowIcePanel()
    {
        icePanel.SetActive(true);
        characterPanel.SetActive(false);
    }
    public void CP_CloseIcePanel()
    {
        icePanel.SetActive(false);
        characterPanel.SetActive(true);
    }
    public void CP_ShowLightningPanel()
    {
        lightningPanel.SetActive(true);
        characterPanel.SetActive(false);
    }
    public void CP_ShowAchievementPanel()
    {
        achievementPanel.SetActive(true);
        characterPanel.SetActive(false);
    }
    public void CP_CloseAchievementPanel()
    {
        achievementPanel.SetActive(false);
        characterPanel.SetActive(true);
    }
    public void CP_CloseLightningPanel()
    {
        lightningPanel.SetActive(false);
        characterPanel.SetActive(true);
    }
}


