using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFriMoveState : RedFriGoundState
{
    public RedFriMoveState(FSM fsm, RedFriend character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
    }

    public override void Update()
    {
        base.Update();

        // 追踪玩家
        MoveTowardsPlayer();

        // 如果和玩家的距离小于或等于5，返回 Idle 状态
        if (Character.DistanceBetweenPlayer <= 5)
        {
            Fsm.SwitchState(Character.IdleState);
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }

    // 向玩家移动的方法
    private void MoveTowardsPlayer()
    {
        if (PlayerManager.Instance != null && PlayerManager.Instance.fx != null)
        {
            Vector3 playerPosition = PlayerManager.Instance.player.transform.position;
            Vector3 direction = (playerPosition - Character.transform.position).normalized;

            // 设置移动的速度
            float moveSpeed = Character.defaultMoveSpeed;
            SetVelocity(direction.x * moveSpeed, Character.Rb.velocity.y);
        }
    }
}
