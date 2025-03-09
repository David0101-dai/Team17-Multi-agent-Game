using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialImage;  // ������� Image ����
    public Button exitButton;         // ��������˳���ť

    void Start()
    {
        // ������ͼƬ�Ͱ�ť
        tutorialImage.SetActive(false);
        exitButton.gameObject.SetActive(false);

        // 2�����ʾ
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
