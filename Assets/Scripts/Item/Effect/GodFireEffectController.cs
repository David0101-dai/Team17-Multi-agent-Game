using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodFireEffectController : MonoBehaviour
{
    private bool hasAttack = true;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            var player = PlayerManager.Instance.player;
            if(hasAttack){
                 other.GetComponent<Damageable>().TakeDamage(player, true, false, false, true);
                 hasAttack =false;
            }
            Invoke(nameof(DestroyMe), 0.5f);
            GetComponent<PolygonCollider2D>().enabled = false;
        }
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }
}
