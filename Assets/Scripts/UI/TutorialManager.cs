using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialImage;  // ������� Image ����
    public Button exitButton;         // ��������˳���ť
    public TextMeshProUGUI feedbackText; // ����TextMeshPro UI���

    private bool tutorialActive = false; // ��ǽ̳��Ƿ񼤻�

    void Start()
    {
        // ���������� UI Ԫ��
        tutorialImage.SetActive(false);
        exitButton.gameObject.SetActive(false);
        feedbackText.gameObject.SetActive(false); // ���ء�Good Job!!!��

        // 2�����ʾ�̳�ͼƬ�Ͱ�ť
        StartCoroutine(ShowTutorialAfterDelay(2f));
    }

    IEnumerator ShowTutorialAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        tutorialImage.SetActive(true);
        exitButton.gameObject.SetActive(true);
        tutorialActive = true; // ��ǽ̳��Ѽ���
    }

    void Update()
    {
        // ֻ�е��̳�ͼƬ��ʾʱ���ż����ո��
        if (tutorialActive && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(ShowGoodJobMessage());
        }
    }

    IEnumerator ShowGoodJobMessage()
    {
        feedbackText.text = "Good Job!!!";
        feedbackText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f); // ��ʾ 1 ��

        feedbackText.gameObject.SetActive(false); // ������ʧ
    }

    public void HideTutorialImage()
    {
        tutorialImage.SetActive(false);
        exitButton.gameObject.SetActive(false);
        tutorialActive = false; // �رս̳�״̬
    }
}
