using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFriend : NPC
{
    #region State
    public IState IdleState { get; private set; }
    public IState MoveState { get; private set; }
    public GameObject player;
    public float DistanceBetweenPlayer { get; private set; }  // 你需要存储玩家与NPC的距离
    #endregion
    private static float lostPlayerTimer = 0f;

    protected override void Start()
    {
        base.Start();
        
        // 初始化状态机
        IdleState = new RedFriIdleState(Fsm, this, "Idle");
        MoveState = new RedFriMoveState(Fsm, this, "Move");

        // 初始状态切换
        Fsm.SwitchState(IdleState);
    }

    protected override void Update()
    {
        base.Update();

        lostPlayerTimer += Time.deltaTime;
        
        if (lostPlayerTimer >= lostPlayerTime)
        {
            Fsm.SwitchState(IdleState);
        }

        DistanceBetweenPlayer = Vector2.Distance(transform.position, player.transform.position);
        
        // 你可以在这里进行其他逻辑处理，比如根据距离决定状态切换等
        // 例如：
        if (DistanceBetweenPlayer > 5f)
        {
            Fsm.SwitchState(MoveState);  // 如果距离大于 5 切换到 Move 状态
        }
        else
        {
            Fsm.SwitchState(IdleState);  // 否则回到 Idle 状态
        }
    }

    public override void Die()
    {
       // Fsm.SwitchState(DeadState);
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

