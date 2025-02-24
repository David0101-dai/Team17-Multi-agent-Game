using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFriJumpState : RedFriAirState
{
    public RedFriJumpState(FSM fsm, RedFriend character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

     public override void Enter(IState lastState)
    {
        base.Enter(lastState);
        SetVelocity(Rb.velocity.x, Character.defaultJumpForce);
    }

    public override void Update()
    {
        base.Update();

        if (Rb.velocity.y <= 0)
        {
            Fsm.SwitchState(Character.FallState);
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
