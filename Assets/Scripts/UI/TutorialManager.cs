using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialImage;  // 拖入你的 Image 对象
    public Button exitButton;         // 拖入你的退出按钮

    void Start()
    {
        // 先隐藏图片和按钮
        tutorialImage.SetActive(false);
        exitButton.gameObject.SetActive(false);

        // 2秒后显示
        StartCoroutine(ShowTutorialAfterDelay(2f));
    }

    IEnumerator ShowTutorialAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        tutorialImage.SetActive(true);
        exitButton.gameObject.SetActive(true);
    }

    public void HideTutorialImage()
    {
        Debug.Log("Button clicked! Hiding tutorial image.");
        tutorialImage.SetActive(false);
        exitButton.gameObject.SetActive(false);
    }
}
