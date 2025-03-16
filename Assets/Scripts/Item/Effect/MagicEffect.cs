using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "MagicEffect", menuName = "Data/ItemEffect/MagicEffect")]
public class MagicEffect : ItemEffect
{
    [Header("药效参数")]
    public float effectDuration = 10f;       // 药效持续时间 10 秒
    public float boostedMagic = 9999f;       // 药效期间魔力值
    public float cooldownMultiplier = 0.2f;  // 药效期间技能冷却时间变为原来的五分之一

    [Header("药效特效")]
    public GameObject potionEffectPrefab;    // 药效特效预制体

    /// <summary>
    /// 仿照 HealEffect：直接在此处获取玩家并执行效果
    /// </summary>
    /// <param name="from">效果发起者（此处可不使用）</param>
    /// <param name="to">效果目标（此处可不使用）</param>
    public override void ExecuteEffect(GameObject from, GameObject to)
    {
        // 与 HealEffect 类似，直接通过 PlayerManager.Instance 获取玩家
        var player = PlayerManager.Instance.player.GetComponent<Player>();
        if (player == null)
        {
            Debug.LogWarning("MagicEffect: Player not found!");
            return;
        }

        // 在玩家上启动协程（玩家是 MonoBehaviour，能执行协程）
        player.StartCoroutine(ApplyPotionEffect(player));
    }

    /// <summary>
    /// 协程：执行 10 秒药效，并在结束后恢复状态
    /// </summary>
    private IEnumerator ApplyPotionEffect(Player player)
    {
        // 备份原始状态
        float originalMaxMagic = MagicManager.Instance.maxMagic;
        float originalCurrentMagic = MagicManager.Instance.currentMagic;
        float originalCooldownMultiplier = player.cooldownMultiplier;

        // 应用药效：魔力值飙升、技能冷却变短
        MagicManager.Instance.maxMagic = boostedMagic;
        MagicManager.Instance.currentMagic = boostedMagic;
        player.cooldownMultiplier = cooldownMultiplier;

        // 在玩家身上生成特效（如果有）
        GameObject effectInstance = null;
        if (potionEffectPrefab != null)
        {
            effectInstance = Instantiate(
                potionEffectPrefab,
                player.transform.position,
                Quaternion.identity,
                player.transform
            );
        }

        // 等待药效持续时间
        yield return new WaitForSeconds(effectDuration);

        // 恢复原始状态
        MagicManager.Instance.maxMagic = originalMaxMagic;
        // 如果要恢复当前魔力，可取消下面注释
        // MagicManager.Instance.currentMagic = originalCurrentMagic;
        player.cooldownMultiplier = originalCooldownMultiplier;

        // 销毁药效特效
        if (effectInstance != null)
        {
            Destroy(effectInstance);
        }
    }
}
