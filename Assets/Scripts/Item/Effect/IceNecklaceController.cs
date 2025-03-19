using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceNecklaceController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            var player = PlayerManager.Instance.player;
            other.GetComponent<Damageable>().TakeDamage(player, false, false, false, false,true);
        }
    }
}
