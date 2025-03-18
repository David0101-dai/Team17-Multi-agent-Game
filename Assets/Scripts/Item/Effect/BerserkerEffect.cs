using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "BerserkerEffect", menuName = "Data/ItemEffect/BerserkerEffect")]
public class BerserkerEffect : ItemEffect
{
    [Header("ҩЧ����")]
    public float effectDuration = 10f;       // ҩЧ����ʱ�� 10 ��
    public float attackMultiplier = 10f;     // �˺�����
    public float damagePerSecond = 10f;        // ÿ������10��
    public float healPerAttack = 10f;          // ÿ�ι�����Ѫ10��

    [Header("ҩЧ��Ч")]
    public GameObject potionEffectPrefab;    // ҩЧ��ЧԤ����

    /// <summary>
    /// ֱ��ͨ�� PlayerManager.Instance ��ȡ��Ҳ�ִ��Ч��
    /// </summary>
    /// <param name="from">Ч�������ߣ��˴��ɲ�ʹ�ã�</param>
    /// <param name="to">Ч��Ŀ�꣨�˴��ɲ�ʹ�ã�</param>
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

        // �������������п�սʿ���棬�����Ƴ�
        BerserkerBuff existingBuff = playerGO.GetComponent<BerserkerBuff>();
        if (existingBuff != null)
        {
            Destroy(existingBuff);
        }

        // ��ӿ�սʿ�������������ʼ������
        BerserkerBuff buff = playerGO.AddComponent<BerserkerBuff>();
        buff.Initialize(effectDuration, attackMultiplier, damagePerSecond, healPerAttack, potionEffectPrefab);
    }
}
