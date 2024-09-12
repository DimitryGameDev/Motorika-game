using UnityEngine;
using UnityEngine.Events;

public class PlayerInputController : MonoSingleton<PlayerInputController>
{
    [SerializeField] private Player player;

    public event UnityAction RunEvent; // event
    public event UnityAction JumpEvent;
    public event UnityAction SlideEvent;

    private Turret turret;

    private void Start()
    {
        turret = player.GetComponentInChildren<Turret>();
    }

    private void Update()
    {
        if (player == null) return;

        if (Input.GetMouseButton(0) && turret != null)
        {
            turret.Fire();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            JumpEvent?.Invoke();
        }

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