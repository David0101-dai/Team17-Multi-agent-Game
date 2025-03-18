using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RedHoodType {level1, level2, level3,level4}
public class RedHood : Enemy
{
    [Header("RedHood Specific Info")]
    [SerializeField] private RedHoodType RedHoodType;
    public Vector2 jumpForce = new Vector2(10, 10);
    public float jumpCooldown;
    [HideInInspector] public float jumpCooldownTimer;
    public float safeDistance = 5f;
    [SerializeField] private GameObject arrow;
    public float dashSpeed = 25f;
    public float dashDuration = 0.25f;
    [SerializeField] private float arrowSpeed = 10f;

    [Header("RedHood Check Info")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 groundCheckSize;


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
        jumpCooldownTimer = 0;
        IdleState = new RedHoodIdleState(Fsm, this, "Idle");
        PatrolState = new RedHoodPatrolState(Fsm, this, "Move");
        ChaseState = new RedHoodChaseState(Fsm, this, "Move");
        AttackState = new RedHoodAttackState(Fsm, this, "Attack");
        HitState = new RedHoodHitState(Fsm, this, "Hit");
        DeadState = new RedHoodDeadState(Fsm, this, "Dead");
        StunState = new RedHoodStunState(Fsm, this, "Stun");
        AimState = new RedHoodAimState(Fsm, this, "Aim");
        JumpState = new RedHoodJumpState(Fsm, this, "Jump");
        FallState = new RedHoodFallState(Fsm, this, "Fall");
        DashSTate = new RedHoodDashState(Fsm, this, "Dash");
        Fsm.SwitchState(IdleState);
    }

    protected override void Update()
    {
        base.Update();
        jumpCooldownTimer -= Time.deltaTime;
    }
    public bool GroundBhindCheck() => Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, LayerMask.GetMask("Platform"));
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        // 确保这里绘制的是你想要的地面检测盒子的大小和位置
        Gizmos.color = Color.red;  // 设置 Gizmo 颜色为红色
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
    }

    public override void AnimationSpecialAttackTrigger()
    {
        base.AnimationSpecialAttackTrigger();
        Debug.Log("Special Attack Triggered");
        GameObject newArrow = Instantiate(arrow,attackCheck.position, Quaternion.identity);
        newArrow.GetComponent<EnergyBall_Controller>().Setup(gameObject, Flip.facingDir*arrowSpeed);
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
