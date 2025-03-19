using UnityEngine;

public class DogState : CharacterState<Dog>, IState
{
    protected static float attackCooldownTimer;

    public DogState(FSM fsm, Dog character, string animBoolName) : base(fsm, character, animBoolName)
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
