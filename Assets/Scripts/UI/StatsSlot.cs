using TMPro;
using UnityEngine;

public class StatsSlot : MonoBehaviour
{
    [SerializeField] private string statName;
    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statNameText;
    [SerializeField] private TextMeshProUGUI statValueText;

    private void OnValidate()
    {
        // 在编辑器中自动给物体改名，方便查看
        gameObject.name = "Stat - " + statName;
        if (statNameText)
        {
            statNameText.text = statName;
        }
    }

    private void Update()
    {
        UpdateStatValue();
    }

    public void UpdateStatValue()
    {
        var playerStat = PlayerManager.Instance.player.GetComponent<Damageable>();
        if (playerStat == null) return;

        // 根据 statType 不同，选择不同的取值方式
        switch (statType)
        {
            case StatType.Damage:
                // 物理伤害显示最终值
                statValueText.text = playerStat.GetFinalDamageValue().ToString();
                break;

            case StatType.FireDamage:
                // 火焰伤害
                statValueText.text = playerStat.GetFinalFireDamageValue().ToString();
                break;

            case StatType.IceDamage:
                // 冰霜伤害
                statValueText.text = playerStat.GetFinalIceDamageValue().ToString();
                break;

            case StatType.LightingDamage:
                // 雷电伤害
                statValueText.text = playerStat.GetFinalLightningDamageValue().ToString();
                break;

            default:
                // 其余属性，直接显示 StatsOfType(...).GetValue()
                var value = playerStat.StatsOfType(statType).GetValue();
                statValueText.text = value.ToString();
                break;
        }
    }
}
