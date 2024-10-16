using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PanelManager : MonoBehaviour
{
    
[SerializeField] private Button[] buttons;
[SerializeField] private GameObject[] panels;
private GameObject activePanel;

void Start()
{
    if (buttons.Length != panels.Length)
    {
        Debug.LogError("The number of buttons and panels must match");
        return;
    }

    for (int i = 0; i < buttons.Length; i++)
    {
        int index = i; // Capture index for the closure
        buttons[i].onClick.AddListener(() => ShowPanel(index));
    }

    if (panels.Length > 0)
    {
        // Set the first panel as the active panel
        activePanel = panels[0];
        activePanel.SetActive(true);
        
        // Deactivate all other panels
        for (int i = 1; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }
    }
}

void ShowPanel(int index)
{
    if (activePanel != null)
    {
        activePanel.SetActive(false);
    }

    activePanel = panels[index];
    activePanel.SetActive(true);
}
}
