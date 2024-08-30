using UnityEngine;

public class AnimatorsChange : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private AnimatorsAssetMenu assetMenu;
    [SerializeField] private Animator animator;

    private void Start()
    {
        player.RunEvent += OnRun;
        player.JumpEvent += OnJump;
        player.SlideEvent += OnSlide;
    }

    private void OnDestroy()
    {
        player.RunEvent -= OnRun;
        player.JumpEvent -= OnJump;
        player.SlideEvent -= OnSlide;
    }
    

    private void OnRun()
    {
        ChangeController(assetMenu.animatorRun);
    }

    private void OnJump()
    {
        ChangeController(assetMenu.animatorJump);
    }

    private void OnSlide()
    {
        ChangeController(assetMenu.animatorSlide);
    }

    private void ChangeController(RuntimeAnimatorController newRuntimeAnimatorController)
    {
        animator.runtimeAnimatorController = newRuntimeAnimatorController;
    }
}