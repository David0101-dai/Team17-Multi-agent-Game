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
               // Debug.Log("ħ�����㣬�޷��ͷų�̼��ܣ�");
                Fsm.SwitchState(Character.IdleState);
                return;
            }
        }

        if (!SkillManager.Instance.Dash.dashUnlocked)
        {
            Debug.Log("Dash ����δ�������޷���̣�");
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


    /// <summary>
    /// ���弼���߼����˷���ֻ����ħ���㹻ʱ�Żᱻ����
    /// </summary>
    private void ExecuteDashEffect()
    {
        Debug.Log("��̼��ܼ��");
        // ���������Ӿ���ĳ��Ч������
    }


    public override void Update()
    {
        base.Update();
        Character.FlashFX.CreatAfterImage();
        Debug.Log("特效");
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
