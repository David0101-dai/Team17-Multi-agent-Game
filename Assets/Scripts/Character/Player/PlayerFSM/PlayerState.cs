using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerState : CharacterState<Player>
{
    private int airDashCounter;
    protected static bool isBusy;
    protected float dashDir;
    protected int deadCount = 1;
    protected InputController Input { get; private set; }

    private float spikeDamageTimer = 0f; // **地刺伤害计时器**

    public PlayerState(FSM fsm, Player character, string animBoolName) : base(fsm, character, animBoolName)
    {
        Input = character.InputController;
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
        //Debug.Log("I enter " + AnimBoolName);
        SetDashDir();
    }

    public override void Update()
    {
        base.Update();
        Anim.SetFloat("VelocityY", Rb.velocity.y);

        if (ColDetect.IsGrounded)
        {
            airDashCounter = 0;
        }

        if (Input.isDashDown
            && airDashCounter < Character.airDashCount
            && SkillManager.Instance.Dash.CanUseSkill()
            && !ColDetect.IsWallDetected && Character.dashUsageTimer < 0)
        {
            if (!ColDetect.IsGrounded)
            {
                airDashCounter++;
            }
            SetDashDir();
            Fsm.SwitchState(Character.DashState);
            Character.dashUsageTimer = Character.dashCooldown;
        }

        // **更新地刺伤害计时器**
        if (spikeDamageTimer > 0)
        {
            spikeDamageTimer -= Time.deltaTime;
        }

        // **检测是否碰到地刺，并限制伤害频率**
        if (spikeDamageTimer <= 0 && Physics2D.OverlapCircle(Character.transform.position, 0.2f, LayerMask.GetMask("Spikes")))
        {
            PlayerDamageable damageable = Character.GetComponent<PlayerDamageable>();
            if (damageable != null)
            {
                damageable.currentHp -= 10; // **每次受伤减少 10 点血量**
                spikeDamageTimer = 1.0f; // **设置 1 秒的冷却时间**

                // **切换到 HitState**
                Fsm.SwitchState(Character.HitState);
                return;
            }
        }

        // **检测玩家血量是否归零，切换到 DeadState**
        if (Character.GetComponent<PlayerDamageable>().currentHp <= 0)
        {
            Fsm.SwitchState(Character.DeadState);
        }
    }



    public override void Exit(IState newState)
    {
        base.Exit(newState);
        //Debug.Log("I exit "+ AnimBoolName);
    }

    private void SetDashDir()
    {
        dashDir = Input.xAxis == 0 ? Flip.facingDir : Input.xAxis;
    }

    public IEnumerator BusyFor(float seconds)
    {
        isBusy = true;
        //Debug.Log("IS BUSY");

        //await Task.Delay((int)(seconds * 1000));
        yield return new WaitForSeconds(seconds);

        //Debug.Log("NOT BUSY");
        isBusy = false;
    }
}
