using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "BerserkerEffect", menuName = "Data/ItemEffect/BerserkerEffect")]
public class BerserkerEffect : ItemEffect
{
    [Header("药效参数")]
    public float effectDuration = 10f;       // 药效持续时间 10 秒
    public float attackMultiplier = 10f;     // 伤害倍率
    public float damagePerSecond = 10f;        // 每秒自伤10点
    public float healPerAttack = 10f;          // 每次攻击回血10点

    [Header("药效特效")]
    public GameObject potionEffectPrefab;    // 药效特效预制体

    /// <summary>
    /// 直接通过 PlayerManager.Instance 获取玩家并执行效果
    /// </summary>
    /// <param name="from">效果发起者（此处可不使用）</param>
    /// <param name="to">效果目标（此处可不使用）</param>
    public override void ExecuteEffect(GameObject from, GameObject to)
    {
        var playerGO = PlayerManager.Instance.player;
        if (playerGO == null)
        {
            Debug.LogWarning("BerserkerEffect: Player not found!");
            return;
        }
        var player = playerGO.GetComponent<Player>();
        if (player == null)
        {
            Debug.LogWarning("BerserkerEffect: Player component not found!");
            return;
        }

        // 如果玩家身上已有狂战士增益，则先移除
        BerserkerBuff existingBuff = playerGO.GetComponent<BerserkerBuff>();
        if (existingBuff != null)
        {
            Destroy(existingBuff);
        }

        // 添加狂战士增益组件，并初始化参数
        BerserkerBuff buff = playerGO.AddComponent<BerserkerBuff>();
        buff.Initialize(effectDuration, attackMultiplier, damagePerSecond, healPerAttack, potionEffectPrefab);
    }
}
