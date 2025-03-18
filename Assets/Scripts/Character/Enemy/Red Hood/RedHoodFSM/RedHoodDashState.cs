using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class RedHoodDashState :  RedHoodBattleState
{
    public RedHoodDashState(FSM fsm, RedHood character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }
    public float dashCost = 5f;
    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        StateTimer = Character.dashDuration;
        
        if(ColDetect.DetectedPlayer != null){
        var isRight = ColDetect.DetectedPlayer.position.x > Character.transform.position.x;
        var isLeft = ColDetect.DetectedPlayer.position.x < Character.transform.position.x;
        var moveDir = isRight ? 1 : isLeft ? -1 : 0;
         dashDir = moveDir;
        }
    }
    
    public override void Update()
    {
        base.Update();
       
        SetVelocity(dashDir * Character.dashSpeed, 0);

        if (StateTimer <= 0)
        {
            Fsm.SwitchState(Character.IdleState);
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
