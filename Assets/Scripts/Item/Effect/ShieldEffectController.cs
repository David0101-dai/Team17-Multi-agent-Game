using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEffectController : MonoBehaviour
{
    [SerializeField] private GameObject shieldbreak;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")|| other.CompareTag("Spike"))
        {
            Debug.Log("spike!");
            var player=PlayerManager.Instance.player;
            var stats = PlayerManager.Instance.player.GetComponent<Damageable>();
            Invoke(nameof(DestroyMe), 0.5f);
            var newShield = Instantiate(shieldbreak);
            newShield.transform.SetParent(player.transform, false); // false 保证相对位置不变
            newShield.transform.localPosition = new Vector3(0, 1, 0);
            stats.MakeInvincible(false);
            Destroy(newShield, 1.17f);
            GetComponent<CircleCollider2D>().enabled = false;
        }
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }
}
