using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFriAirState : RedFriState
{
    public RedFriAirState(FSM fsm, RedFriend character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}

