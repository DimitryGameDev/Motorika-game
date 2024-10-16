using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class LevelGUI : MonoBehaviour
{
    private const string text1 = "���������� ����: ";

    [FormerlySerializedAs("m_Panel")][SerializeField] private GameObject m_PausePanel;
    [SerializeField] private GameObject m_ResultPanel;
    [SerializeField] private GameObject m_SettingsPanel;
    [SerializeField] private GameObject pauseButtonLeft;
    [SerializeField] private GameObject pauseButtonRight;
    [SerializeField] private TMP_Text coinText;

    [SerializeField] private Destructible playerDestructible;

    private Bag bag;

    private int controlID;
    private int coinID;

    private void Start()
    {
        controlID = PlayerPrefs.GetInt("Control");
        coinID = PlayerPrefs.GetInt("Coin");

        bag = playerDestructible.GetComponent<Bag>();

        SetPauseButton();

        m_PausePanel.SetActive(false);
        m_ResultPanel.SetActive(false);
        m_SettingsPanel.SetActive(false);
        Time.timeScale = 1;

        if(playerDestructible != null)
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

        if (bag != null)
        {
            coinText.text = text1 + bag.GetCoinAmount();
            PlayerPrefs.SetInt("Coin", coinID + bag.GetCoinAmount());
            PlayerPrefs.Save();
        }
    }

    private void SetPauseButton()
    {
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
}