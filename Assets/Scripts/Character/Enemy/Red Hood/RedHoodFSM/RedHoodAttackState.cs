using UnityEngine;
using UnityEngine.TextCore.Text;

public class RedHoodAttackState : RedHoodBattleState
{
    public int comboCounter;
     private bool hasAimed = false;  

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
        hasAimed = false;
    }

    public override void Update()
    {
        base.Update();

         // 更新状态计时器
        StateTimer -= Time.deltaTime;

        // 如果状态计时器结束，但动画还没有播放完成，继续待在 AimState
        if (StateTimer <= 0 && !hasAimed)
        {
            // 停止敌人的移动
            SetVelocity(0, 0);
        }

        if (IsAnimationFinished && !hasAimed)
        {   
            hasAimed = true;  // 确保动画完成后才会切换到 IdleState
            if(Character.RedHoodType == RedHoodType.level1)
                Fsm.SwitchState(Character.IdleState);
            if(Character.RedHoodType == RedHoodType.level2) 
                Fsm.SwitchState(Character.ChaseState);
            if (Character.RedHoodType == RedHoodType.level3)
            {
                float randomValue = Random.value;

                // 70% 概率进入追击状态
                if (randomValue < 0.7f)
                {
                    Fsm.SwitchState(Character.ChaseState);
                }
                // 20% 概率进入冲刺状态
                else if (randomValue < 0.9f)
                {
                    Fsm.SwitchState(Character.DashState);
                }
                // 10% 概率进入射击状态
                else
                {
                    Fsm.SwitchState(Character.AimState);
                }
            } 
        }

        if (!ColDetect.IsGrounded && !hasAimed)
        {
             hasAimed = true;  // 确保动画完成后才会切换到 IdleState
            Fsm.SwitchState(Character.FallState);
        }

        if(hasAimed){
            Exit(Character.DashState);
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