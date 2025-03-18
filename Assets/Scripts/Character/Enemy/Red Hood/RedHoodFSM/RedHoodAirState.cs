using UnityEngine;
using UnityEngine.TextCore.Text;

public class RedHoodAirState : RedHoodBattleState
{

    public RedHoodAirState(FSM fsm, RedHood character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
