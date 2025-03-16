using System;
using UnityEngine;

public class NightBorneChaseState : NightBorneState
{
    public NightBorneChaseState(FSM fsm, NightBorne character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }


    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        StateTimer = Character.lostPlayerTime;
    }

    public override void Update()
    {
        base.Update();

        if (!ColDetect.DetectedPlayer)
        {
            Fsm.SwitchState(Character.IdleState);
            return;
        }

        if(ColDetect.IsWallDetected && !Character.CanAttack() ){
            Flip.Flip();
        }
        
        var isRight = ColDetect.DetectedPlayer.position.x > Character.transform.position.x;
        var isLeft = ColDetect.DetectedPlayer.position.x < Character.transform.position.x;
        var moveDir = isRight ? 1 : isLeft ? -1 : 0;

        var distance = Vector2.Distance(ColDetect.DetectedPlayer.position, Character.transform.position);
       
        if(!Character.canAttack){
            SetVelocity(moveDir * Character.moveSpeed * 2, Rb.velocity.y);
        }else{
            SetVelocity(0, Rb.velocity.y);
            if(attackCooldownTimer <= 0){
               Fsm.SwitchState(Character.AttackState);
            }
        }

        if (StateTimer < 0 || distance - 1 > ColDetect.playerCheckDistance)
        {
            ColDetect.DetectedPlayer = null;
            //Flip.Flip();
            Fsm.SwitchState(Character.IdleState);
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
