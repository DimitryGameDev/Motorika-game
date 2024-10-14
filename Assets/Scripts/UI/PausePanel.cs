using UnityEngine;
using UnityEngine.SceneManagement;

    public class PausePanel : MonoBehaviour
    {
        [SerializeField] private GameObject m_Panel;
        [SerializeField] private GameObject pauseButton;
        private int controlID;
        
        private void Start()
        {
            controlID = PlayerPrefs.GetInt("Control");
            m_Panel.SetActive(false);
            Time.timeScale = 1;
            
            if (controlID == 0)
                pauseButton.transform.position = new Vector2(60 , pauseButton.transform.position.y);
        }
        
        public void ShowPause()
        {
            m_Panel.SetActive(true);
            Time.timeScale = 0;
        }

        public void HidePause()
        {
            m_Panel.SetActive(false);
            Time.timeScale = 1;
        }
        public void LoadMainMenu()
        {
            m_Panel.SetActive(false);
            Time.timeScale = 1;

            SceneManager.LoadScene(0);
        }
    }


