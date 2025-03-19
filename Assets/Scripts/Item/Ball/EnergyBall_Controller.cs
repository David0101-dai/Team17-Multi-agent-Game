using System.Collections;
using UnityEngine;

public class EnergyBall_Controller : EnergyBall  // 继承自 EnergyBall
{
    protected override void Start()
    {
        base.Start();  // 调用基类的 Start 方法
    }

    public new void Setup(GameObject attacker, float speed)
    {
        base.Setup(attacker, speed);  // 如果需要调用基类的 Setup 方法
        // 其他自定义的行为
    }

    protected override void OnTriggerEnter2D(Collider2D collision)  // 覆写 OnTriggerEnter2D 方法
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName) && !worked)
        {
            worked = true;
            rb.velocity = new Vector2(0, 0);
            Damageable damageableComponent = collision.GetComponent<Damageable>();
            Damageable attackerDamageable = Attacker.GetComponent<Damageable>();
            if (damageableComponent != null)
            {
                if (attackerDamageable != null)
                {
                    anim.SetBool("boom", true);
                    damageableComponent.TakeDamage(Attacker, true, false, false, false, true, false);
                    StartCoroutine(DestroyAfterAnimation());
                }
                else
                {
                    StartCoroutine(DestroyAfterAnimation());
                    Debug.LogWarning("Attacker's Damageable component is missing.");
                }
            }
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            Damageable damageableComponent = collision.GetComponent<Damageable>();
            anim.SetBool("boom", true);
            if (damageableComponent != null)
            {
                damageableComponent.TakeDamage(Attacker, true, false, false, false, true, false);
            }
            StartCoroutine(DestroyAfterAnimation());
        }
    }
}
