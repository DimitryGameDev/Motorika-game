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
    [SerializeField] private List<TMP_Text> loreModeTexts;
    [SerializeField] private List<TMP_Text> loreModeTitles;
    [SerializeField] private List<Image> loreModeImages;
    [SerializeField] private List<TMP_Text> enemiesModeTexts;
    [SerializeField] private List<TMP_Text> enemiesModeTitles;
    [SerializeField] private List<Image> enemiesModeImages;
    [SerializeField] private List<TMP_Text> abilitiesModeTexts;
    [SerializeField] private List<TMP_Text> abilitiesModeTitles;
    [SerializeField] private List<Image> abilitiesModeImages;
    private Dictionary<JournalType, List<TMP_Text>> journalTexts;
    private Dictionary<JournalType, List<TMP_Text>> journalTitles;
    private Dictionary<JournalType, List<Image>> journalImages;
    private JournalType currentJournalType = JournalType.LoreMode;
    private int currentIndex;

    private void Start()
    {
        currentIndex = 0;
        journalTexts = new Dictionary<JournalType, List<TMP_Text>>()
        {
            { JournalType.LoreMode, loreModeTexts },
            { JournalType.EnemiesMode, enemiesModeTexts },
            { JournalType.AbilitiesMode, abilitiesModeTexts },
            
        };
        journalTitles = new Dictionary<JournalType, List<TMP_Text>>()
        {
            { JournalType.LoreMode, loreModeTitles },
            { JournalType.EnemiesMode, enemiesModeTitles },
            { JournalType.AbilitiesMode, abilitiesModeTitles },
            
        };
        Debug.Log(currentJournalType);
        journalImages = new Dictionary<JournalType, List<Image>>()
        {
            { JournalType.LoreMode, loreModeImages },
            { JournalType.EnemiesMode, enemiesModeImages },
            { JournalType.AbilitiesMode, abilitiesModeImages }
        };
        journalTitles[currentJournalType][currentIndex].gameObject.SetActive(true);
        journalTexts[currentJournalType][currentIndex].gameObject.SetActive(true);
        journalImages[currentJournalType][currentIndex].gameObject.SetActive(true);
       
    }

    private void SwitchJournalType(JournalType newJournalType)
    {
        journalTexts[currentJournalType][currentIndex].gameObject.SetActive(false);
        journalTitles[currentJournalType][currentIndex].gameObject.SetActive(false);
        journalImages[currentJournalType][currentIndex].gameObject.SetActive(false);


        currentJournalType = newJournalType;
        currentIndex = 0;


        journalTexts[currentJournalType][currentIndex].gameObject.SetActive(true);
        journalTitles[currentJournalType][currentIndex].gameObject.SetActive(true);

        journalImages[currentJournalType][currentIndex].gameObject.SetActive(true);

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
        journalTexts[currentJournalType][currentIndex].gameObject.SetActive(false);
        journalTitles[currentJournalType][currentIndex].gameObject.SetActive(false);
        journalImages[currentJournalType][currentIndex].gameObject.SetActive(false);

        currentIndex ++ ;
        journalTexts[currentJournalType][currentIndex].gameObject.SetActive(true);
        journalTitles[currentJournalType][currentIndex].gameObject.SetActive(true);
        journalImages[currentJournalType][currentIndex].gameObject.SetActive(true);
    }

    public void ShowPreviousContent()
    {
        if (currentIndex > 0)
        {
            journalTexts[currentJournalType][currentIndex].gameObject.SetActive(false);
            journalTitles[currentJournalType][currentIndex].gameObject.SetActive(false);
            journalImages[currentJournalType][currentIndex].gameObject.SetActive(false);

            currentIndex --;

            journalTexts[currentJournalType][currentIndex].gameObject.SetActive(true);
            journalTitles[currentJournalType][currentIndex].gameObject.SetActive(true);
            journalImages[currentJournalType][currentIndex].gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Невозможно перейти к предыдущему контенту, текущий индекс уже на нуле.");
        }
    }
}
