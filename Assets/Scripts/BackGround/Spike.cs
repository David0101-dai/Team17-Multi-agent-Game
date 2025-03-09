using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public int damage = 10;               // 地刺的伤害
    public float damageCooldown = 1f;     // 防止频繁触发的冷却时间
    private float lastDamageTime;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 检测玩家是否与地刺碰撞
        if (collision.CompareTag("Player"))
        {
            // 防止在冷却时间内多次触发伤害
            if (Time.time - lastDamageTime < damageCooldown) return;

            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                // 确保玩家的 Damageable 组件已初始化
                if (player.Damageable == null)
                {
                    Debug.LogError("Player Damageable component is missing.");
                    return;
                }

                // 扣血并触发状态切换到受击状态
                player.Damageable.TakeDamage(gameObject, isMagic: false, canEffect: true);

                // 切换到 HitState
                player.Fsm.SwitchState(player.HitState);  // 切换到受击状态

                // 更新冷却时间
                lastDamageTime = Time.time;
            }
            else
            {
                Debug.LogError("Player component is missing on the object.");
            }
        }
    }
}
