public class DashState : PlayerState
{
    public DashState(FSM fsm, Player character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        if (!SkillManager.Instance.Dash.dashUnlocked)
        {
            Fsm.SwitchState(Character.IdleState);
            return;
        }

        AudioManager.instance.PlaySFX(4,null); //�����Ч

        StateTimer = Character.dashDuration;

        if (SkillManager.Instance.Dash.cloneOnDashUnlocked)
        {
            Character.Skill.Clone.CreateCloneOnDashStart(Character.transform);
        }

        //Character.Skill.Clone.CreateCloneOnDashStart(Character.transform);
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

        if (SkillManager.Instance.Dash.cloneOnDashUnlocked)
        {
            Character.Skill.Clone.CreateCloneOnDashOver(Character.transform);
        }

        //Character.Skill.Clone.CreateCloneOnDashOver(Character.transform);
    }
}
