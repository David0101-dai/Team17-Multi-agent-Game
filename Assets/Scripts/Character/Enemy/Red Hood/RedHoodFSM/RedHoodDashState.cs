using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class RedHoodDashState :  RedHoodBattleState
{
    public RedHoodDashState(FSM fsm, RedHood character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }
    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
        StateTimer = Character.dashDuration;
        Character.Damageable.isInvincible = true;  // 冲刺期间无敌
        // 如果是 level3 类型，决定冲刺的方向
        if (Character.RedHoodType == RedHoodType.level3)
        {
            // 50% 概率决定冲刺的方向
            dashDir = Random.value < 0.5f ? 1 : -1; // 50% 概率向前（1）或者向后（-1）
        }
    }
    
    public override void Update()
    {
        base.Update();

        if (Character.RedHoodType == RedHoodType.level2 || Character.RedHoodType == RedHoodType.level1)
        {
            SetVelocity(dashDir * Character.dashSpeed, 0);

            if (StateTimer <= 0)
            {
                Fsm.SwitchState(Character.IdleState);
            }
        }
        
        if (Character.RedHoodType == RedHoodType.level3)
        {
            // 如果是 level3 类型，根据 dashDir 决定冲刺方向
            if (dashDir == 1)
            {
                // 向前冲刺
                SetVelocity(dashDir * Character.dashSpeed, 0);
            }
            else
            {
                // 向后冲刺
                Flip.Flip();  // 反转角色方向
                SetVelocity(dashDir * Character.dashSpeed, 0);
            }

            if(ColDetect.DetectedPlayer && canJump() && Character.GetDistanceToPlayer() < Character.safeDistance){
            Fsm.SwitchState(Character.JumpState);
            }else if(Character.canAttack){
            SetVelocity(0, Rb.velocity.y);
            Fsm.SwitchState(Character.AttackState);
            }

             // 检查是否结束冲刺
            if (StateTimer <= 0)
            {
                // 50% 概率决定进入 AimState（射击状态）或 AttackState（攻击状态）
                if (Random.value < 0.5f)
                {
                    Fsm.SwitchState(Character.AimState);  // 50% 概率进入射击状态
                }
                else
                {
                    Fsm.SwitchState(Character.ChaseState);  // 50% 概率进入攻击状态
                }
            }
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
         Character.Damageable.isInvincible = false;  // 冲刺期间无敌
    }
}
