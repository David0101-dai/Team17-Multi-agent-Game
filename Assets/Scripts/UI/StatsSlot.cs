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
        // �ڱ༭�����Զ����������������鿴
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

        // ���� statType ��ͬ��ѡ��ͬ��ȡֵ��ʽ
        switch (statType)
        {
            case StatType.Damage:
                // �����˺���ʾ����ֵ
                statValueText.text = playerStat.GetFinalDamageValue().ToString();
                break;

            case StatType.FireDamage:
                // �����˺�
                statValueText.text = playerStat.GetFinalFireDamageValue().ToString();
                break;

            case StatType.IceDamage:
                // ��˪�˺�
                statValueText.text = playerStat.GetFinalIceDamageValue().ToString();
                break;

            case StatType.LightingDamage:
                // �׵��˺�
                statValueText.text = playerStat.GetFinalLightningDamageValue().ToString();
                break;

            default:
                // �������ԣ�ֱ����ʾ StatsOfType(...).GetValue()
                var value = playerStat.StatsOfType(statType).GetValue();
                statValueText.text = value.ToString();
                break;
        }
    }
}
