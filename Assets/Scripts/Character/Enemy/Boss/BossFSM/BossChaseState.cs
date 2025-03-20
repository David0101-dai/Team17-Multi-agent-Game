using UnityEngine;

public class BossChaseState : BossState
{

    public BossChaseState(FSM fsm, Boss character, string animBoolName) : base(fsm, character, animBoolName)
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
            SetVelocity(direction * Character.moveSpeed * 1.4f, Rb.velocity.y);
        }else{
            if(attackCooldownTimer <= 0){
                SetVelocity(0, Rb.velocity.y);
                attackCooldownTimer = attackCooldown;
               Fsm.SwitchState(Character.AttackState);
            }else{
                Fsm.SwitchState(Character.IdleState);
            }
        }

        if ((StateTimer < 0 || distance - 1 > ColDetect.playerCheckDistance) && Character.BossStage != BossStage.stage4)
        {
            Fsm.SwitchState(Character.IdleState);
        }else{
            if(Character.BossStage == BossStage.stage4){
                Fsm.SwitchState(Character.TeleportState);
            }
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
