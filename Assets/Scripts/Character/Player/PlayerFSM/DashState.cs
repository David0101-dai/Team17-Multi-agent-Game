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
                Fsm.SwitchState(Character.IdleState);
                return;
            }
        }

        if (!SkillManager.Instance.Dash.dashUnlocked)
        {
            Fsm.SwitchState(Character.IdleState);
            return;
        }

        AudioManager.instance.PlaySFX(4,null);

        StateTimer = Character.dashDuration;
        Character.FlashFX.CreatAfterImage();
        //ExecuteDashEffect();

        if (SkillManager.Instance.Dash.cloneOnDashUnlocked)
        {
            Character.Skill.Clone.CreateCloneOnDashStart(Character.transform);
        }
    }

    private void ExecuteDashEffect()
    {
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
