using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class LevelGUI : MonoBehaviour
{
    [FormerlySerializedAs("m_Panel")] [SerializeField]
    private GameObject m_PausePanel;

    [SerializeField] private GameObject m_ResultPanel;
    [SerializeField] private GameObject m_SettingsPanel;
    [SerializeField] private GameObject pauseButtonLeft;
    [SerializeField] private GameObject pauseButtonRight;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private GameObject m_AchievementPanel;
    [SerializeField] private VirtualGamepad virtualGamepad;
    [SerializeField] private Destructible playerDestructible;
    [SerializeField] private AudioMute audioMute;

    [SerializeField] private Transform transformFirstL;
    [SerializeField] private Transform transformFirstR;
    [SerializeField] private Transform transformSecondL;
    [SerializeField] private Transform transformSecondR;
    [SerializeField] private Transform transformJumpL;
    [SerializeField] private Transform transformJumpR;
    [SerializeField] private Transform transformSlideL;
    [SerializeField] private Transform transformSlideR;
    
    private Bag bag;

    private int controlID;
    private int coinID;
    private int soundID;

    private void Start()
    {
        //controlID = PlayerPrefs.GetInt("Control");
        coinID = PlayerPrefs.GetInt("Coin");

        bag = playerDestructible.GetComponent<Bag>();

        SetPauseButton();
        SetGamepadPosition();
        
        m_PausePanel.SetActive(false);
        m_ResultPanel.SetActive(false);
        m_SettingsPanel.SetActive(false);
        Time.timeScale = 1;

        if (playerDestructible != null)
        {
            playerDestructible.EventOnDeath.AddListener(ShowResult);
        }
    }

    private void OnDestroy()
    {
        playerDestructible.EventOnDeath.RemoveAllListeners();
    }

    public void ShowPause()
    {
        m_SettingsPanel.SetActive(false);
        m_PausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void EX_ShowSettingsPanel()
    {
        m_SettingsPanel.SetActive(true);
        m_PausePanel.SetActive(false);
    }

    public void EX_HideAchievementPanel()
    {
        m_AchievementPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void EX_HideSettingsPanel()
    {
        m_SettingsPanel.SetActive(false);
        m_PausePanel.SetActive(true);

        SetSound();
        SetPauseButton();
        SetGamepadPosition();
    }

    public void HidePause()
    {
        m_PausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void LoadMainMenu()
    {
        m_PausePanel.SetActive(false);
        Time.timeScale = 1;

        SceneManager.LoadScene(0);
    }

    private void ShowResult()
    {
        m_ResultPanel.SetActive(true);
        AchievementManager.Instance.OnPlayerDeath();
        if (bag != null)
        {
            coinText.text = bag.GetCoinAmount().ToString();
            PlayerPrefs.SetInt("Coin", coinID + bag.GetCoinAmount());
            PlayerPrefs.Save();
        }
    }

    private void SetSound()
    {
        audioMute.SetSoundVolume();
    }

    private void SetPauseButton()
    {
        controlID = PlayerPrefs.GetInt("Control");

        if (controlID == 0)
        {
            pauseButtonLeft.SetActive(true);
            pauseButtonRight.SetActive(false);
        }
        else
        {
            pauseButtonLeft.SetActive(false);
            pauseButtonRight.SetActive(true);
        }
    }

    private void SetGamepadPosition()
    {
        controlID = PlayerPrefs.GetInt("Control");

        if (controlID == 0)
        {
            virtualGamepad.FirstAbility.SetButtonPosition(transformFirstL);
            virtualGamepad.SecondAbility.SetButtonPosition(transformSecondL);
            virtualGamepad.Jump.SetButtonPosition(transformJumpL);
            virtualGamepad.Slide.SetButtonPosition(transformSlideL);
        }
        else
        {
            virtualGamepad.FirstAbility.SetButtonPosition(transformFirstR);
            virtualGamepad.SecondAbility.SetButtonPosition(transformSecondR);
            virtualGamepad.Jump.SetButtonPosition(transformJumpR);
            virtualGamepad.Slide.SetButtonPosition(transformSlideR);
        }
    }
}