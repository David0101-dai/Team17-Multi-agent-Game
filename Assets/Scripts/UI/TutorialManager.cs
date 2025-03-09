using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialImage;  // 拖入你的 Image 对象
    public Button exitButton;         // 拖入你的退出按钮
    public TextMeshProUGUI feedbackText; // 拖入TextMeshPro UI组件

    private bool tutorialActive = false; // 标记教程是否激活

    void Start()
    {
        // 先隐藏所有 UI 元素
        tutorialImage.SetActive(false);
        exitButton.gameObject.SetActive(false);
        feedbackText.gameObject.SetActive(false); // 隐藏“Good Job!!!”

        // 2秒后显示教程图片和按钮
        StartCoroutine(ShowTutorialAfterDelay(2f));
    }

    IEnumerator ShowTutorialAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        tutorialImage.SetActive(true);
        exitButton.gameObject.SetActive(true);
        tutorialActive = true; // 标记教程已激活
    }

    void Update()
    {
        // 只有当教程图片显示时，才监听空格键
        if (tutorialActive && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(ShowGoodJobMessage());
        }
    }

    IEnumerator ShowGoodJobMessage()
    {
        feedbackText.text = "Good Job!!!";
        feedbackText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f); // 显示 1 秒

        feedbackText.gameObject.SetActive(false); // 让它消失
    }

    public void HideTutorialImage()
    {
        tutorialImage.SetActive(false);
        exitButton.gameObject.SetActive(false);
        tutorialActive = false; // 关闭教程状态
    }
}
