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
    private float spikeDamageTimer = 0f;

    public PlayerState(FSM fsm, Player character, string animBoolName) : base(fsm, character, animBoolName)
    {
        Input = character.InputController;
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
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

        if (spikeDamageTimer > 0)
        {
            spikeDamageTimer -= Time.deltaTime;
        }

        if (spikeDamageTimer <= 0 && Physics2D.OverlapCircle(Character.transform.position, 0.2f, LayerMask.GetMask("Spikes")))
        {
            PlayerDamageable damageable = Character.GetComponent<PlayerDamageable>();
            if (damageable != null)
            {
                if(PlayerManager.Instance.isDead){
                     return;
                }
                damageable.currentHp -= 10;
                spikeDamageTimer = 1.0f;
                Fsm.SwitchState(Character.HitState);
                return;
            }
        }

        if (Character.GetComponent<PlayerDamageable>().currentHp <= 0)
        {
            Fsm.SwitchState(Character.DeadState);
        }
    }



    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }

    private void SetDashDir()
    {
        dashDir = Input.xAxis == 0 ? Flip.facingDir : Input.xAxis;
    }

    public IEnumerator BusyFor(float seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(seconds);
        isBusy = false;
    }
}
