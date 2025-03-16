using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "MagicEffect", menuName = "Data/ItemEffect/MagicEffect")]
public class MagicEffect : ItemEffect
{
    [Header("ҩЧ����")]
    public float effectDuration = 10f;       // ҩЧ����ʱ�� 10 ��
    public float boostedMagic = 9999f;       // ҩЧ�ڼ�ħ��ֵ
    public float cooldownMultiplier = 0.2f;  // ҩЧ�ڼ似����ȴʱ���Ϊԭ�������֮һ

    [Header("ҩЧ��Ч")]
    public GameObject potionEffectPrefab;    // ҩЧ��ЧԤ����

    /// <summary>
    /// ���� HealEffect��ֱ���ڴ˴���ȡ��Ҳ�ִ��Ч��
    /// </summary>
    /// <param name="from">Ч�������ߣ��˴��ɲ�ʹ�ã�</param>
    /// <param name="to">Ч��Ŀ�꣨�˴��ɲ�ʹ�ã�</param>
    public override void ExecuteEffect(GameObject from, GameObject to)
    {
        // �� HealEffect ���ƣ�ֱ��ͨ�� PlayerManager.Instance ��ȡ���
        var player = PlayerManager.Instance.player.GetComponent<Player>();
        if (player == null)
        {
            Debug.LogWarning("MagicEffect: Player not found!");
            return;
        }

        // �����������Э�̣������ MonoBehaviour����ִ��Э�̣�
        player.StartCoroutine(ApplyPotionEffect(player));
    }

    /// <summary>
    /// Э�̣�ִ�� 10 ��ҩЧ�����ڽ�����ָ�״̬
    /// </summary>
    private IEnumerator ApplyPotionEffect(Player player)
    {
        // ����ԭʼ״̬
        float originalMaxMagic = MagicManager.Instance.maxMagic;
        float originalCurrentMagic = MagicManager.Instance.currentMagic;
        float originalCooldownMultiplier = player.cooldownMultiplier;

        // Ӧ��ҩЧ��ħ��ֵ�����������ȴ���
        MagicManager.Instance.maxMagic = boostedMagic;
        MagicManager.Instance.currentMagic = boostedMagic;
        player.cooldownMultiplier = cooldownMultiplier;

        // ���������������Ч������У�
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

        // �ȴ�ҩЧ����ʱ��
        yield return new WaitForSeconds(effectDuration);

        // �ָ�ԭʼ״̬
        MagicManager.Instance.maxMagic = originalMaxMagic;
        // ���Ҫ�ָ���ǰħ������ȡ������ע��
        // MagicManager.Instance.currentMagic = originalCurrentMagic;
        player.cooldownMultiplier = originalCooldownMultiplier;

        // ����ҩЧ��Ч
        if (effectInstance != null)
        {
            Destroy(effectInstance);
        }
    }
}
