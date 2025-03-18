using UnityEngine;

public class RedHoodChaseState : RedHoodBattleState
{
    public RedHoodChaseState(FSM fsm, RedHood character, string animBoolName) : base(fsm, character, animBoolName)
    {}

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
       
        if(!Character.canAttack&& Character.GetDistanceToPlayer() > 4*Character.safeDistance){
            SetVelocity(moveDir * Character.moveSpeed, Rb.velocity.y);
        }else if(ColDetect.DetectedPlayer && canJump() && Character.GetDistanceToPlayer() < Character.safeDistance){
            Fsm.SwitchState(Character.JumpState);
        }else if(ColDetect.DetectedPlayer && Character.GetDistanceToPlayer() > 2*Character.safeDistance){
            Fsm.SwitchState(Character.AimState);
        }else if(Character.canAttack){
            SetVelocity(0, Rb.velocity.y);
            Fsm.SwitchState(Character.AttackState);
        }else{
            SetVelocity(moveDir * Character.moveSpeed, Rb.velocity.y);
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

    private bool canJump()
    {
        return Character.jumpCooldownTimer <= 0 && Character.GroundBhindCheck();
    }

}
