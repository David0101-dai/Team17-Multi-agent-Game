
using UnityEngine;

public class BlackholeState : PlayerState
{
    private float flyTime = 0.3f;
    private bool skillUsed;
    public float blackholeCost = 50f;

    public BlackholeState(FSM fsm, Player character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        if (SkillManager.Instance.Blackhole.blackHole)
        {
            if (!MagicManager.Instance.ConsumeMagic(blackholeCost))
            {
                Debug.Log("魔法不足，无法释放冲刺技能！");
                Fsm.SwitchState(Character.IdleState);
                return;
            }
        }

        if (!SkillManager.Instance.Blackhole.blackHole)
        {
            Fsm.SwitchState(Character.IdleState);
            return;
        }

        AudioManager.instance.PlaySFX(7,null); //大招音效

        skillUsed = false;
        StateTimer = flyTime;
        Rb.gravityScale = 0;
    }

    public override void Update()
    {
        base.Update();

        if (Character.Skill.Blackhole.BlackholeFinished())
        {
            Fsm.SwitchState(Character.FallState);
            return;
        }

        if (StateTimer > 0)
        {
            Rb.velocity = new Vector2(0, 15);
        }
        else
        {
            Rb.velocity = new Vector2(0, -0.1f);
            if (!skillUsed)
            {
                if (!Character.Skill.Blackhole.CanUseSkill()) return;
                skillUsed = true;
            }
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);

        Rb.gravityScale = 1;
        Character.MakeTransprent(false);
    }
}
