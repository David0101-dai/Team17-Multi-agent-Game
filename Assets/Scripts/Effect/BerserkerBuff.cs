using System.Collections;
using UnityEngine;

/// <summary>
/// ��սʿ���������
/// 1. ��������˺���ԭ���� attackMultiplier ��  
/// 2. ÿ��۳� damagePerSecond ��Ѫ��  
/// 3. ÿ�ι���ʱ�ظ� healPerAttack ��Ѫ����������ҹ����߼��е��� OnAttack ������  
/// 4. ͬʱ�������ٶȱ�Ϊԭ���� 10 ��  
/// 5. ҩЧ������ָ�ԭʼ״̬
/// </summary>
public class BerserkerBuff : MonoBehaviour
{
    private float remainingDuration;
    private float attackMultiplier;
    private float damagePerSecond;
    private float healPerAttack;
    private GameObject effectInstance;

    private Player player;
    private int bonusDamage; // Ϊ�ﵽ����Ч�������ӵ��˺�ֵ
    private float tickTimer = 0f;

    // ����������ԭʼ�����ٶ�
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

        // ͨ�����Ӷ�����˺���ʵ���˺����ʣ�ע��ͨ�� Damageable ��ȡ Damage ���ԣ�
        int baseDamage = player.Damageable.Damage.GetValue();
        bonusDamage = baseDamage * (Mathf.RoundToInt(attackMultiplier) - 1);
        player.Damageable.Damage.AddModifier(bonusDamage);

        // ��������¼ԭʼ�����ٶȣ����������ٶ�����Ϊԭ����10��
        originalAttackSpeed = player.attackSpeed;
        player.attackSpeed = originalAttackSpeed * 5f;

        // �������������ҩЧ��Ч������У�
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
        // ÿ���������Ч��
        tickTimer += Time.deltaTime;
        if (tickTimer >= 1f)
        {
            tickTimer -= 1f;
            // ֱ�ӿ۳�Ѫ����ͨ�� Damageable.currentHp ��ȡ��ǰѪ����
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
    /// �ⲿ���ã�����ҹ���ʱ���ô˷������ظ�Ѫ��
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
        // �ָ�ԭʼ�˺����Ƴ����ӵ��˺�����
        if (player != null)
        {
            player.Damageable.Damage.RemoveModifier(bonusDamage);
            // �ָ�ԭʼ�����ٶ�
            player.attackSpeed = originalAttackSpeed;
        }
        // ����ҩЧ��Ч
        if (effectInstance != null)
        {
            Destroy(effectInstance);
        }
        Destroy(this);
    }
}
