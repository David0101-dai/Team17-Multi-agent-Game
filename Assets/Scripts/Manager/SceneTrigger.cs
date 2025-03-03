using System.Collections;
using UnityEngine;

public class SceneTrigger : MonoBehaviour
{
    [SerializeField] private Transform targetPosition; // 目标传送位置

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(TeleportPlayer(other.transform));
        }
    }

    private IEnumerator TeleportPlayer(Transform player)
    {
        // 渐入黑屏
        yield return StartCoroutine(TransitionManager.Instance.Fade(0, 1));

        // 传送玩家
        player.position = targetPosition.position;

        // 渐出黑屏
        yield return StartCoroutine(TransitionManager.Instance.Fade(1, 0));
    }
}
