using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordSKillController : MonoBehaviour
{
    private SwordType swordType;

    [Header("Bounce Info")]
    private bool isBouncing;
    //private bool fireAttached;
    private float bounceSpeed;
    private int bounceAmount;
    private List<Transform> enemyTargets;
    private int targetIndex;
    
    [Header("Pierce Info")]
    private bool isPierce;
    private float pierceAmount;
    
    [Header("Spin Info")]
    private bool isSpinning;
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private float hitTimer;
    private float hitCooldown;

    private float spinDirection;

    [Header("State Info")]
    private float returnSpeed;
    private bool canRotate;
    private bool isReturning;
    private float freezeTime;
    private Vector2 pos;
    private Vector2 playerPos;
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();

        canRotate = true;
        //fireAttached = true;
        enemyTargets = new List<Transform>();
    }

    public void Setup(
        SwordType swordType,
        Player player,
        Vector2 aimDir,
        float gravityScale,
        float returnSpeed,
        float bounceSpeed,
        int bounceAmount,
        int pierceAmount,
        float maxTravelDistance,
        float spinDuration,
        float hitCooldown,
        float freezeTime)
    {
        this.swordType = swordType;
        this.player = player;
        rb.velocity = aimDir;
        rb.gravityScale = gravityScale;
        this.returnSpeed = returnSpeed;
        this.bounceSpeed = bounceSpeed;
        this.bounceAmount = bounceAmount;
        this.pierceAmount = pierceAmount;
        this.maxTravelDistance = maxTravelDistance;
        this.spinDuration = spinDuration;
        this.hitCooldown = hitCooldown;
        this.freezeTime = freezeTime;

        AudioManager.instance.PlaySFX(11,null);

        //新增旋转向前逻辑
        spinDirection = Mathf.Clamp(rb.velocity.x,-1,1);

        if (swordType == SwordType.Bounce)
            isBouncing = true;

        if (swordType == SwordType.Spin)
            isSpinning = true;

        if (swordType == SwordType.Pierce)
        {
            isPierce = true;
        }
        else
        {
            anim.SetBool("Rotation", true);
        }

        Invoke(nameof(DestoryMe), 5);
    }

    private void DestoryMe()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        pos = transform.position;
        playerPos = player.transform.position + new Vector3(0, 1, 0);

        if (canRotate)
        {
            transform.right = rb.velocity;
        }

        ReturnLogic();

        switch (swordType)
        {
            case SwordType.Bounce:
                BounceLogic();
                break;
            case SwordType.Spin:
                SpinLogic();
                break;
            default:
                break;
        }
    }
    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.SetParent(FxManager.Instance.fx.transform);
        isReturning = true;
    }
    private void ReturnLogic()
    {
        if (!isReturning) return;
        cd.enabled = true;
        anim.SetBool("Rotation", true);
        transform.position = Vector2.MoveTowards(pos, playerPos, returnSpeed * Time.deltaTime);
        if (Vector2.Distance(pos, playerPos) >= 1) return;
        player.CatchSword();
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTargets.Count > 0)
        {
            anim.SetBool("Rotation", true);
            var enemyPos = enemyTargets[targetIndex].position + new Vector3(0, 1, 0);
            transform.position = Vector2.MoveTowards(pos, enemyPos, bounceSpeed * Time.deltaTime);
            if (Vector2.Distance(pos, enemyPos) < 0.1f)
            {
                TakeDamage(enemyTargets[targetIndex].GetComponent<Collider2D>());
                targetIndex = (targetIndex + 1) % enemyTargets.Count;
                bounceAmount--;

                AudioManager.instance.StopSFX(10); //停止旋转音效

                if (bounceAmount <= 0)
                {
                    enemyTargets.Clear();
                    isReturning = true;
                    isBouncing = false;
                }
            }
        }
    }

    private void SpinLogic()
    {
        if (Vector2.Distance(playerPos, pos) > maxTravelDistance && !wasStopped && !isReturning)
        {
            StopWhenSpinning();
        }

        if (!wasStopped) return;

        spinTimer -= Time.deltaTime;

        // 只有当碰撞体是敌人时，才缓慢向前移动
        var colliders = Physics2D.OverlapCircleAll(transform.position, 1);  // 你可以调整这里的范围
        foreach (var hit in colliders)
        {
            if (hit.CompareTag("Enemy"))
            {
                // 触碰到敌人时，缓慢向前移动
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);
            }
        }

        if (spinTimer < 0)
        {
            isReturning = true;
            isSpinning = false;
        }

        hitTimer -= Time.deltaTime;
        if (hitTimer >= 0) return;

        hitTimer = hitCooldown;

        colliders = Physics2D.OverlapCircleAll(transform.position, 1);
        foreach (var hit in colliders)
        {
            // 跳过Player标签的碰撞体
            if (hit.CompareTag("Player")) continue;

            TakeDamage(hit);
        }

        AudioManager.instance.PlaySFX(10,null); //回旋镖音效
    }
    private void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Platform"))
        {
            if (other.CompareTag("Player") || !other.CompareTag("Enemy")) return;

            if (!isSpinning && !isBouncing) TakeDamage(other);

            GetBounceEnemy();

            if (CancelStuck()) return;
        }

        StuckInTo(other);
    }


    private void TakeDamage(Collider2D other)
    {
        // 先确认命中的是敌人
        if (!other.TryGetComponent(out Damageable damageable)) return;
        if (!other.TryGetComponent(out Enemy enemy)) return;

        // 如果你的敌人脚本里对“脆弱”或“冻结”有单独实现，也可以拿出来
        EnemyDamageable enemyDamageable = enemy.GetComponent<EnemyDamageable>();

        // 根据当前剑类型，决定伤害类型 & 是否附加特殊效果
        switch (swordType)
        {
            case SwordType.Regular:
                // Regular: 纯物理伤害 + 30%衰减（isFromSwordSkill=true）
                damageable.TakeDamage(
                    player.gameObject,
                    isMagic: false,      // 物理
                    canEffect: true,
                    isFromSwordSkill: true,  // 表示剑技衰减
                    isFireDamage: false,
                    isIceDamage: false,
                    isShockDamage: false
                );
                break;

            case SwordType.Spin:
                // Spin: 火焰魔法伤害
                damageable.TakeDamage(
                    player.gameObject,
                    isMagic: true,   // 魔法伤害
                    canEffect: true,
                    isFromSwordSkill: false,
                    isFireDamage: true,
                    isIceDamage: false,
                    isShockDamage: false
                );
                break;

            case SwordType.Pierce:
                // Pierce: 冰霜魔法伤害 + 冻结敌人 (时间停止效果)
                damageable.TakeDamage(
                    player.gameObject,
                    isMagic: true,
                    canEffect: true,
                    isFromSwordSkill: false,
                    isFireDamage: false,
                    isIceDamage: true,
                    isShockDamage: false
                );
                // 在这里直接冻结敌人
                enemy.FreezeTimeForSeconds(freezeTime);
                break;

            case SwordType.Bounce:
                // Bounce: 雷电魔法伤害 + 使敌人脆弱
                damageable.TakeDamage(
                    player.gameObject,
                    isMagic: true,
                    canEffect: true,
                    isFromSwordSkill: false,
                    isFireDamage: false,
                    isIceDamage: false,
                    isShockDamage: true
                );
                // 让敌人进入脆弱状态
                enemyDamageable.MakeVulnerableFor(freezeTime);
                break;
        }
    }


    private void GetBounceEnemy()
    {
        if (!isBouncing || enemyTargets.Count > 0) return;
        var colliders = Physics2D.OverlapCircleAll(transform.position, 6);
        foreach (var hit in colliders)
        {
            if (!hit.CompareTag("Enemy")) continue;
            enemyTargets.Add(hit.transform);
        }
    }

    private bool CancelStuck()
    {
        if (isSpinning)
        {
            wasStopped = true;
            cd.enabled = false;
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
            spinTimer = spinDuration;
            return true;
        }

        if (isPierce && pierceAmount > 0)
        {
            pierceAmount--;

            return true;
        }

        if (isBouncing && enemyTargets.Count > 0)
        {
            canRotate = false;
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            return true;
        }

        return false;
    }

    private void StuckInTo(Collider2D other)
    {
        canRotate = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        cd.enabled = false;
        transform.SetParent(other.transform);
        anim.SetBool("Rotation", false);
    }
}
