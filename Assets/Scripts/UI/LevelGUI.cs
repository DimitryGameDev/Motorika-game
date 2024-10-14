using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class LevelGUI : MonoBehaviour
    {
        [FormerlySerializedAs("m_Panel")] [SerializeField] private GameObject m_PausePanel;
        [SerializeField] private GameObject m_SettingsPanel;
        [SerializeField] private GameObject pauseButton;
        private int controlID;
        
        private void Start()
        {
            controlID = PlayerPrefs.GetInt("Control");
            m_PausePanel.SetActive(false);
            m_SettingsPanel.SetActive(false);
            Time.timeScale = 1;
            
            if (controlID == 0)
                pauseButton.transform.position = new Vector2(60 , pauseButton.transform.position.y);
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
    }


