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

        if(ColDetect.IsWallDetected){
            Fsm.SwitchState(Character.JumpState);
        }
        
        if(Character.DistanceBetweenPlayer > Character.MaxDistance){
            Fsm.SwitchState(Character.MoveState);
        }
    
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }

    public void MoveTowardsPlayer()
    {
        if (PlayerManager.Instance != null && FxManager.Instance.fx != null)
        {
            Vector3 playerPosition = PlayerManager.Instance.player.transform.position;
            Vector3 direction = (playerPosition - Character.transform.position).normalized;

            // 设置移动的速度
            float moveSpeed = Character.defaultMoveSpeed;
            SetVelocity(direction.x * moveSpeed, 0);
        }
    }
}
