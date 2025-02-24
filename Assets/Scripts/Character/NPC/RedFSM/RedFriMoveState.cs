using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFriMoveState : RedFriGoundState
{
    public RedFriMoveState(FSM fsm, RedFriend character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
    }

    public override void Update()
    {
        base.Update();
        // 追踪玩家
        MoveTowardsPlayer();

        //SetVelocity(Rb.velocity.x,0);

        if(Character.DistanceBetweenPlayer < Character.MaxDistance){
            Fsm.SwitchState(Character.FollowState);
        }

        if(ColDetect.IsWallDetected){
            Fsm.SwitchState(Character.JumpState);
        }

        if (!ColDetect.IsGrounded)
        {
            Fsm.SwitchState(Character.FallState);
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }

}
