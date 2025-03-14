using UnityEngine;

public class IdleState : GroundState
{
    public IdleState(FSM fsm, Player character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
        deadCount = 1;
        SetVelocity(0, 0);
    }

    public override void Update()
    {
        base.Update();
        Character.airJumpCount = 1;

        if (Input.xAxis != 0 && !isBusy)
        {
            Fsm.SwitchState(Character.MoveState);
        }

    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
