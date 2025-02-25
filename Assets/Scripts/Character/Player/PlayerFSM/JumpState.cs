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

<<<<<<< HEAD
        AudioManager.instance.PlaySFX(0,null); //ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Ð§
=======
        AudioManager.instance.PlaySFX(0,null); //ÆðÌøÒôÐ§
>>>>>>> f8abc43d55ba4e9991a89a5503be0e9b3111f1ad

        if (!ColDetect.IsGrounded)
        {
            airJumpCounter++;
        }else{
            airJumpCounter = 0;
        }       

        if (airJumpCounter >= Character.airJumpCount+1)
        {
            //UnityEngine.Debug.Log("return");
            return;
        }

        SetVelocity(Rb.velocity.x, Character.jumpForce);
        //UnityEngine.Debug.Log("Jump");

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
