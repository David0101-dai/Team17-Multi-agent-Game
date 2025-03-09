using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public int damage = 10;               // �ش̵��˺�
    public float damageCooldown = 1f;     // ��ֹƵ����������ȴʱ��
    private float lastDamageTime;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �������Ƿ���ش���ײ
        if (collision.CompareTag("Player"))
        {
            // ��ֹ����ȴʱ���ڶ�δ����˺�
            if (Time.time - lastDamageTime < damageCooldown) return;

            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                // ȷ����ҵ� Damageable ����ѳ�ʼ��
                if (player.Damageable == null)
                {
                    Debug.LogError("Player Damageable component is missing.");
                    return;
                }

                // ��Ѫ������״̬�л����ܻ�״̬
                player.Damageable.TakeDamage(gameObject, isMagic: false, canEffect: true);

                // �л��� HitState
                player.Fsm.SwitchState(player.HitState);  // �л����ܻ�״̬

                // ������ȴʱ��
                lastDamageTime = Time.time;
            }
            else
            {
                Debug.LogError("Player component is missing on the object.");
            }
        }
    }
}
