using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFriIdleState : RedFriGoundState
{
    public RedFriIdleState(FSM fsm, RedFriend character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
        SetVelocity(0, 0);
        StateTimer = 1f;
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
