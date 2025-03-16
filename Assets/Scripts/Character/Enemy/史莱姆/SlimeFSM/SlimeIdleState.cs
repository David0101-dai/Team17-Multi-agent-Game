public class SlimeIdleState : SlimeGroundState
{
    public SlimeIdleState(FSM fsm, Slime character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        AudioManager.instance.PlaySFX(12,  Character.transform);

        SetVelocity(0, 0);

        StateTimer = 1f;
    }

    public override void Update()
    {
        base.Update();

        if (StateTimer < 0)
            Fsm.SwitchState(Character.PatrolState);
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
