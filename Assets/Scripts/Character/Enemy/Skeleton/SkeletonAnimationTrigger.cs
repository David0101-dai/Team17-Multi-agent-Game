using System.Diagnostics;
using UnityEngine;

public class SkeletonAnimationTrigger : AnimationTrigger<Slime>
{
    private void OpenCounterWindow()
    {
        //UnityEngine.Debug.Log("触发OpenCounterWindow");
        character.OpenCounterAttackWindow();
    }

    private void CloseCounterWindow()
    {
        //UnityEngine.Debug.Log("触发CloseCounterWindow");
        character.CloseCounterAttackWindow();
    }
}
