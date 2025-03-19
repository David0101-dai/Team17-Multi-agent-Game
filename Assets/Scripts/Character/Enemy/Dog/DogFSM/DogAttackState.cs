using System.Numerics;
using UnityEngine;

public class DogAttackState : DogState
{
    public DogAttackState(FSM fsm, Dog character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
        SetVelocity(Flip.facingDir * Character.moveSpeed*2f, 10);
    }

    public override void Update()
    {
        base.Update();

        if (IsAnimationFinished || !ColDetect.DetectedPlayer )
        {              
            Fsm.SwitchState(Character.IdleState);
            attackCooldownTimer = Character.attackCooldown;
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
