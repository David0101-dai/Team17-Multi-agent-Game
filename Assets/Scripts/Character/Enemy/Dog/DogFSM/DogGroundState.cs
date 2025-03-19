using UnityEngine;
public class DogGroundState : DogState
{
    private float flipCooldownTimer = 0f;  // 反转冷却计时器
    private const float flipCooldown = 1.0f;  // 反转冷却时间（0.5秒）
    public DogGroundState(FSM fsm, Dog character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
    }

    public override void Update()
    {
        base.Update();

        // 每帧更新冷却计时器
        flipCooldownTimer -= Time.deltaTime;

        // 检查是否需要反转
        if ((ColDetect.IsWallDetected || ColDetect.ShouldFlip) && flipCooldownTimer <= 0f)
        {
            Flip.Flip();
            flipCooldownTimer = flipCooldown;  // 重置冷却计时器
        }

        // 检查是否需要切换到追击状态
        if (Fsm.CurrentState != Character.ChaseState && ColDetect.DetectedPlayer && !PlayerManager.Instance.isDead)
        {
            Fsm.SwitchState(Character.ChaseState);
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
