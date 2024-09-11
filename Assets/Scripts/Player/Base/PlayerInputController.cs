using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private Player player;
    
    private Animator animator;
    private Turret turret;

    private void Start()
    {
        turret = player.GetComponentInChildren<Turret>();
        animator = player.GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        if (!player.IsBarrier())
            player.Run();
        //else
            //player.Idle();

        if (Input.GetMouseButton(0) && turret != null)
        {
            turret.Fire();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            player.Jump();
        }

        if (Input.GetKey(KeyCode.S))
        {
            player.Slide();
        }
    }
}