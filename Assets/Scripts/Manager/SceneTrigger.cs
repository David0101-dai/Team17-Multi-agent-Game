using System.Collections;
using UnityEngine;

public class SceneTrigger : MonoBehaviour
{
    [SerializeField] private Transform targetPosition; // Ŀ�괫��λ��

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(TeleportPlayer(other.transform));
        }
    }

    private IEnumerator TeleportPlayer(Transform player)
    {
        // �������
        yield return StartCoroutine(TransitionManager.Instance.Fade(0, 1));

        // �������
        player.position = targetPosition.position;

        // ��������
        yield return StartCoroutine(TransitionManager.Instance.Fade(1, 0));
    }
}
