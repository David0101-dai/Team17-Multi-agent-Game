using System.Collections;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(InputController))]
[RequireComponent(typeof(PlayerDamageable))]
public class Player : Character
{
    public CinemachineVirtualCamera counterCamera;
    //public static Player instance;
    #region Value
    [Header("Move Value")]
    [HideInInspector] public float moveSpeed;
    public float defaultMoveSpeed = 7f;

    [Header("Jump Value")]
    [HideInInspector] public float jumpForce;
    public float defaultJumpForce = 20f;
    public int airJumpCount = 2;
    public float swordReturnImpact = 3f;
    public GameObject UsedSword;

    [Header("Dash Value")]
    [HideInInspector] public float dashSpeed;
    public float defaultDashSpeed = 25f;
    public float dashDuration = 0.25f;
    public float dashUsageTimer;
    public float dashCooldown = 0.4f;
    public int airDashCount = 1;

    [Header("Wallslide Value")]
    public float slideSpeed = 0.5f;
    public float wallJumpXSpeed = 5f;

    [Header("Attack Value")]
    public int comboCount = 3;
    public float comboWindow = 0.25f;
    public float attackSpeed = 1;
    public Vector2[] attackMovement ={
        new Vector2(3,0),
        new Vector2(2,0),
        new Vector2(5,0),
    };

    [Header("Counter Value")]
    public float counterDuration = 0.2f;

    #endregion

    #region Component
    public InputController InputController { get; private set; }
    public Damageable Damageable { get; private set; }
    public FlashFX FlashFX { get; private set; }
    public SkillManager Skill {get; private set;}
    #endregion

    #region StateMachine
    public IState IdleState { get; private set; }
    public IState MoveState { get; private set; }
    public IState JumpState { get; private set; }
    public IState FallState { get; private set; }
    public IState DashState { get; private set; }
    public IState WallSlideState { get; private set; }
    public IState WallJumpState { get; private set; }
    public IState AttackState { get; private set; }
    public IState HitState { get; private set; }
    public IState DeadState { get; private set; }
    public IState CounterState { get; private set; }
    public IState AimSwordState { get; private set; }
    public IState CatchSwordState { get; private set; }
    public IState BlackholeState { get; private set; }
    #endregion
    protected override void Start()
    {
        base.Start();

        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
        
        //更方便获得技能
        Skill = SkillManager.Instance;
        InputController = GetComponent<InputController>();
        Damageable = GetComponent<Damageable>();
        FlashFX = GetComponent<FlashFX>();

        Damageable.OnTakeDamage += (from, to) =>
        {
            damageFrom = from;
            Fsm.SwitchState(HitState);
        };

        IdleState = new IdleState(Fsm, this, "Idle");
        MoveState = new MoveState(Fsm, this, "Move");
        JumpState = new JumpState(Fsm, this, "Jump");
        FallState = new FallState(Fsm, this, "Jump");
        DashState = new DashState(Fsm, this, "Dash");
        WallSlideState = new WallSlideState(Fsm, this, "WallSlide");
        WallJumpState = new WallJumpState(Fsm, this, "Jump");
        AttackState = new AttackState(Fsm, this, "Attack");
        HitState = new HitState(Fsm, this, "Hit");
        DeadState = new DeadState(Fsm, this, "Dead");
        CounterState = new CounterState(Fsm, this, "Counter", counterCamera);
        AimSwordState = new AimSwordState(Fsm, this, "AimSword");
        CatchSwordState = new CatchSwordState(Fsm, this, "CatchSword");
        BlackholeState = new BlackholeState(Fsm, this, "Jump");
        Fsm.SwitchState(IdleState);
    }

    protected override void Update()
    {
        base.Update();
        dashUsageTimer -= Time.deltaTime;

        //
        if (Input.GetKeyDown(KeyCode.Q) && Skill.Crystal.crystalUnlocked)
            Skill.Crystal.CanUseSkill();
        
        //
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Inventory.Instance.UsedFlask();
    }

    public void CatchSword()
    {
        Fsm.SwitchState(CatchSwordState);
        Destroy(UsedSword);
    }

    public override void Die()
    {
        if (PlayerManager.Instance.isDead)
        {
            return;
        }
        Debug.Log("dead");
        PlayerManager.Instance.isDead = true;
        Fsm.SwitchState(DeadState);
    }

    public override void SlowBy(float slowPercentage, float slowDuration)
    {
        StartCoroutine(Slow(slowPercentage, slowDuration));
        IEnumerator Slow(float slowPercentage, float slowDuration)
        {
            var slow = 1 - slowPercentage;
            Anim.speed = slow;
            moveSpeed *= slow;
            jumpForce *= slow;
            dashSpeed *= slow;
            yield return new WaitForSeconds(slowDuration);
            Anim.speed = 1;
            moveSpeed = defaultMoveSpeed;
            jumpForce = defaultJumpForce;
            dashSpeed = defaultDashSpeed;
        }
    }
}
