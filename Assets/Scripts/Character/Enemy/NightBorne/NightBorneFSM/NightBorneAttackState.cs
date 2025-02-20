using System.Numerics;
using UnityEngine;

public class NightBorneAttackState : NightBorneState
{
    public NightBorneAttackState(FSM fsm, NightBorne character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
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
