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

        if(ColDetect.DetectedPlayer != null){
        var isRight = ColDetect.DetectedPlayer.position.x > Character.transform.position.x;
        var isLeft = ColDetect.DetectedPlayer.position.x < Character.transform.position.x;
        var moveDir = isRight ? 1 : isLeft ? -1 : 0;
         dashDir = moveDir;
        }

    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}