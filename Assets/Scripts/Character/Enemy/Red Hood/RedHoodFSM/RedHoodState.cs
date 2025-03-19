using UnityEngine;

public class RedHoodState : CharacterState<RedHood>, IState
{
    protected static float attackCooldownTimer;
    protected float dashDir;

    public RedHoodState(FSM fsm, RedHood character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
        //Debug.Log("RedHoodState Enteer"+ Fsm.CurrentState.GetType().Name);
    }

    public override void Update()
    {
        base.Update();
        attackCooldownTimer -= Time.deltaTime;
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
