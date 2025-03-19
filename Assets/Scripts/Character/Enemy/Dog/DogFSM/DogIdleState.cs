public class DogIdleState : DogGroundState
{
    public DogIdleState(FSM fsm, Dog character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        SetVelocity(0, 0);

        StateTimer = 1f;
    }

    public override void Update()
    {
        base.Update();
        SetVelocity(Flip.facingDir * Character.moveSpeed*0.5f, Rb.velocity.y);
        if (StateTimer < 0)
            Fsm.SwitchState(Character.PatrolState);
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
