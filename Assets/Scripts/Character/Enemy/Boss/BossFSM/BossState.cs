using JetBrains.Annotations;
using UnityEngine;

public class BossState : CharacterState<Boss>, IState
{
    protected static float attackCooldownTimer;
    protected float attackCooldown; 

    public BossState(FSM fsm, Boss character, string animBoolName) : base(fsm, character, animBoolName)
    {
        attackCooldown = character.attackCooldown; 
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
    }

    public override void Update()
    {
        base.Update();

        if(Input.GetKeyDown(KeyCode.V))
            Fsm.SwitchState(Character.TeleportState);

        attackCooldownTimer -= Time.deltaTime;
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
