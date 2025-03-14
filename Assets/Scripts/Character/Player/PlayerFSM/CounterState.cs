using UnityEngine;
using UnityEngine.VFX;
using System.Collections;
using Cinemachine;
public class CounterState : PlayerState
{
    private bool canCreateClone = false;
    private bool hasDefenseBuff = false;
    private CinemachineVirtualCamera counterCamera;

    public CounterState (FSM fsm, Player character, string animBoolName, CinemachineVirtualCamera counterCamera)
        : base(fsm, character, animBoolName)
    {
        this.counterCamera = counterCamera;  // 初始化镜头
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
        Character.Damageable.MakeInvincible(true);
        if (hasDefenseBuff){
        Collider2D[] colliders = Physics2D.OverlapCircleAll(Character.attackCheck.position, Character.attackCheckRadius);
         // 记录当前的镜头大小
        float initialOrthographicSize = counterCamera.m_Lens.OrthographicSize;
        float targetOrthographicSize = 5f;
        float lerpSpeed = 200f; // 控制镜头过渡的速度，可以根据需要调整

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<Enemy>().CanBeStun())
                {

                    StateTimer = 10;
                    PlayerManager.Instance.AddScore(3);
                    // 使用 Lerp 来平滑过渡镜头的 OrthographicSize
                    counterCamera.m_Lens.OrthographicSize = Mathf.Lerp(initialOrthographicSize, targetOrthographicSize, lerpSpeed * Time.deltaTime);
                
                    // 激活特写镜头
                    Anim.SetBool("CounterSuccess", true);
                    counterCamera.Priority = 50;
                    Time.timeScale = 0.2f;
                   
                    if (canCreateClone)
                    {
                        Character.Skill.Clone.CreateCloneOnCounterAttack(hit.transform);
                        canCreateClone = false;
                    }

                    if (hit.TryGetComponent(out Damageable to))
                    {
                       to.TakeDamage(Character.gameObject);
                    }
                }
            }
        }
        }
        
        if (StateTimer < 0 || IsAnimationFinished)
        {
            Fsm.SwitchState(Character.IdleState);
        }
    }
    public override void Exit(IState newState)
    {
        base.Exit(newState);
        Character.Damageable.MakeInvincible(false);
        Time.timeScale = 1;
        counterCamera.Priority = 10; // 恢复默认镜头
        counterCamera.m_Lens.OrthographicSize = 7.73f;
    }
}
