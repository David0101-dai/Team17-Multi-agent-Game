using System.Collections;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public TextMeshProUGUI tutorialText; // �� UI �������
    public CanvasGroup textCanvasGroup;  // �� UI ͸�������

    private bool waitingForInput = false;
    public float fadeDuration = 1f;  // ���ƽ������Ե�ʱ�䣬ֵԽС����Խ�죬ֵԽ�󶯻�Խ��

    void Start()
    {
        StartCoroutine(RunTutorial()); // �����̳�����
    }

    /// <summary>
    /// ���̳����̿���
    /// </summary>
    IEnumerator RunTutorial()
    {
        yield return new WaitForSeconds(1f);  // ��ʼ�ȴ� 1 ��
        yield return ShowMessage("Welcome to the tutorial!", 2f); // ��ʾ��ӭ��Ϣ 2 ��

        yield return ShowMessage("Press the space bar", 0f); // ��ʾ���ո�
        yield return WaitForPlayerInput(KeyCode.Space); // �ȴ���Ұ��¿ո�

        yield return ShowMessage("Good Job!!!", 2f); // ��ʾ "Good Job!!!" ���ȴ� 2 ��
        yield return ShowMessage("Press A or D to move", 0f);
        yield return WaitForPlayerInput(KeyCode.A, KeyCode.D);

        yield return ShowMessage("Good Job!!!", 2f);
        yield return ShowMessage("Press Shift to sprint", 0f);
        yield return WaitForPlayerInput(KeyCode.LeftShift);

        yield return ShowMessage("Good Job!!!", 2f);
        yield return ShowMessage("Left Click to attack", 0f);
        yield return WaitForPlayerInput(KeyCode.Mouse0);

        yield return ShowMessage("Good Job!!!", 2f);
        yield return ShowMessage("Right Click to block", 0f);
        yield return WaitForPlayerInput(KeyCode.Mouse1);

        yield return ShowMessage("Good Job!!!", 2f);
        yield return FadeOut(); // �����ȫ�����ı�
    }

    /// <summary>
    /// ��ʾָ���ı���������ʾǰ��ִ�н������Զ���
    /// </summary>
    /// <param name="message">Ҫ��ʾ���ı�</param>
    /// <param name="duration">��ʾʱ����0 ��ʾֱ��������룩</param>
    IEnumerator ShowMessage(string message, float duration)
    {
        yield return FadeOut(); // �ȵ������ı�
        tutorialText.text = message; // ������������

        // ����� "Good Job!!!"���ı���ɫ�������С����ʽ
        tutorialText.color = (message == "Good Job!!!") ? Color.green : Color.white;
        tutorialText.fontSize = (message == "Good Job!!!") ? 40 : 30;
        tutorialText.fontStyle = (message == "Good Job!!!") ? FontStyles.Bold | FontStyles.Italic : FontStyles.Normal;

        yield return FadeIn(); // �������ı�

        if (duration > 0) yield return new WaitForSeconds(duration); // ��� duration ���� 0���ȴ�ָ��ʱ��
    }

    /// <summary>
    /// �ȴ���Ұ���ָ������
    /// </summary>
    IEnumerator WaitForPlayerInput(params KeyCode[] keys)
    {
        waitingForInput = true;
        while (waitingForInput)
        {
            foreach (KeyCode key in keys)
            {
                if (Input.GetKeyDown(key)) // �����������
                {
                    waitingForInput = false;
                    break;
                }
            }
            yield return null; // �ȴ���һ֡�����������
        }
    }

    /// <summary>
    /// ���붯����ʹ�ı���͸����Ϊ��ȫ�ɼ���
    /// </summary>
    IEnumerator FadeIn()
    {
        float timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime; // ��ʱ
            textCanvasGroup.alpha = Mathf.Lerp(0, 1, timer / fadeDuration); // ��͸����0�����䵽��ȫ�ɼ���1��
            yield return null; // �ȴ���һ֡
        }
        textCanvasGroup.alpha = 1; // ȷ������͸����Ϊ 1
    }

    /// <summary>
    /// ����������ʹ�ı�����ȫ�ɼ���Ϊ͸����
    /// </summary>
    IEnumerator FadeOut()
    {
        float timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime; // ��ʱ
            textCanvasGroup.alpha = Mathf.Lerp(1, 0, timer / fadeDuration); // ����ȫ�ɼ���1�����䵽͸����0��
            yield return null; // �ȴ���һ֡
        }
        textCanvasGroup.alpha = 0; // ȷ������͸����Ϊ 0
    }
}
