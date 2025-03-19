using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSwordEffectController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            var player = PlayerManager.Instance.player;
            other.GetComponent<Damageable>().TakeDamage(player, false, false, false, false,true);
            Invoke(nameof(DestroyMe), 1f);
            GetComponent<CircleCollider2D>().enabled = false;
        }
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }
}
