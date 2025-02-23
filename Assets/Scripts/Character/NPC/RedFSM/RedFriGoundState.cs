using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFriGoundState : RedFriState
{
    public RedFriGoundState(FSM fsm, RedFriend character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

   public override void Enter(IState lastState)
    {
        base.Enter(lastState);
    }

    public override void Update()
    {
        base.Update();

        // if (Fsm.CurrentState != Character.MoveState && ColDetect.DetectedPlayer)
        //     Fsm.SwitchState(Character.MoveState);
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
