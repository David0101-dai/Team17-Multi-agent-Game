using System.Diagnostics;
using UnityEngine;

public class NightBorneAnimationTrigger : AnimationTrigger<NightBorne>
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

    private void DetonationTrigger()
    {
        var colliders = Physics2D.OverlapCircleAll(character.detonationCheck.position, character.detonationCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.transform == transform.parent) continue;
            if (transform.parent.CompareTag("Enemy") && hit.CompareTag("Enemy")) continue;
            hit.GetComponent<Damageable>()?.TakeDamage(transform.parent.gameObject);
        }
    }
}
