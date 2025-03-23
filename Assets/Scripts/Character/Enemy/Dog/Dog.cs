using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : Enemy
{
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

        IdleState = new DogIdleState(Fsm, this, "Idle");
        PatrolState = new DogPatrolState(Fsm, this, "Move");
        ChaseState = new DogChaseState(Fsm, this, "Move");
        AttackState = new DogAttackState(Fsm, this, "Attack");
        HitState = new DogHitState(Fsm, this, "Hit");
        DeadState = new DogDeadState(Fsm, this, "Dead");
        StunState = new DogStunState(Fsm, this, "Stun");
        Fsm.SwitchState(IdleState);

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
