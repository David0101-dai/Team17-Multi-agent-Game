using UnityEngine;

public class PlayerDamageable : Damageable
{
    public GameObject bloodEffect;  // 流血特效

    // 重写 Die 方法
    protected override void Die()
    {
        if (!TryGetComponent(out Player player)) return;
        player.Die();
    }

    // 重写 TakeDamage 方法
    public override void TakeDamage(GameObject from, bool isMagic = false, bool canEffect = true)
    {
        // 调用父类的 TakeDamage 方法
        base.TakeDamage(from, isMagic, canEffect);
        // 设置特效的偏移量，这里我们让特效在 y 轴上偏移 0.5 个单位
        Vector3 effectPosition = transform.position + new Vector3(0, 1.0f, 0);

        // 在角色的偏移位置生成流血特效
        Instantiate(bloodEffect, effectPosition, Quaternion.identity);
    }
}
