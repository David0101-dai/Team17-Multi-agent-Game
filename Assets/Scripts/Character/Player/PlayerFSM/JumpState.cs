using System.Diagnostics;
using System.Collections;
using UnityEngine;

public class JumpState : AirState
{
    public JumpState(FSM fsm, Player character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        AudioManager.instance.PlaySFX(0,null);

        if (!ColDetect.IsGrounded)
        {
            airJumpCounter++;
        }else{
            airJumpCounter = 0;
        }       

        if (airJumpCounter >= Character.airJumpCount+1)
        {
            return;
        }

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
