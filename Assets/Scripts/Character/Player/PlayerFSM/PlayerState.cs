using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerState : CharacterState<Player>
{
    private int airDashCounter;
    protected static bool isBusy;
    protected float dashDir;
    protected InputController Input { get; private set; }

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
        //Debug.Log("I'm in "+ AnimBoolName);
        Anim.SetFloat("VelocityY", Rb.velocity.y);

        if (ColDetect.IsGrounded)
        {
            airDashCounter = 0;
            
        }

        if (Input.isDashDown
            && airDashCounter < Character.airDashCount
            && SkillManager.Instance.Dash.CanUseSkill()
            && !ColDetect.IsWallDetected && Character.dashUsageTimer<0)
        {

            
            if (!ColDetect.IsGrounded)
            {
                airDashCounter++;
            }
            SetDashDir();
            Fsm.SwitchState(Character.DashState);


            Character.dashUsageTimer = Character.dashCooldown;
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
