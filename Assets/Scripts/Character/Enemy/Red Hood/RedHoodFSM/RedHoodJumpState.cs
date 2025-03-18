using System.Diagnostics;
using System.Collections;
using UnityEngine;

public class RedHoodJumpState : RedHoodAirState
{
    public RedHoodJumpState(FSM fsm, RedHood character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }
    
    private Coroutine jumpCoroutine;

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        // 设置跳跃冷却计时器
        Character.jumpCooldownTimer = Character.jumpCooldown;

        // 启动跳跃的协程
        jumpCoroutine = Character.StartCoroutine(JumpCoroutine());
    }

      // 跳跃协程
    private IEnumerator JumpCoroutine()
    {
        // 设置跳跃的速度
        Rb.velocity = new Vector2(Character.jumpForce.x * (-Character.Flip.facingDir), Character.jumpForce.y);

        // 更新跳跃动画
        Character.Anim.SetFloat("Yvelocity", Rb.velocity.y);

        // 等待跳跃完成（假设跳跃时间为 0.5 秒）
        yield return new WaitForSeconds(0.5f);

        // 如果跳跃未结束，开始下落
        if (Rb.velocity.y > 0)
        {
            while (Rb.velocity.y > 0)
            {
                yield return null;
            }
        }

        // 跳跃完成后进入 FallState
        Fsm.SwitchState(Character.FallState);
    }


    public override void Update()
    {
        base.Update();

        // 更新跳跃时的 Y 轴速度，以便处理动画
        Character.Anim.SetFloat("Yvelocity", Rb.velocity.y);

        // 检查是否进入下落状态
        if (Rb.velocity.y < 0)
        {
            Fsm.SwitchState(Character.FallState);
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);

        // 如果跳跃协程正在运行，停止它
        if (jumpCoroutine != null)
        {
            Character.StopCoroutine(jumpCoroutine);
            jumpCoroutine = null;
        }
    }
}
