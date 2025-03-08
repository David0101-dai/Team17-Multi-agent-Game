using UnityEngine;

public class CounterState : PlayerState
{
    private bool canCreateClone = false;
    private bool hasDefenseBuff = false;


   
    public CounterState(FSM fsm, Player character, string animBoolName)
        : base(fsm, character, animBoolName)
    {
        
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        // 1级：解锁counters
        if (!SkillManager.Instance.Counter.counterUnlocked)
        {
            Fsm.SwitchState(Character.IdleState);
            return;
        }

        // 2级：解锁counterBuff
        hasDefenseBuff = SkillManager.Instance.Counter.counterBuffUnlocked;

        // 3级：解锁counterMirage
        canCreateClone = SkillManager.Instance.Counter.counterMiragelUnlocked;

        StateTimer = Character.counterDuration;
        Anim.SetBool("CounterSuccess", false);
    }

    public override void Update()
    {
        base.Update();

        SetVelocity(0, 0);

        // 在防御期间设置角色无敌
        Character.isInvincible = true;

        if (hasDefenseBuff)
        {
            Collider2D[] coliders = Physics2D.OverlapCircleAll(Character.attackCheck.position, Character.attackCheckRadius);

            foreach (var hit in coliders)
            {
                if (!hit.CompareTag("Enemy")) continue;

                var enemy = hit.GetComponent<Enemy>();

                if (!enemy || !enemy.CanBeStun()) continue;

                // 触发反击效果
                StateTimer = 10;
                Anim.SetBool("CounterSuccess", true);

                if (canCreateClone)
                {
                    Character.Skill.Clone.CreateCloneOnCounterAttack(hit.transform);
                    canCreateClone = false;
                }

                if (!hit.TryGetComponent(out Damageable to)) return;
                to.TakeDamage(Character.gameObject);
            }
        }

        if (StateTimer < 0 || IsAnimationFinished)
        {
            Fsm.SwitchState(Character.IdleState);
        }
    }

    public override void Exit(IState newState)
    {
        Character.isInvincible = false;
        base.Exit(newState);
    }
}
