using UnityEngine;
using UnityEngine.TextCore.Text;

public class RedHoodAimState : RedHoodBattleState
{

    public RedHoodAimState(FSM fsm, RedHood character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        StateTimer = 0.1f;
    }

    public override void Update()
    {
        base.Update();

        if (StateTimer < 0)
        {
            SetVelocity(0, 0);
        }

        if (IsAnimationFinished)
        {
            Fsm.SwitchState(Character.IdleState);
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }

}