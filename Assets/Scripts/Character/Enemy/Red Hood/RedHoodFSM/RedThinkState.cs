using UnityEngine;
public class RedHoodThinkState : RedHoodState
{    protected RedHood enemy;
    protected Transform Player;

    public RedHoodThinkState(FSM fsm, RedHood character, string animBoolName) : base(fsm, character, animBoolName)
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
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
