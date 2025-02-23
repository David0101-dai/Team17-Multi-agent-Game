using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFriend : NPC
{
    #region State
    public IState IdleState { get; private set; }
    public IState MoveState { get; private set; }
    public IState FollowState { get; private set; }  // 新增跟随状态
    #endregion

    #region Player
    public GameObject player;
    public float DistanceBetweenPlayer { get; private set; }
    public float moveTimer = 5f;  // 跟随超时计时器
    public float moveTime = 5f;
    public bool isFollowing = false;  // 标记是否已进入伴随状态
    public float FollowTimeout = 5f;  // 设定超时5秒
    public float MaxDistance = 5f;  // 伴随状态的最大距离
    public float ExtraMoveChance = 0.2f;  // 每次移动时有20%的概率再向前走几步
    #endregion

    protected override void Start()
    {
        base.Start();
        // 初始化状态机
        IdleState = new RedFriIdleState(Fsm, this, "Idle");
        MoveState = new RedFriMoveState(Fsm, this, "Move");
        FollowState = new RedFriFollowState(Fsm, this, "Move");
        // 初始状态切换
        Fsm.SwitchState(IdleState);
    }

    protected override void Update()
    {
        base.Update();
        //实时计算和玩家之间的距离
        player = PlayerManager.Instance.player;
        DistanceBetweenPlayer = Vector2.Distance(transform.position, player.transform.position);
        Debug.Log("RedFriend的Current State: " + Fsm.CurrentState.ToString());

        FollowPlayer();

    }

    private void FollowPlayer()
    {

        // 非跟随状态会计时
        if (isFollowing)
        {
            moveTimer = moveTime;
        }
        else
        {
            moveTimer -= Time.deltaTime;
        }
        if (DistanceBetweenPlayer <= MaxDistance)
        {
            isFollowing = true;
        }
        else
        {
            isFollowing = false;
        }
        if (moveTimer <= 0)
        {
            // 超过5秒没有进入伴随状态，直接传送到玩家头顶
            transform.position = player.transform.position + new Vector3(0, 5, 0);  // 玩家头顶2个单位的位置
            moveTimer = moveTime;  // 重置计时器
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