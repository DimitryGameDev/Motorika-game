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

    private Turret turret;
    [SerializeField] private VirtualGamepad m_VirtualGamepad;

    private void Start()
    {
        turret = player.GetComponentInChildren<Turret>();
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
    void Update()
    {
        if (player == null) return;
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

    private void ControlMobile()
    {
        if (m_VirtualGamepad.Jump.IsHold) JumpEvent?.Invoke();

        if (m_VirtualGamepad.Slide.IsHold) SlideEvent?.Invoke();
        else RunEvent?.Invoke();
        
        if (m_VirtualGamepad.FirstAbility.IsHold) FirstAbilityEvent?.Invoke();
        if (m_VirtualGamepad.SecondAbility.IsHold) SecondAbilityEvent?.Invoke();
    }
    private void ControlKeyboard()
    {
        if (Input.GetKey(KeyCode.T)) FirstAbilityEvent?.Invoke();
        if (Input.GetKey(KeyCode.D)) SecondAbilityEvent?.Invoke();
        if (Input.GetMouseButton(0) && turret != null) turret.Fire();
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
        if (m_VirtualGamepad.FirstAbility.IsHold) FirstAbilityEvent?.Invoke();
        if (m_VirtualGamepad.SecondAbility.IsHold) SecondAbilityEvent?.Invoke();
    }
   
}