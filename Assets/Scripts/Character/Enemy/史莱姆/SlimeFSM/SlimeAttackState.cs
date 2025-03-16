using System;
using UnityEngine;  // 引入 Unity 引擎中的 Random 类

public class SlimeAttackState : SlimeState
{
    public SlimeAttackState(FSM fsm, Slime character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        if(PlayerManager.Instance.isDead){
            Fsm.SwitchState(Character.IdleState);
        }

        AudioManager.instance.PlaySFX(12, Character.transform);
    }

    public override void Update()
    {
        base.Update();

        if (IsAnimationFinished || !ColDetect.DetectedPlayer)
        {
            Fsm.SwitchState(Character.ChaseState);
            // 使用 UnityEngine.Random.Range 来随机设置攻击冷却时间
            attackCooldownTimer = UnityEngine.Random.Range(Character.minAttackCooldown, Character.maxAttackCooldown);
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
