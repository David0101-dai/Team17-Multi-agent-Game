using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFricombatState : RedFriState
{

    public RedFricombatState(FSM fsm, RedFriend character, string animBoolName) : base(fsm, character, animBoolName)
    {
        
    }

    // Start is called before the first frame update
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
