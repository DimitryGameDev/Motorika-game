using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu]
public class AnimatorsAssetMenu : ScriptableObject
{
    public AnimatorController animatorRun;
    public AnimatorController animatorJump;
    public AnimatorController animatorSlide;
}