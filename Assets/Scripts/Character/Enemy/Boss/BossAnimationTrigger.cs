using System.Diagnostics;
using UnityEngine;

public class BossAnimationTrigger : AnimationTrigger<Boss>
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
