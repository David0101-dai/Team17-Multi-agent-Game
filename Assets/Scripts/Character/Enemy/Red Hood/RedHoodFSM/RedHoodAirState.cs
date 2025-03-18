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

        if(Rb.velocity.y == 0){
            Fsm.SwitchState(Character.IdleState);
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
