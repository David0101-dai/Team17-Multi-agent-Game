using UnityEngine;
public class RedHoodFallState : RedHoodAirState
{
    public RedHoodFallState(FSM fsm, RedHood character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
    }

    public override void Update()
    {
        base.Update();
        
        if (ColDetect.IsGrounded)
        {   
            if(Character.RedHoodType == RedHoodType.level1){ 
                Fsm.SwitchState(Character.AimState);
            }else if(Character.RedHoodType == RedHoodType.level2){
                if (Random.value < 0.5f)
                {
                    Fsm.SwitchState(Character.AimState);
                }
                else
                {
                    Fsm.SwitchState(Character.ChaseState);
                }
            }else if(Character.RedHoodType == RedHoodType.level3){
                if (Random.value < 0.5f)
                {
                    Fsm.SwitchState(Character.AimState);
                }
                else
                {
                    Fsm.SwitchState(Character.DashState);
                }
            }else if(Character.RedHoodType == RedHoodType.level4){
                if (Random.value < 0.7f)
                {
                    Fsm.SwitchState(Character.AimState);
                }
                else
                {
                    Fsm.SwitchState(Character.DashState);
                }
            }
            else{
                Fsm.SwitchState(Character.AimState);
            }
           
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
