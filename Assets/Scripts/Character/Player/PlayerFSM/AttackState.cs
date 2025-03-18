using UnityEngine;
using UnityEngine.TextCore.Text;

public class AttackState : PlayerState
{
    private float lastAttackTime;
    public int comboCounter;

    public AttackState(FSM fsm, Player character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        AudioManager.instance.PlaySFX(2, null); 

        if (comboCounter >= Character.comboCount || Time.time >= lastAttackTime + Character.comboWindow)
        {
            comboCounter = 0;
        }

        Anim.SetInteger("ComboCounter", comboCounter);
        Anim.speed = Character.attackSpeed;

        var attackDir = Input.xAxis == 0 ? Flip.facingDir : Input.xAxis;
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
        // 如果玩家处于狂战士状态，则调用其 OnAttack 方法实现回血
        // 定义一个攻击检测的半径和敌人所在的 LayerMask
    float attackRadius = 1f; // 根据实际需求调整
    LayerMask enemyLayer = LayerMask.GetMask("Enemy"); // 确保敌人放在 "Enemy" 层
     // 在玩家当前位置做一个圆形检测
    Collider2D hitCollider = Physics2D.OverlapCircle(Character.transform.position, attackRadius, enemyLayer);
    if (hitCollider != null)
    {
        // 如果检测到了敌人，则说明攻击命中，调用狂战士buff的回血方法
        BerserkerBuff buff = Character.GetComponent<BerserkerBuff>();
            if (buff != null)
            {
                buff.OnAttack();
            }
    }

    Character.StartCoroutine(BusyFor(0.15f));

    Anim.speed = 1;

    comboCounter++;
    lastAttackTime = Time.time;
}

}
