using System;
using UnityEngine;

public class NightBorneChaseState : NightBorneState
{
    public NightBorneChaseState(FSM fsm, NightBorne character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    private bool canAttack = false;

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

        var isRight = ColDetect.DetectedPlayer.position.x > Character.transform.position.x;
        var isLeft = ColDetect.DetectedPlayer.position.x < Character.transform.position.x;
        var moveDir = isRight ? 1 : isLeft ? -1 : 0;

        var distance = Vector2.Distance(ColDetect.DetectedPlayer.position, Character.transform.position);
       
       
        if(canAttack == false){
            SetVelocity(moveDir * Character.moveSpeed * 2, Rb.velocity.y);
        }else{
            SetVelocity(0, Rb.velocity.y);
        }
        
        if (distance < Character.attackDistance)
        {   
            if(attackCooldownTimer > 0){
                Fsm.SwitchState(Character.IdleState);
            }else{
            canAttack = true;
            SetVelocity(0, Rb.velocity.y);
            Fsm.SwitchState(Character.AttackState);   
            }
        }else{
            canAttack = false;
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
