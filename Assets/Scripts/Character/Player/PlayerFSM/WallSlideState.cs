public class WallSlideState : PlayerState
{
    public WallSlideState(FSM fsm, Player character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        AudioManager.instance.PlaySFX(5,null); //下滑音效
    }

    public override void Update()
    {
        base.Update();

        SetVelocity(0, Input.yAxis < 0 ? Rb.velocity.y : Rb.velocity.y * Character.slideSpeed);

        var x = Input.xAxis == 0 ? 0 : Input.xAxis > 0 ? 1 : -1;
        if (ColDetect.IsGrounded || (Input.xAxis != 0 && Flip.facingDir != x))
        {
            Fsm.SwitchState(Character.IdleState);
        }

        if (Input.isJumpDown)
        {
            Fsm.SwitchState(Character.WallJumpState);
        }

        if(!Character.ColDetect.IsWallDetected){
            Fsm.SwitchState(Character.FallState);
        }
    }

    public override void Exit(IState newState)
    {
        AudioManager.instance.StopSFX(5); //下滑音效停止

        base.Exit(newState);
    }
}
