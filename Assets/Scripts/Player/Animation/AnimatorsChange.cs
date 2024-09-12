using UnityEngine;

public class AnimatorsChange : MonoBehaviour
{
    private Player player;
    private Animator animator;

    private void Awake()
    {
        player = GetComponent<Player>();
        animator = GetComponentInChildren<Animator>();
    }
    /*
    private void Start()
    {
        player.RunEvent += OnRun;
        player.IdleEvent += OnIdle;
        player.JumpEvent += OnJump;
        player.SlideEvent += OnSlide;
    }

    private void OnDestroy()
    {
        player.RunEvent -= OnRun;
        player.IdleEvent -= OnIdle;
        player.JumpEvent -= OnJump;
        player.SlideEvent -= OnSlide;
    }

    private void OnRun()
    {
        //animator.SetTrigger("Run");
    }

    private void OnIdle()
    {
        
        //animator.SetBool("IsIdle", true);
    }

    private void OnJump()
    {
        //animator.SetTrigger("Jump");
    }

    private void OnSlide()
    {
        //animator.SetTrigger("Slide");
    }*/
}