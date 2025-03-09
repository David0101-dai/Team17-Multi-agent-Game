using UnityEngine;

public class PlayerDamageable : Damageable
{   
    [Header("Effect")]
    public GameObject bloodEffect;  // 流血特效
    public float offset = 1.0f;      // 特效的偏移量

    // 重写 Die 方法
    protected override void Die()
    {
        if (!TryGetComponent(out Player player)) return;
        player.Die();
    }

    // 重写 TakeDamage 方法
    public override void TakeDamage(GameObject from, bool isMagic = false, bool canEffect = true, bool isFromSwordSkill = false)
    {
        // 调用父类的 TakeDamage 方法
        base.TakeDamage(from, isMagic, canEffect);
        Vector3 effectPosition = transform.position + new Vector3(0, offset, 0);
        // 在角色的偏移位置生成流血特效
        Instantiate(bloodEffect, effectPosition, Quaternion.identity);
    }
}
