using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFriFollowState : RedFriGoundState
{
    private float randomExitTimer;  // 用于控制随机退出的定时器
    private float exitTime;  // 随机的退出时间（0.5 到 1.5 秒之间）
    
    private Vector3 followDirection;  // 确定的跟随方向
    private bool isDirectionSet = false;  // 标记是否已经确定了跟随方向

    public RedFriFollowState(FSM fsm, RedFriend character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
        
        // 随机生成退出时间
        exitTime = Random.Range(0.5f, 1.5f);
        randomExitTimer = 0f;  // 重置定时器

        // 确定跟随方向，只在第一次进入FollowState时执行
        if (!isDirectionSet)
        {
            Vector3 playerPosition = PlayerManager.Instance.player.transform.position;
            followDirection = (playerPosition - Character.transform.position).normalized;
            isDirectionSet = true;  // 标记方向已经确定
        }
      //  Debug.Log("RedFriend进入伴随状态");
    }

    public override void Update()
    {
        base.Update();

        // 每帧更新定时器
        randomExitTimer += Time.deltaTime;
        
        // 如果定时器到期，切换到 Idle 状态
        if (randomExitTimer >= exitTime)
        {
            Fsm.SwitchState(Character.IdleState);  // 切换到 Idle 状态
        }
        
        // 如果方向已经确定，就沿着这个方向持续移动
        if (isDirectionSet)
        {
            // 继续超越玩家
            MoveInDirection(followDirection);
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
        isDirectionSet = false;
        //Debug.Log("RedFriend离开伴随状态");
    }

    // 向固定方向移动的方法
    private void MoveInDirection(Vector3 direction)
    {
        float moveSpeed = Character.defaultMoveSpeed;
        // 按给定的方向继续移动
        SetVelocity(direction.x * moveSpeed, 0);
    }
}
