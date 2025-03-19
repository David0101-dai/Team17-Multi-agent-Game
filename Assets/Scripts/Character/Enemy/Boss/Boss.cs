using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
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

        IdleState = new BossIdleState(Fsm, this, "Idle");
        PatrolState = new BossPatrolState(Fsm, this, "Move");
        ChaseState = new BossChaseState(Fsm, this, "Move");
        AttackState = new BossAttackState(Fsm, this, "Attack");
        HitState = new BossHitState(Fsm, this, "Hit");
        DeadState = new BossDeadState(Fsm, this, "Dead");
        StunState = new BossStunState(Fsm, this, "Stun");
        Fsm.SwitchState(IdleState);
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
        if (this.gameObject.name == "BigSkeleton")
        {
            PlayerManager.Instance.AddScore(4);
        }
        else
        {
            PlayerManager.Instance.AddScore(2);
        }
        Debug.Log(PlayerManager.finalscore);
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
