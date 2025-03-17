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

    public override void Die()
    {
        throw new System.NotImplementedException();
    }

    public override void SlowBy(float slowPercentage, float slowDuration)
    {
        throw new System.NotImplementedException();
    }

    protected override bool IsInStunState()
    {
        throw new System.NotImplementedException();
    }

    protected override void SwitchHitState()
    {
        throw new System.NotImplementedException();
    }

    protected override void SwitchStunState()
    {
        throw new System.NotImplementedException();
    }
    #endregion
}
