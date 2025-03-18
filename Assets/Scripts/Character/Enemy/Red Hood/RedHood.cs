using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHood : Enemy
{
    #region State
    public IState IdleState { get; private set; }
    public IState PatrolState { get; private set; }
    public IState ChaseState { get; private set; }
    public IState AttackState { get; private set; }
    public IState HitState { get; private set; }
    public IState DeadState { get; private set; }
    public IState StunState { get; private set; }
    public IState JumpState {get; private set; }
    public IState FallState {get; private set; }
    public IState LandState {get; private set; }
    public IState DashSTate {get; private set; }
    public IState AimState {get; private set; }
    #endregion
    
    [Header("Attack Value")]
    public int comboCount = 6;
    public float comboWindow = 0.25f;
    public float attackSpeed = 1;
    public Vector2[] attackMovement ={
        new Vector2(2,0),
        new Vector2(2,0),
        new Vector2(2,0),
        new Vector2(3,0),
        new Vector2(3,0),
        new Vector2(5,0),
    };

        protected override void Start()
    {
        base.Start();

        IdleState = new RedHoodIdleState(Fsm, this, "Idle");
        PatrolState = new RedHoodPatrolState(Fsm, this, "Move");
        ChaseState = new RedHoodChaseState(Fsm, this, "Move");
        AttackState = new RedHoodAttackState(Fsm, this, "Attack");
        HitState = new RedHoodHitState(Fsm, this, "Hit");
        DeadState = new RedHoodDeadState(Fsm, this, "Dead");
        StunState = new RedHoodStunState(Fsm, this, "Stun");

        Fsm.SwitchState(IdleState);
    }

    protected override void Update()
    {
        base.Update();

        Damageable.OnTakeDamage += (from, to) =>
        {
            damageFrom = from;
            Fsm.SwitchState(HitState);
        };

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
