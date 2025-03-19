using System.IO;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.TextCore.Text;

public class RedHoodAimState : RedHoodBattleState
{
    private bool hasAimed = false;  // 用于跟踪是否已经完成瞄准过程

    public RedHoodAimState(FSM fsm, RedHood character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
        
        // 设置状态计时器为 0.1 秒，但确保不会在动画播放过程中强制切换状态
        StateTimer = 0.1f;
        hasAimed = false;
    }

    public override void Update()
    {
        base.Update();

        // 更新状态计时器
        StateTimer -= Time.deltaTime;

        // 如果状态计时器结束，但动画还没有播放完成，继续待在 AimState
        if (StateTimer <= 0 && !hasAimed)
        {
            SetVelocity(0, 0);
        }
        
        if(dashDir != Flip.facingDir){
            Flip.Flip();
        }
        // 检查动画是否完成，并且可以切换到 IdleState
        if (IsAnimationFinished && !hasAimed && Character.RedHoodType == RedHoodType.level1)
        {
            hasAimed = true;  // 确保动画完成后才会切换到 IdleState
            Fsm.SwitchState(Character.IdleState);
        }

        if (IsAnimationFinished && !hasAimed && Character.RedHoodType == RedHoodType.level2)
        {
            hasAimed = true;  // 确保动画完成后才会切换到 IdleState
            Fsm.SwitchState(Character.DashState);
        }

        if (IsAnimationFinished && !hasAimed && Character.RedHoodType == RedHoodType.level3)
        {
            hasAimed = true;  // 确保动画完成后才会切换到 IdleState
            Fsm.SwitchState(Character.DashState);
        }

         if (IsAnimationFinished && !hasAimed && Character.RedHoodType == RedHoodType.level4)
        {
            hasAimed = true;  // 确保动画完成后才会切换到 IdleState
            Fsm.SwitchState(Character.DashState);
            // 50% 概率决定进入 AimState（射击状态）或 AttackState（攻击状态）
                if (Random.value < 0.8f)
                {
                    Fsm.SwitchState(Character.DashState); // 50% 概率进入射击状态
                }
                else
                {
                    Fsm.SwitchState(Character.JumpState);  // 50% 概率进入攻击状态
                }
        }

        if(hasAimed){
            Exit(Character.DashState);
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
