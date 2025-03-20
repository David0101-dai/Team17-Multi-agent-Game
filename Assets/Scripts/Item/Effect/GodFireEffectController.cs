using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodFireEffectController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            var player = PlayerManager.Instance.player;
            other.GetComponent<Damageable>().TakeDamage(player, false, false, false, true);
            Invoke(nameof(DestroyMe), 0.5f);
            GetComponent<PolygonCollider2D>().enabled = false;
        }
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }
}
