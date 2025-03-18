using System.Diagnostics;
using System.Collections;
using UnityEngine;

public class RedHoodJumpState : RedHoodAirState
{
    public RedHoodJumpState(FSM fsm, RedHood character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        SetVelocity(Rb.velocity.x, Character.jumpForce);
    }

    public override void Update()
    {
        base.Update();

        if (Rb.velocity.y < 0)
        {
            Fsm.SwitchState(Character.FallState);
        }
    }

    public override void Exit(IState newState)  
    {
        base.Exit(newState);
    }
}
