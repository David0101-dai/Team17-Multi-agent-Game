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
