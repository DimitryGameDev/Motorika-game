using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private Player player;

    private Turret turret;

    private void Start()
    {
        turret = player.GetComponentInChildren<Turret>();
    }

    private void FixedUpdate()
    {
        if (player == null) return;

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
        else
        {
            player.Run();
        }
    }
}