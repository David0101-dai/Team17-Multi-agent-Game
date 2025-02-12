using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(EnemyDamageable))]
[RequireComponent(typeof(FlashFX))]

public abstract class Enemy : Character
{
    #region Value
    [Header("Move Value")]
    public float defaultMoveSpeed = 7f;
    [HideInInspector] public float moveSpeed;

    [Header("Attack Value")]
    public float attackDistance = 2f;
    public float attackCooldown = 0.4f;
    public float lostPlayerTime = 7f;
    private float lastHitTime = 0f;
    public float hitCooldown = 0.5f; // 设置冷却时间为 0.5 秒


    [Header("Stun Value")]
    public float stunTime = 2f;
    public bool canBeStun = true;
    public GameObject counterImage;
    #endregion

    #region Component
    public Damageable Damageable { get; private set; }
    public FlashFX FlashFX { get; private set; }
    #endregion

    protected override void Start()
    {
        base.Start();

        moveSpeed = defaultMoveSpeed;

        Damageable = GetComponent<Damageable>();
        FlashFX = GetComponent<FlashFX>();

        Damageable.OnTakeDamage += (from, to) =>
        {
            damageFrom = from;

            // 检查是否符合冷却时间
            if (Time.time - lastHitTime >= hitCooldown)
            {
                // 更新时间戳
                lastHitTime = Time.time;

                if (damageFrom.CompareTag("Player") && IsInStunState())
                {
                    var isRight = damageFrom.transform.position.x > transform.position.x;
                    var isLeft = damageFrom.transform.position.x < transform.position.x;
                    var faceDir = isRight ? 1 : isLeft ? -1 : 0;
                    Rb.velocity = new Vector2(faceDir * -1 * knockbackXSpeed, knockbackYSpeed);
                    if (faceDir != 0 && faceDir != Flip.facingDir) Flip.Flip();
                    return;
                }

                // 只有在冷却时间过去后才触发 SwitchHitState
                SwitchHitState();
            }
        };

    }

    protected override void Update(){
       base.Update();
    }

    public void FreezeTimeForSeconds(float seconds)
    {
        StartCoroutine(FreezeTimeFor(seconds));
    }

    protected virtual IEnumerator FreezeTimeFor(float seconds)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(seconds);
        FreezeTime(false);
    }

    public virtual void FreezeTime(bool toggle)
    {
        if (toggle)
        {
            moveSpeed = 0;
            Anim.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            Anim.speed = 1;
        }
    }

    public virtual void OpenCounterAttackWindow()
    {
        canBeStun = true;
        counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow()
    {
        canBeStun = false;
        counterImage.SetActive(false);
    }

    public virtual bool CanBeStun()
    {
        if (canBeStun)
        {
            SwitchStunState();
            CloseCounterAttackWindow();
            return true;
        }
        else
        {
            return false;
        }
    }

    protected abstract bool IsInStunState();

    protected abstract void SwitchHitState();

    protected abstract void SwitchStunState();
}
