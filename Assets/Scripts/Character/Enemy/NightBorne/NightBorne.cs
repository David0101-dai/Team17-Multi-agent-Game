using System.Collections;
using UnityEngine;

public class NightBorne : Enemy
{
    [Header("Self detonation")]
    public Transform detonationCheck;
    public float detonationCheckRadius;
    #region State
    public IState IdleState { get; private set; }
    public IState PatrolState { get; private set; }
    public IState ChaseState { get; private set; }
    public IState AttackState { get; private set; }
    public IState HitState { get; private set; }
    public IState DeadState { get; private set; }
    public IState StunState { get; private set; }
    #endregion

    protected override void Start()
    {
        base.Start();

        IdleState = new NightBorneIdleState(Fsm, this, "Idle");
        PatrolState = new NightBornePatrolState(Fsm, this, "Move");
        ChaseState = new NightBorneChaseState(Fsm, this, "Move");
        AttackState = new NightBorneAttackState(Fsm, this, "Attack");
        HitState = new NightBorneHitState(Fsm, this, "Hit");
        DeadState = new NightBorneDeadState(Fsm, this, "Dead");
        StunState = new NightBorneStunState(Fsm, this, "Stun");
        Fsm.SwitchState(IdleState);

        if (detonationCheck == null)
        {
            Debug.LogError("detonationCheck 没有正确赋值！");
            return;
        }

        var colliders = Physics2D.OverlapCircleAll(detonationCheck.position, detonationCheckRadius);

        // 重写 Damageable.OnTakeDamage 事件
        Damageable.OnTakeDamage += (from, to) =>
        {
            // 你可以在这里加入一些Boss特定的处理逻辑

            // 调用 AudioManager 播放音效
            AudioManager.instance.PlaySFX(6, null); // 替换为你想播放的音效名称
        };
    }

    protected override void Update()
    {
        base.Update();
        //Debug.Log("夜魔的Current State: " + Fsm.CurrentState.ToString());
    }

    protected  override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(detonationCheck.position, detonationCheckRadius);
    }

    protected override void SwitchHitState()
    {
        Fsm.SwitchState(HitState);
    }

    protected override void SwitchStunState()
    {
        Fsm.SwitchState(StunState);
    }

    protected override bool IsInStunState()
    {
        return Fsm.CurrentState == StunState;
    }

    public override void Die()
    {
        Fsm.SwitchState(DeadState);
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }

    public override void SlowBy(float slowPercentage, float slowDuration)
    {
        StartCoroutine(Slow(slowPercentage, slowDuration));
        IEnumerator Slow(float slowPercentage, float slowDuration)
        {
            var slow = 1 - slowPercentage;
            Anim.speed = slow;
            moveSpeed *= slow;
            yield return new WaitForSeconds(slowDuration);
            Anim.speed = 1;
            moveSpeed = defaultMoveSpeed;
        }
    }
}
