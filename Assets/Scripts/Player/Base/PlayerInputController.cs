using UnityEngine;
using UnityEngine.Events;

public class PlayerInputController : MonoSingleton<PlayerInputController>
{
    public enum ControlMode
    {
        Keyboard,
        Mobile,
        Mixed
    }
    [SerializeField] private Player player;
    [SerializeField] private ControlMode m_ControlMode;
    public event UnityAction RunEvent; // event
    public event UnityAction JumpEvent;
    public event UnityAction SlideEvent;

    public event UnityAction FirstAbilityEvent;
    public event UnityAction SecondAbilityEvent;

    [SerializeField] private VirtualGamepad m_VirtualGamepad;

    private void Start()
    {
        m_VirtualGamepad.FirstAbility.Click += OnFirstAbilityClick;
        m_VirtualGamepad.SecondAbility.Click += OnSecondAbilityClick;
        
        if (m_ControlMode == ControlMode.Keyboard)
        {
            m_VirtualGamepad.Jump.gameObject.SetActive(false);
            m_VirtualGamepad.Slide.gameObject.SetActive(false);
            m_VirtualGamepad.FirstAbility.gameObject.SetActive(false);
            m_VirtualGamepad.SecondAbility.gameObject.SetActive(false);
        }
        if (m_ControlMode == ControlMode.Mobile)
        {
            m_VirtualGamepad.Jump.gameObject.SetActive(true);
            m_VirtualGamepad.Slide.gameObject.SetActive(true);
            m_VirtualGamepad.FirstAbility.gameObject.SetActive(true);
            m_VirtualGamepad.SecondAbility.gameObject.SetActive(true);
        }
        if (m_ControlMode == ControlMode.Mixed)
        {
            m_VirtualGamepad.Jump.gameObject.SetActive(false);
            m_VirtualGamepad.Slide.gameObject.SetActive(false);
            m_VirtualGamepad.FirstAbility.gameObject.SetActive(true);
            m_VirtualGamepad.SecondAbility.gameObject.SetActive(true);
        }
    }
    
    private void Update()
    {
        if (player != null)
        {
            if (m_ControlMode == ControlMode.Mobile)
            {
                ControlMobile();
            }

            if (m_ControlMode == ControlMode.Keyboard)

            {
                ControlKeyboard();
            }
            if (m_ControlMode == ControlMode.Mixed)
            {
                ControlMixed();
            }
        }
    }

    private void OnDestroy()
    {
        m_VirtualGamepad.FirstAbility.Click -= OnFirstAbilityClick;
        m_VirtualGamepad.SecondAbility.Click -= OnSecondAbilityClick;
    }
    
    private void OnFirstAbilityClick()
    {
        FirstAbilityEvent?.Invoke();
    }

    private void OnSecondAbilityClick()
    {
        SecondAbilityEvent?.Invoke();
    }

    private void ControlMobile()
    {
        if (m_VirtualGamepad.Jump.IsHold) JumpEvent?.Invoke();
        if (m_VirtualGamepad.Slide.IsHold) SlideEvent?.Invoke();
        else RunEvent?.Invoke();
    }
    private void ControlKeyboard()
    {
        if (Input.GetKey(KeyCode.T)) FirstAbilityEvent?.Invoke();
        if (Input.GetKey(KeyCode.D)) SecondAbilityEvent?.Invoke();
       // if (Input.GetKeyDown(KeyCode.Space)) JumpEvent?.Invoke();
        if (Input.GetKey(KeyCode.S))
        {
            SlideEvent?.Invoke();
        }
        else
        {
            RunEvent?.Invoke();
        }
    }
    private void ControlMixed()
    {
        if (Input.GetKey(KeyCode.Space)) JumpEvent?.Invoke();
        if (Input.GetKey(KeyCode.S))
        {
            SlideEvent?.Invoke();
        }
        else
        {
            RunEvent?.Invoke();
        }
    }
}