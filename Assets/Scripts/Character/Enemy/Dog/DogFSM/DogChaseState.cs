using System;
using UnityEngine;

public class DogChaseState : DogState
{
    public DogChaseState(FSM fsm, Dog character, string animBoolName) : base(fsm, character, animBoolName)
    {

    }
    
    private int direction;

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
        if (ColDetect.DetectedPlayer){
            var isRight = ColDetect.DetectedPlayer.position.x > Character.transform.position.x;
            var isLeft = ColDetect.DetectedPlayer.position.x < Character.transform.position.x;
            var moveDir = isRight ? 1 : isLeft ? -1 : 0;
            direction = moveDir;
        }
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
        
        var distance = Vector2.Distance(ColDetect.DetectedPlayer.position, Character.transform.position);
       
        if(!Character.canAttack){
            SetVelocity(direction * Character.moveSpeed * 2, Rb.velocity.y);
        }else{
            SetVelocity(0, Rb.velocity.y);
            if(attackCooldownTimer <= 0){
               Fsm.SwitchState(Character.AttackState);
            }
        }

        if (StateTimer < 0 || distance - 1 > ColDetect.playerCheckDistance)
        {
            ColDetect.DetectedPlayer = null;
            Fsm.SwitchState(Character.IdleState);
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
