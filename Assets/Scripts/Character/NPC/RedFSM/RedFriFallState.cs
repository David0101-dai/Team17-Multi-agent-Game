using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFriFallState : RedFriAirState
{
    public RedFriFallState(FSM fsm, RedFriend character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
    }

    public override void Update()
    {
        base.Update();
        if (ColDetect.IsGrounded)
        {
            Fsm.SwitchState(Character.IdleState);
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}

