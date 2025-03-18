using UnityEngine;
public class RedHoodBattleState : RedHoodThinkState
{    
    public RedHoodBattleState(FSM fsm, RedHood character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
        Player = PlayerManager.Instance.player.transform;
    }

    public override void Update()
    {
        base.Update();

        if(ColDetect.DetectedPlayer == null){
            Fsm.SwitchState(Character.IdleState);
            return;
        }

    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}