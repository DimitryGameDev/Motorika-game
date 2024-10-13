using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilitiesChanger : MonoBehaviour
{
    [SerializeField] private Abilities[] abilities;
    public Abilities[] Abilities => abilities;

    [SerializeField] private GameObject leftPanel;
    [SerializeField] private GameObject rightPanel;

    [SerializeField] private Image currentAbilityImageLeft;
    [SerializeField] private Image changeAbilityImageLeft;
    [SerializeField] private Image currentAbilityImageRight;
    [SerializeField] private Image changeAbilityImageRight;

    [SerializeField] private TMP_Text descriptionTextLeft;
    [SerializeField] private TMP_Text descriptionTextRight;

    [SerializeField] private Button closeButton;

    [SerializeField] private Bag playerBag;
    [SerializeField] private int countForChange;

    private int firstAbilitiesIndex;
    private int secondAbilitiesIndex;

    private int previousFirstIndex;
    private int previousSecondIndex;
    public int PreviousFirstIndex => previousFirstIndex;
    public int PrevoiusSecondIndex => previousSecondIndex;

    private bool isFirstChanging;

    private void Awake()
    {
        leftPanel.SetActive(false);
        //rightPanel.SetActive(false);

        previousFirstIndex = firstAbilitiesIndex;
        previousSecondIndex = secondAbilitiesIndex;
    }

    private void Start()
    {
        RandomValue();
    }

    private void Update()
    {
        OpenPanel();
    }

    private void OpenPanel()
    {
        if (playerBag.GetAnomaliesAmount() >= countForChange)
        {
            if (isFirstChanging)
            {
                currentAbilityImageLeft.sprite = abilities[previousFirstIndex].Icon;
                changeAbilityImageLeft.sprite = abilities[firstAbilitiesIndex].Icon;
                descriptionTextLeft.text = abilities[firstAbilitiesIndex].SetText(firstAbilitiesIndex);
            }
            else
            {
                currentAbilityImageLeft.sprite = abilities[previousSecondIndex].Icon;
                changeAbilityImageLeft.sprite = abilities[secondAbilitiesIndex].Icon;
                descriptionTextLeft.text = abilities[secondAbilitiesIndex].SetText(secondAbilitiesIndex);
            }

            leftPanel.SetActive(true);
            //rightPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void RandomValue()
    {
        if (isFirstChanging)
        {
            if (firstAbilitiesIndex == secondAbilitiesIndex || firstAbilitiesIndex == previousFirstIndex)
                firstAbilitiesIndex = Random.Range(1, abilities.Length);
        }
        else
        {
            if (secondAbilitiesIndex == firstAbilitiesIndex || secondAbilitiesIndex == previousSecondIndex)
                secondAbilitiesIndex = Random.Range(1, abilities.Length);
        }
    }

    public void ClosePanel()
    {
        DrawAnomalies();

        leftPanel.SetActive(false);
        //rightPanel.SetActive(false);
        Time.timeScale = 1;
        RandomValue();
    }

    private void DrawAnomalies()
    {
        playerBag.DrawAnomalies(countForChange);
    }

    public void ChangeAbility()
    {
        previousFirstIndex = firstAbilitiesIndex;
        previousSecondIndex = secondAbilitiesIndex;

        if (isFirstChanging)
            isFirstChanging = false;
        else
            isFirstChanging = true;

        RandomValue();
        ClosePanel();
    }

    public void SetFirstAbility(Image image)
    {
        image.sprite = abilities[previousFirstIndex].Icon;
    }

    public void SetSecondAbility(Image image)
    {
        image.sprite = abilities[previousSecondIndex].Icon;
    }
}