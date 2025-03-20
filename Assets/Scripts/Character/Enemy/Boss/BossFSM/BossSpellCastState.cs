using UnityEngine;
public class BossSpellCastState : BossState
{
    public BossSpellCastState(FSM fsm, Boss character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    private int amountOfSpells;
    private float spellCoolDown;
    private float spellCoolDownTimer = 0;

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
        Character.ResetSpellCoolDown();
        amountOfSpells = Character.amountOfSpells;
        spellCoolDown = Character.spellCoolDown;
        StateTimer = 5f;
    }

    public override void Update()
    {
        base.Update();

        spellCoolDownTimer -= Time.deltaTime;
        
        if(spellCoolDownTimer <= 0 && amountOfSpells > 0){
            Character.SpellCast();
            spellCoolDownTimer = spellCoolDown;
            amountOfSpells--;
        }

        if (amountOfSpells <= 0)
        {
            Fsm.SwitchState(Character.TeleportState);
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
