using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "MagicEffect", menuName = "Data/ItemEffect/MagicEffect")]
public class MagicEffect : ItemEffect
{
    [Header("药效参数")]
    public float effectDuration = 10f;          // 药效持续时间 10 秒
    public float boostedMagic = 9999f;          // 药效期间魔力值
    public float cooldownMultiplier = 0.2f;     // 药效期间技能冷却时间变为原来的五分之一

    [Header("药效特效")]
    public GameObject potionEffectPrefab;       // 药效特效预制体

    /// <summary>
    /// 继承自抽象基类，执行药效的入口
    /// from: 发起者（这里通常是玩家）
    /// to:   目标（如无目标，可忽略）
    /// </summary>
    public override void ExecuteEffect(GameObject from, GameObject to)
    {
        // 1. 确认发起者是否是玩家
        Player player = from.GetComponent<Player>();
        if (player == null)
        {
            Debug.LogWarning("MagicEffect: 'from' 对象没有 Player 组件，无法执行药效！");
            return;
        }

        // 2. 在玩家（MonoBehaviour）上启动协程
        player.StartCoroutine(ApplyPotionEffect(player));

        // 如果有需要销毁场景中的“药瓶物体”，
        // 请在使用该效果的逻辑里处理，而不是在 ScriptableObject 里销毁自身。
    }

    /// <summary>
    /// 协程：执行具体的药效逻辑
    /// </summary>
    private IEnumerator ApplyPotionEffect(Player player)
    {
        // 备份原始状态
        float originalMaxMagic = MagicManager.Instance.maxMagic;
        float originalCurrentMagic = MagicManager.Instance.currentMagic;
        float originalCooldownMultiplier = player.cooldownMultiplier;

        // 应用药效：提升魔法值，修改冷却倍率
        MagicManager.Instance.maxMagic = boostedMagic;
        MagicManager.Instance.currentMagic = boostedMagic;
        player.cooldownMultiplier = cooldownMultiplier;

        // 在玩家身上实例化药效特效（例如粒子特效）
        GameObject effectInstance = null;
        if (potionEffectPrefab != null)
        {
            effectInstance = Object.Instantiate(
                potionEffectPrefab,
                player.transform.position,
                Quaternion.identity,
                player.transform
            );
        }

        // 等待药效持续时间结束
        yield return new WaitForSeconds(effectDuration);

        // 恢复原始状态
        MagicManager.Instance.maxMagic = originalMaxMagic;
        // 如果你希望恢复之前的当前魔力，也可以取消注释：
        // MagicManager.Instance.currentMagic = originalCurrentMagic;
        player.cooldownMultiplier = originalCooldownMultiplier;

        // 销毁药效特效
        if (effectInstance != null)
        {
            Object.Destroy(effectInstance);
        }
    }
}
