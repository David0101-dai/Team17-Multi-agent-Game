public class RedHoodFallState : RedHoodAirState
{
    public RedHoodFallState(FSM fsm, RedHood character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
    }

    public override void Update()
    {
        base.Update();

        if (ColDetect.IsGrounded)
        {
            Fsm.SwitchState(Character.LandState);
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
