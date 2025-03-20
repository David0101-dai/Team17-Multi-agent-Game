using Unity.VisualScripting;
using UnityEngine;
public class BossTeleportState : BossState
{
    public BossTeleportState(FSM fsm, Boss character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
        StateTimer = 3f;
        Character.Damageable.MakeInvincible(true);
    }

    public override void Update()
    {
        base.Update();    
        if (IsAnimationFinished)
        {   
            if(Character.CanSpellCast()){
                Fsm.SwitchState(Character.SpellCastState);
            }else{
                Fsm.SwitchState(Character.ChaseState);
            }
        }
    }

    public override void Exit(IState newState)
    {
        Character.Damageable.MakeInvincible(false);
        base.Exit(newState);
    }
}
