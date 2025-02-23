public class DashState : PlayerState
{
    public DashState(FSM fsm, Player character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        AudioManager.instance.PlaySFX(4,null); //�����Ч

        StateTimer = Character.dashDuration;

       Character.Skill.Clone.CreateCloneOnDashStart(Character.transform);
    }
    public override void Update()
    {
        base.Update();

        if (!ColDetect.IsGrounded && ColDetect.IsWallDetected)
        {
            Fsm.SwitchState(Character.WallSlideState);
        }

        SetVelocity(dashDir * Character.dashSpeed, 0);

        if (StateTimer <= 0)
        {
            Fsm.SwitchState(Character.IdleState);
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);

        Character.Skill.Clone.CreateCloneOnDashOver(Character.transform);
    }
}
