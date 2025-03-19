using UnityEngine; // 需要引用 Unity 引擎的命名空间，包含 Debug.Log 方法
// 使用 System.Diagnostics 通常是为了调试，除非有明确需要

public class DogPatrolState : DogGroundState
{
    public DogPatrolState(FSM fsm, Dog character, string animBoolName) : base(fsm, character, animBoolName)
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
        
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
