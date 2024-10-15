using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum JournalType
{
    LoreMode,
    EnemiesMode,
    AbilitiesMode
}

public class JournalPanel : MonoBehaviour
{
    
    [SerializeField] private List<GameObject> loreModeContent;
    [SerializeField] private List<GameObject> enemiesModeContent;
    [SerializeField] private List<GameObject> abilitiesModeContent;
    private Dictionary<JournalType, List<GameObject>> journalContents;
    private JournalType currentJournalType = JournalType.LoreMode;
    private int currentIndex;

    private void Start()
    {
        currentIndex = 0;
        journalContents = new Dictionary<JournalType, List<GameObject>>()
        {
            { JournalType.LoreMode, loreModeContent },
            { JournalType.EnemiesMode, enemiesModeContent },
            { JournalType.AbilitiesMode, abilitiesModeContent },
            
        };

        journalContents[currentJournalType][currentIndex].SetActive(true);
    }

    private void SwitchJournalType(JournalType newJournalType)
    {
        journalContents[currentJournalType][currentIndex].SetActive(false);

        currentJournalType = newJournalType;
        currentIndex = 0;

        journalContents[currentJournalType][currentIndex].SetActive(true);

        Debug.Log(currentIndex + " : " + currentJournalType);
    }
    public void SwitchToEnemiesMode()
    {
        SwitchJournalType(JournalType.EnemiesMode);
    }

    public void SwitchToAbilitiesMode()
    {
        SwitchJournalType(JournalType.AbilitiesMode);
    }

    public void SwitchToLoreMode()
    {
        SwitchJournalType(JournalType.LoreMode);
    }

    public void ShowNextContent()
    {
        Debug.Log(currentIndex + " : " + currentJournalType);
        journalContents[currentJournalType][currentIndex].SetActive(false);

        currentIndex++;

        if (currentIndex >= journalContents[currentJournalType].Count) currentIndex = 0;

         journalContents[currentJournalType][currentIndex].SetActive(true);
    }

    public void ShowPreviousContent()
    {
        if (currentIndex > 0)
        {
            journalContents[currentJournalType][currentIndex].SetActive(false);

            currentIndex--;

            journalContents[currentJournalType][currentIndex].SetActive(true);
        }
        else
        {
            Debug.LogWarning("Невозможно перейти к предыдущему контенту, текущий индекс уже на нуле.");
        }
    }
}
