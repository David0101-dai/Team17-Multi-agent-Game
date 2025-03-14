using System.Collections;
using UnityEngine;
using UnityEngine.UI; // ���� UI ���

public class SceneTriggerWithKey : MonoBehaviour
{
    [SerializeField] private Transform targetPosition; // Ŀ�괫��λ��
    [SerializeField] private GameObject wIndicator;    // "W" ��ʾ UI ����

    private bool playerInTrigger = false; // ��¼����Ƿ��ڴ�������
    private Transform player;             // ��¼��Ҷ���

    private void Start()
    {
        if (wIndicator != null)
        {
            wIndicator.SetActive(false); // ��ʼʱ���� "W" ��ʾ
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            player = other.transform;

            if (wIndicator != null)
            {
                wIndicator.SetActive(true); // ��ʾ "W" ��ʾ
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            player = null;

            if (wIndicator != null)
            {
                wIndicator.SetActive(false); // ���� "W" ��ʾ
            }
        }
    }

    private void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.W)) // ���� W ���Ŵ���
        {
            StartCoroutine(TeleportPlayer(player));
        }
    }

    private IEnumerator TeleportPlayer(Transform player)
    {
        if (wIndicator != null)
        {
            wIndicator.SetActive(false); // ����ʱ���� "W" ��ʾ
        }

        // �������
        yield return StartCoroutine(TransitionManager.Instance.Fade(0, 1));

        // �������
        player.position = targetPosition.position;

        // ��������
        yield return StartCoroutine(TransitionManager.Instance.Fade(1, 0));
    }
}