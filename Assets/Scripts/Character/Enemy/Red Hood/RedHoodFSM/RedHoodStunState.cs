using UnityEngine;

public class RedHoodStunState : RedHoodState
{
    public RedHoodStunState(FSM fsm, RedHood character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
        
        if(Character.RedHoodType == RedHoodType.level4){
            StateTimer = Character.stunTime*0.5f;
        }else{
            StateTimer = Character.stunTime;
        }       

        Character.FlashFX.RedBlink(true);

        if (Character.damageFrom)
        {
            var isRight = Character.damageFrom.transform.position.x > Character.transform.position.x;
            var isLeft = Character.damageFrom.transform.position.x < Character.transform.position.x;
            var faceDir = isRight ? 1 : isLeft ? -1 : 0;
            SetVelocity(faceDir * -1 * Character.knockbackXSpeed, Character.knockbackYSpeed, false);
            if (faceDir != 0 && faceDir != Flip.facingDir) Flip.Flip();
        }
    }

    public override void Update()
    {
        base.Update();
        if (StateTimer < 0){
        if(Character.RedHoodType == RedHoodType.level4){
            Fsm.SwitchState(Character.ChaseState);
        }            
        Fsm.SwitchState(Character.ChaseState);
        }
            
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);

        Character.FlashFX.RedBlink(false);
    }
}
