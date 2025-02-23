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

        // 检查和玩家之间的距离
        if (Character.DistanceBetweenPlayer > 5)
        {
            // 当距离大于 5 时切换到 Move 状态并追踪玩家
            Fsm.SwitchState(Character.MoveState);
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
