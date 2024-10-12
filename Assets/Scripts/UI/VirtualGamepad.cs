using UnityEngine;
using UnityEngine.UI;

public class VirtualGamepad : MonoBehaviour
{
    [SerializeField] private AbilitiesChanger abilitiesChanger;
    [SerializeField] private float cooldown;
    public float Cooldown => cooldown;

    public PointerClickHold FirstAbility;
    public PointerClickHold SecondAbility;
    public PointerClickHold Slide;
    public PointerClickHold Jump;

    private Image firstAbility;
    private Image secondAbility;

    private Button firstButton;
    private Button secondButton;

    private float timerFirst;
    private float timerSecond;

    private void Awake()
    {
        firstAbility = FirstAbility.GetComponent<Image>();
        secondAbility = SecondAbility.GetComponent<Image>();

        firstButton = FirstAbility.GetComponent<Button>();
        secondButton = SecondAbility.GetComponent<Button>();
    }

    private void Update()
    {
        SetButton();
        SetColldown();

        timerFirst -= Time.deltaTime;
        timerSecond -= Time.deltaTime;
    }

    private void SetButton()
    {
        abilitiesChanger.SetFirstAbility(firstAbility);
        abilitiesChanger.SetSecondAbility(secondAbility);
    }

    private void SetColldown()
    {
        if (FirstAbility.IsHold)
        {
            firstButton.interactable = false;
            timerFirst = cooldown;
        }

        if(SecondAbility.IsHold)
        {
            secondButton.interactable = false;
            timerSecond = cooldown;
        }

        if(timerFirst <= 0 )
        {
            firstButton.interactable = true;
            timerFirst = 0;
        }
        if (timerSecond <= 0)
        {
            secondButton.interactable = true;
            timerSecond = 0;
        }
    }
}