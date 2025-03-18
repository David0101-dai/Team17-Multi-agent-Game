using Unity.VisualScripting;
using UnityEngine;

public class AnimationTrigger<T> : MonoBehaviour where T : Character
{
    protected T character;

    private void Start()
    {
        character = GetComponentInParent<T>();
        if (!character) Debug.LogError($"父物体未找到名为 {typeof(T)} 的组件");
    }

    private void AnimationFinishTrigger()
    {
        character.Fsm.CurrentState?.AnimationFinishTrigger();
    }

    private void AttackTrigger()
    {
        var colliders = Physics2D.OverlapCircleAll(character.attackCheck.position, character.attackCheckRadius);
        foreach (var hit in colliders)
        {   
            if (hit.transform == transform.parent) continue;
            if (transform.parent.CompareTag("Player") && hit.GetComponent<EnergyBall_Controller>() != null){
                hit.GetComponent<EnergyBall_Controller>().FlipArrow();
            }
            if (transform.parent.CompareTag("Enemy") && hit.CompareTag("Enemy")) continue;
            hit.GetComponent<Damageable>()?.TakeDamage(transform.parent.gameObject);
        }
    }

    private void FireAttackTrigger()
    {
        var colliders = Physics2D.OverlapCircleAll(character.attackCheck.position, character.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.transform == transform.parent) continue;
            if (transform.parent.CompareTag("Enemy") && hit.CompareTag("Enemy")) continue;
            hit.GetComponent<Damageable>()?.TakeDamage(transform.parent.gameObject,true,false,false,true,false,false);
        }
    }
    private void IceAttackTrigger()
    {
        var colliders = Physics2D.OverlapCircleAll(character.attackCheck.position, character.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.transform == transform.parent) continue;
            if (transform.parent.CompareTag("Enemy") && hit.CompareTag("Enemy")) continue;
            hit.GetComponent<Damageable>()?.TakeDamage(transform.parent.gameObject,true,false,false,false,true,false);
        }
    }
    private void ShockAttackTrigger()
    {
        var colliders = Physics2D.OverlapCircleAll(character.attackCheck.position, character.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.transform == transform.parent) continue;
            if (transform.parent.CompareTag("Enemy") && hit.CompareTag("Enemy")) continue;
            hit.GetComponent<Damageable>()?.TakeDamage(transform.parent.gameObject,true,false,false,false,false,true);
        }
    }    
}
