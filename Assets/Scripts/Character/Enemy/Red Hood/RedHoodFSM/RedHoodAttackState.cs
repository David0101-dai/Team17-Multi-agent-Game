using UnityEngine;
using UnityEngine.TextCore.Text;

public class RedHoodAttackState : RedHoodBattleState
{
    public int comboCounter;

    public RedHoodAttackState(FSM fsm, RedHood character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        if (comboCounter >= Character.attackMovement.Length)
        {
            comboCounter = 0;  // 或者重置为某个合理的值
        }

        if (comboCounter >= Character.comboCount)
        {
            comboCounter = 0;
        }

        Debug.Log("Character.comboCount: " + Character.comboCount);

        Anim.SetInteger("AttackCounter", comboCounter);

        Anim.speed = Character.attackSpeed;

        var isRight = ColDetect.DetectedPlayer.position.x > Character.transform.position.x;
        var isLeft = ColDetect.DetectedPlayer.position.x < Character.transform.position.x;
        var attackDir = isRight ? 1 : isLeft ? -1 : 0;

        SetVelocity(Character.attackMovement[comboCounter].x * attackDir, Character.attackMovement[comboCounter].y);

        StateTimer = 0.1f;
    }

    public override void Update()
    {
        base.Update();

        if (StateTimer < 0)
        {
            SetVelocity(0, 0);
        }

        if (IsAnimationFinished)
        {
            Fsm.SwitchState(Character.IdleState);
        }

        if (!ColDetect.IsGrounded)
        {
            Fsm.SwitchState(Character.FallState);
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
        Anim.speed = 1;
        comboCounter = Mathf.Min(comboCounter, Character.comboCount-1);  // 这样comboCounter最大为5
        comboCounter++;
    }

}