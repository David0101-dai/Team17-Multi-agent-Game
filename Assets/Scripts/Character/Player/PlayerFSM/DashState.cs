using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class DashState : PlayerState
{
    public DashState(FSM fsm, Player character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }
    public float dashCost = 5f;
    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        if (SkillManager.Instance.Dash.dashUnlocked)
        {
            if (!MagicManager.Instance.ConsumeMagic(dashCost))
            {
                Debug.Log("魔法不足，无法释放冲刺技能！");
                Fsm.SwitchState(Character.IdleState);
                return;
            }
        }

        if (!SkillManager.Instance.Dash.dashUnlocked)
        {
            Debug.Log("Dash 技能未解锁，无法冲刺！");
            Fsm.SwitchState(Character.IdleState);
            return;
        }

        AudioManager.instance.PlaySFX(4,null);

        StateTimer = Character.dashDuration;

        ExecuteDashEffect();

        if (SkillManager.Instance.Dash.cloneOnDashUnlocked)
        {
            Character.Skill.Clone.CreateCloneOnDashStart(Character.transform);
        }
    }


    /// <summary>
    /// 具体技能逻辑，此方法只有在魔法足够时才会被调用
    /// </summary>
    private void ExecuteDashEffect()
    {
        Debug.Log("冲刺技能激活！");
        // 在这里添加具体的冲刺效果代码
    }


    public override void Update()
    {
        base.Update();
        Character.FlashFX.CreatAfterImage();
        if (!ColDetect.IsGrounded && ColDetect.IsWallDetected)
        {
            Fsm.SwitchState(Character.WallSlideState);
        }

        SetVelocity(dashDir * Character.dashSpeed, 0);

        if (StateTimer <= 0)
        {
            Fsm.SwitchState(Character.IdleState);
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);

        if (SkillManager.Instance.Dash.cloneOnDashUnlocked)
        {
            Character.Skill.Clone.CreateCloneOnDashOver(Character.transform);
        }
    }
}
