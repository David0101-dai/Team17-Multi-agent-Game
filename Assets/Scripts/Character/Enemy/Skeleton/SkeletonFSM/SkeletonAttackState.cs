using System.Numerics;

public class SkeletonAttackState : SkeletonState
{
    public SkeletonAttackState(FSM fsm, Skeleton character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        AudioManager.instance.PlaySFX(12, Character.transform); //÷¼÷Ã½ÐÒôÐ§
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
