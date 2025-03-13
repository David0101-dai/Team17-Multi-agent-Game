using System.Collections;
using UnityEngine;
using UnityEngine.UI; // 用于 UI 组件

public class SceneTriggerWithKey : MonoBehaviour
{
    [SerializeField] private Transform targetPosition; // 目标传送位置
    [SerializeField] private GameObject wIndicator;    // "W" 提示 UI 物体

    private bool playerInTrigger = false; // 记录玩家是否在触发区内
    private Transform player;             // 记录玩家对象

    private void Start()
    {
        if (wIndicator != null)
        {
            wIndicator.SetActive(false); // 开始时隐藏 "W" 提示
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
                wIndicator.SetActive(true); // 显示 "W" 提示
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
                wIndicator.SetActive(false); // 隐藏 "W" 提示
            }
        }
    }

    private void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.W)) // 按下 W 键才传送
        {
            StartCoroutine(TeleportPlayer(player));
        }
    }

    private IEnumerator TeleportPlayer(Transform player)
    {
        if (wIndicator != null)
        {
            wIndicator.SetActive(false); // 传送时隐藏 "W" 提示
        }

        // 渐入黑屏
        yield return StartCoroutine(TransitionManager.Instance.Fade(0, 1));

        // 传送玩家
        player.position = targetPosition.position;

        // 渐出黑屏
        yield return StartCoroutine(TransitionManager.Instance.Fade(1, 0));
    }
}