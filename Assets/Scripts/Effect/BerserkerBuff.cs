using System.Collections;
using UnityEngine;

/// <summary>
/// 狂战士增益组件：
/// 1. 提升玩家伤害至原来的 attackMultiplier 倍  
/// 2. 每秒扣除 damagePerSecond 点血量  
/// 3. 每次攻击时回复 healPerAttack 点血量（需在玩家攻击逻辑中调用 OnAttack 方法）  
/// 4. 同时将攻击速度变为原来的 10 倍  
/// 5. 药效结束后恢复原始状态
/// </summary>
public class BerserkerBuff : MonoBehaviour
{
    private float remainingDuration;
    private float attackMultiplier;
    private float damagePerSecond;
    private float healPerAttack;
    private GameObject effectInstance;

    private Player player;
    private int bonusDamage; // 为达到倍率效果所增加的伤害值
    private float tickTimer = 0f;

    // 新增：保存原始攻击速度
    private float originalAttackSpeed;
    [SerializeField] Vector3 effectOffset = new Vector3(0, 1f, 0);

    public void Initialize(float effectDuration, float attackMultiplier, float damagePerSecond, float healPerAttack, GameObject potionEffectPrefab)
    {
        remainingDuration = effectDuration;
        this.attackMultiplier = attackMultiplier;
        this.damagePerSecond = damagePerSecond;
        this.healPerAttack = healPerAttack;
        player = GetComponent<Player>();
        if (player == null)
        {
            Debug.LogWarning("BerserkerBuff: Player component not found!");
            Destroy(this);
            return;
        }

        // 通过增加额外的伤害来实现伤害倍率（注意通过 Damageable 获取 Damage 属性）
        int baseDamage = player.Damageable.Damage.GetValue();
        bonusDamage = baseDamage * (Mathf.RoundToInt(attackMultiplier) - 1);
        player.Damageable.Damage.AddModifier(bonusDamage);

        // 新增：记录原始攻击速度，并将攻击速度设置为原来的10倍
        originalAttackSpeed = player.attackSpeed;
        player.attackSpeed = originalAttackSpeed * 5f;

        // 在玩家身上生成药效特效（如果有）
        if (potionEffectPrefab != null)
        {
            effectInstance = Instantiate(
                potionEffectPrefab,
                player.transform.position + effectOffset,
                Quaternion.identity,
                player.transform);
        }
    }

    private void Update()
    {
        // 每秒造成自伤效果
        tickTimer += Time.deltaTime;
        if (tickTimer >= 1f)
        {
            tickTimer -= 1f;
            // 直接扣除血量（通过 Damageable.currentHp 获取当前血量）
            player.Damageable.currentHp -= Mathf.RoundToInt(damagePerSecond);
            if (player.Damageable.currentHp <= 0)
            {
                player.Damageable.currentHp = 0;
                player.Die();
                return;
            }
        }

        remainingDuration -= Time.deltaTime;
        if (remainingDuration <= 0)
        {
            EndBuff();
        }
    }

    /// <summary>
    /// 外部调用：在玩家攻击时调用此方法来回复血量
    /// </summary>
    public void OnAttack()
    {
        if (player != null)
        {
            player.Damageable.IncreaseHealthBy(Mathf.RoundToInt(healPerAttack));
        }
    }

    private void EndBuff()
    {
        // 恢复原始伤害：移除增加的伤害修正
        if (player != null)
        {
            player.Damageable.Damage.RemoveModifier(bonusDamage);
            // 恢复原始攻击速度
            player.attackSpeed = originalAttackSpeed;
        }
        // 销毁药效特效
        if (effectInstance != null)
        {
            Destroy(effectInstance);
        }
        Destroy(this);
    }
}
