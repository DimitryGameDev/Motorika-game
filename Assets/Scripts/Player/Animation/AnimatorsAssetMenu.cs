using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu]
public class AnimatorsAssetMenu : ScriptableObject
{
    [SerializeField] private AnimatorController animatorRun;
    [SerializeField] private AnimatorController animatorJump;
    [SerializeField] private AnimatorController animatorSlide;

    public AnimatorController AnimatorRun => animatorRun;
    public AnimatorController AnimatorJump => animatorJump;
    public AnimatorController AnimatorSlide => animatorSlide;
}