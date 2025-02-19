using UnityEngine;

public class NightBorneState : CharacterState<NightBorne>, IState
{
    protected static float attackCooldownTimer;

    public NightBorneState(FSM fsm, NightBorne character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
    }

    public override void Update()
    {
        base.Update();

        attackCooldownTimer -= Time.deltaTime;
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
