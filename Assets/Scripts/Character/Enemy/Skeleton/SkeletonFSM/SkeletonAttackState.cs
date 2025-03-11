using System.Numerics;

public class SkeletonAttackState : SkeletonState
{
    public SkeletonAttackState(FSM fsm, Skeleton character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        if(PlayerManager.Instance.isDead){
            Fsm.SwitchState(Character.IdleState);
        }

        AudioManager.instance.PlaySFX(12, Character.transform);
    }

    public override void Update()
    {
        base.Update();

        if (IsAnimationFinished || !ColDetect.DetectedPlayer)
        {
            Fsm.SwitchState(Character.ChaseState);
            attackCooldownTimer = Character.attackCooldown;
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
