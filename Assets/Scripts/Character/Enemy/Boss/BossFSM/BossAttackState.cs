using System;
using UnityEngine;  // 引入 Unity 引擎中的 Random 类

public class BossAttackState : BossState
{
    public BossAttackState(FSM fsm, Boss character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        if(PlayerManager.Instance.isDead){
            Fsm.SwitchState(Character.IdleState);
        }

        Character.AddChanceToTeleport(5f);
        AudioManager.instance.PlaySFX(12, Character.transform);
    }

    public override void Update()
    {
        base.Update();

        if (IsAnimationFinished || !ColDetect.DetectedPlayer)
        {
            attackCooldownTimer = UnityEngine.Random.Range(Character.minAttackCooldown, Character.maxAttackCooldown);
            if(Character.CanTeleport()){
                Character.ResetChanceToTeleport();
                Fsm.SwitchState(Character.TeleportState);
            }else{
                Fsm.SwitchState(Character.ChaseState);
            }
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
