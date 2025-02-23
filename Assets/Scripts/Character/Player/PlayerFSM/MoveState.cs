public class MoveState : GroundState
{
    public MoveState(FSM fsm, Player character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        AudioManager.instance.PlaySFX(1,null); //走路音效
    }

    public override void Update()
    {
        base.Update();

        SetVelocity(Input.xAxis * Character.moveSpeed, Rb.velocity.y);

        if (Input.xAxis == 0)
        {
            Fsm.SwitchState(Character.IdleState);
        }
    }

    public override void Exit(IState newState)
    {
        AudioManager.instance.StopSFX(1); //挥刀砍 音效退出
        base.Exit(newState);
    }
}
