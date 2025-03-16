using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "MagicEffect", menuName = "Data/ItemEffect/MagicEffect")]
public class MagicEffect : ItemEffect
{
    [Header("ҩЧ����")]
    public float effectDuration = 10f;          // ҩЧ����ʱ�� 10 ��
    public float boostedMagic = 9999f;          // ҩЧ�ڼ�ħ��ֵ
    public float cooldownMultiplier = 0.2f;     // ҩЧ�ڼ似����ȴʱ���Ϊԭ�������֮һ

    [Header("ҩЧ��Ч")]
    public GameObject potionEffectPrefab;       // ҩЧ��ЧԤ����

    /// <summary>
    /// �̳��Գ�����ִ࣬��ҩЧ�����
    /// from: �����ߣ�����ͨ������ң�
    /// to:   Ŀ�꣨����Ŀ�꣬�ɺ��ԣ�
    /// </summary>
    public override void ExecuteEffect(GameObject from, GameObject to)
    {
        // 1. ȷ�Ϸ������Ƿ������
        Player player = from.GetComponent<Player>();
        if (player == null)
        {
            Debug.LogWarning("MagicEffect: 'from' ����û�� Player ������޷�ִ��ҩЧ��");
            return;
        }

        // 2. ����ң�MonoBehaviour��������Э��
        player.StartCoroutine(ApplyPotionEffect(player));

        // �������Ҫ���ٳ����еġ�ҩƿ���塱��
        // ����ʹ�ø�Ч�����߼��ﴦ���������� ScriptableObject ����������
    }

    /// <summary>
    /// Э�̣�ִ�о����ҩЧ�߼�
    /// </summary>
    private IEnumerator ApplyPotionEffect(Player player)
    {
        // ����ԭʼ״̬
        float originalMaxMagic = MagicManager.Instance.maxMagic;
        float originalCurrentMagic = MagicManager.Instance.currentMagic;
        float originalCooldownMultiplier = player.cooldownMultiplier;

        // Ӧ��ҩЧ������ħ��ֵ���޸���ȴ����
        MagicManager.Instance.maxMagic = boostedMagic;
        MagicManager.Instance.currentMagic = boostedMagic;
        player.cooldownMultiplier = cooldownMultiplier;

        // ���������ʵ����ҩЧ��Ч������������Ч��
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

        // �ȴ�ҩЧ����ʱ�����
        yield return new WaitForSeconds(effectDuration);

        // �ָ�ԭʼ״̬
        MagicManager.Instance.maxMagic = originalMaxMagic;
        // �����ϣ���ָ�֮ǰ�ĵ�ǰħ����Ҳ����ȡ��ע�ͣ�
        // MagicManager.Instance.currentMagic = originalCurrentMagic;
        player.cooldownMultiplier = originalCooldownMultiplier;

        // ����ҩЧ��Ч
        if (effectInstance != null)
        {
            Object.Destroy(effectInstance);
        }
    }
}
