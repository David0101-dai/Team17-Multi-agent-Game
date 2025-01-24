using UnityEngine; // 需要引用 Unity 引擎的命名空间，包含 Debug.Log 方法
// 使用 System.Diagnostics 通常是为了调试，除非有明确需要

public class SkeletonPatrolState : SkeletonGroundState
{
    public SkeletonPatrolState(FSM fsm, Skeleton character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
    }

    public override void Update()
    {
        base.Update();

        SetVelocity(Flip.facingDir * Character.moveSpeed, Rb.velocity.y);
        //Debug.Log(" | IsGrounded: " + ColDetect.IsGrounded);
        
        if (ColDetect.IsWallDetected || !ColDetect.IsGrounded)
        {
            Flip.Flip();
          //  Debug.Log("骷髅没有找到地面,开始反转");
            SetVelocity(Flip.facingDir * Character.moveSpeed, Rb.velocity.y);
            Fsm.SwitchState(Character.IdleState);
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
