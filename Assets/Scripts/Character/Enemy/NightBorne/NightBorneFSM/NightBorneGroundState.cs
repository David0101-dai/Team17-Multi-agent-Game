public class NightBorneGroundState : NightBorneState
{
    public NightBorneGroundState(FSM fsm, NightBorne character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
    }

    public override void Update()
    {
        base.Update();

        if (Fsm.CurrentState != Character.ChaseState && ColDetect.DetectedPlayer)
            Fsm.SwitchState(Character.ChaseState);
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
