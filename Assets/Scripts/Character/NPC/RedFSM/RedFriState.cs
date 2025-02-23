using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFriState : CharacterState<RedFriend>, IState
{
    protected static float attackCooldownTimer;
    public RedFriState(FSM fsm, RedFriend character, string animBoolName) : base(fsm, character, animBoolName)
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
        attackCooldownTimer -= Time.deltaTime;
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
