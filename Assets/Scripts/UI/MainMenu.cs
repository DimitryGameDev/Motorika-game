using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject boostPanel;
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private GameObject m_MainPanel;
        private void Start()
        {
            ShowMainPanel();
        }
        public void ShowMainPanel()
        {
            m_MainPanel.SetActive(true);
            settingsPanel.SetActive(false);
            boostPanel.SetActive(false);
        }
        public void EX_ShowSettingsPanel()
        {
            settingsPanel.SetActive(true);
            m_MainPanel.SetActive(false);
        }
        public void EX_ShowBoostPanel()
        {
            boostPanel.SetActive(true);
            m_MainPanel.SetActive(false);
        }
       
    }


