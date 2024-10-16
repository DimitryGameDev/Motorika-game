using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
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
        
        firstButton.interactable = true;
        secondButton.interactable = true;
    }

    private void Start()
    {
        FirstAbility.Click += () => SetCooldown(ref timerFirst, firstButton);
        SecondAbility.Click += () => SetCooldown(ref timerSecond, secondButton);
    }

    private void Update()
    {
        UpdateCooldown(ref timerFirst, firstButton);
        UpdateCooldown(ref timerSecond, secondButton);
        
            SetButton();
    }

    private void SetButton()
    {
        abilitiesChanger.SetFirstAbility(firstAbility);
        abilitiesChanger.SetSecondAbility(secondAbility);
    }

    private void SetCooldown(ref float timer, Button button)
    {
        button.interactable = false;
        timer = cooldown;
    }

    private void UpdateCooldown(ref float timer, Button button)
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                button.interactable = true;
                timer = 0;
            }
        }
    }
}