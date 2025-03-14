using System.Collections;
using UnityEngine;
using UnityEngine.UI; // ����UI�����ռ�
using TMPro;

public class TutorialManager : MonoBehaviour
{
    private DashSkill dashSkill;
    private CounterSkill counterSkill;


    [Header("�ı�_1")]
    public TextMeshProUGUI tutorialText; // �� UI �������
    public CanvasGroup textCanvasGroup;  // �� UI ͸�������

    [Header("�ı�_2")]
    public TextMeshProUGUI characterUIText;         // �ڶ����ı���
    public CanvasGroup characterTextCanvasGroup;    // �ڶ��� CanvasGroup

    [Header("������ť")]
    public GameObject skipButton; // ֱ�����ð�ť GameObject


    private bool waitingForInput = false;
    private bool isTutorialSkipped = false; // ������ʶ
    public float fadeDuration = 1f;  // ���ƽ������Ե�ʱ�䣬ֵԽС����Խ�죬ֵԽ�󶯻�Խ��

    void Start()
    {
        // �����������Ϸ��ֱ�������̳�
        if (!All.IsNewGame)
        {
            // ����ѡ��ѽ̳��ı����ء���ֱ��Destroy(TutorialManager)
            skipButton.SetActive(false);
            return;
        }

        dashSkill = FindObjectOfType<DashSkill>();
        counterSkill = FindObjectOfType<CounterSkill>();

        // ȷ��������ť��ʾ�����󶨰�ť����¼�
        skipButton.SetActive(true);
        skipButton.GetComponent<Button>().onClick.AddListener(SkipTutorial);

        StartCoroutine(RunTutorial()); // �����̳�����
    }

    



    /// <summary>
    /// �����̳�
    /// </summary>
    public void SkipTutorial()
    {
        isTutorialSkipped = true;
        skipButton.SetActive(false);
        // ����ֹͣ���н̳���ص�Э��
        StopAllCoroutines();
        // ֱ�����������ı�
        textCanvasGroup.alpha = 0;
        characterTextCanvasGroup.alpha = 0;
        // �ر������̳������Ҳ�ɸ�������ѡ�� Destroy(gameObject)��
        gameObject.SetActive(false);
    }



    /// <summary>
    /// ���̳����̿���
    /// </summary>
    IEnumerator RunTutorial()
    {
        //yield return new WaitForSeconds(1f);  // ��ʼ�ȴ� 1 ��


        if (isTutorialSkipped) yield break;
        yield return ShowMessage("Welcome to the tutorial!", 1f); // ��ʾ��ӭ��Ϣ 2 ��
       


        if (isTutorialSkipped) yield break;
        yield return ShowMessage("Press A or D to move", 0f);
        if (isTutorialSkipped) yield break;
        yield return WaitForPlayerInput(KeyCode.A, KeyCode.D);

        if (isTutorialSkipped) yield break;
        yield return ShowMessage("Good Job!!!", 1f);
        if (isTutorialSkipped) yield break;
        yield return ShowMessage("Left Click to attack", 0f);
        if (isTutorialSkipped) yield break;
        yield return WaitForPlayerInput(KeyCode.Mouse0);

        if (isTutorialSkipped) yield break;
        yield return ShowMessage("Good Job!!!", 1f);
        if (isTutorialSkipped) yield break;
        yield return ShowMessage("Press the space bar", 0f); // ��ʾ���ո�
        if (isTutorialSkipped) yield break;
        yield return WaitForPlayerInput(KeyCode.Space); // �ȴ���Ұ��¿ո�
        if (isTutorialSkipped) yield break;
        yield return ShowMessage("Good Job!!!", 1f); // ��ʾ "Good Job!!!" ���ȴ� 2 ��

        if (isTutorialSkipped) yield break;
        yield return ShowMessage("Press Space while in the air to double jump", 0f);
        if (isTutorialSkipped) yield break;
        yield return WaitForPlayerInput(KeyCode.Space);
        if (isTutorialSkipped) yield break;
        yield return WaitForPlayerInput(KeyCode.Space);

        if (isTutorialSkipped) yield break;
        yield return ShowMessage("Good Job!!!", 1f);
        if (isTutorialSkipped) yield break;
        yield return ShowMessage("Now let's learn some skills", 1f);
        if (isTutorialSkipped) yield break;
        yield return ShowMessage("Press \"C\" and open \"Skill Tree\"", 1f);


        if (isTutorialSkipped) yield break;
        while (!dashSkill.dashUnlocked || !counterSkill.counterUnlocked)
        {
            yield return ShowMessageOnCharacterUI("Click \"Dash\" & \"Counter\" to unlock them", 0.5f);
        }
        if (isTutorialSkipped) yield break;
        yield return ShowMessageOnCharacterUI("Good Job!!!", 1f);

        //yield return WaitForPlayerInput(KeyCode.Escape);

        if (isTutorialSkipped) yield break;
        yield return ShowMessage("Press Shift to dash", 0f);
        if (isTutorialSkipped) yield break;
        yield return WaitForPlayerInput(KeyCode.LeftShift);

        if (isTutorialSkipped) yield break;
        yield return ShowMessage("Good Job!!!", 1f);
        if (isTutorialSkipped) yield break;
        yield return ShowMessage("Right Click to counter", 0f);
        if (isTutorialSkipped) yield break;
        yield return WaitForPlayerInput(KeyCode.Mouse1);

        if (isTutorialSkipped) yield break;
        yield return ShowMessage("Good Job!!!", 1f);

        if (isTutorialSkipped) yield break;
        yield return ShowMessage("Press Esc to pause", 0f);
        if (isTutorialSkipped) yield break;
        yield return WaitForPlayerInput(KeyCode.Escape);

        if (isTutorialSkipped) yield break;
        yield return ShowMessage("Good Job!!!", 1f);
        if (isTutorialSkipped) yield break;
        yield return ShowMessage("Now let's install some equipments", 1f);
        if (isTutorialSkipped) yield break;
        yield return ShowMessage("Press C open Equipment Bar", 0f);
        if (isTutorialSkipped) yield break;
        yield return WaitForPlayerInput(KeyCode.C);
        if (isTutorialSkipped) yield break;
        yield return WaitForPlayerInput(KeyCode.C);
        if (isTutorialSkipped) yield break;
        yield return ShowMessage("Great!!!", 1f);


        if (isTutorialSkipped) yield break;
        yield return ShowMessage("Now, Kill all the enemys.", 1f);

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
        tutorialText.color = (message == "Good Job!!!" || message == "Great!!!") ? Color.green : new Color(1.0f, 0.5f, 0.0f);
        tutorialText.fontSize = (message == "Good Job!!!" || message == "Great!!!") ? 40 : 30;
        tutorialText.fontStyle = (message == "Good Job!!!" || message == "Great!!!") ? FontStyles.Bold | FontStyles.Italic : FontStyles.Normal;

        if (message == "Now, Kill all the enemys.")
        {
            tutorialText.fontSize = 50;
            tutorialText.color = Color.red;
        }


        yield return FadeIn(); // �������ı�

        if (duration > 0) yield return new WaitForSecondsRealtime(duration); // ��� duration ���� 0���ȴ�ָ��ʱ��
    }

    /// <summary>
    /// �ȴ���Ұ���ָ������
    /// </summary>
    IEnumerator WaitForPlayerInput(params KeyCode[] keys)
    {
        waitingForInput = true;
        while (waitingForInput)
        {
            // ����̳̱��������˳�Э��
            if (isTutorialSkipped)
            {
                waitingForInput = false;
                yield break;
            }
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



    IEnumerator ShowMessageOnCharacterUI(string message, float duration)
    {
        // �������ı�
        yield return FadeOut(characterTextCanvasGroup);

        // ���ý�ɫUI������
        characterUIText.text = message;

        // ����� "Good Job!!!"���ı���ɫ�������С����ʽ
        characterUIText.color = (message == "Good Job!!!") ? Color.green : new Color(0f, 1f, 0.8156863f);
        characterUIText.fontSize = (message == "Good Job!!!") ? 40 : 20;
        characterUIText.fontStyle = (message == "Good Job!!!") ? FontStyles.Bold | FontStyles.Italic : FontStyles.Normal;

        // �����ɫUI�ı�
        yield return FadeIn(characterTextCanvasGroup);

        if (duration > 0)
            yield return new WaitForSecondsRealtime(duration);
        //yield return new WaitForSeconds(duration);
    }

    IEnumerator FadeIn(CanvasGroup group)
    {
        float timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime; // ����TimeScaleӰ��
            group.alpha = Mathf.Lerp(0, 1, timer / fadeDuration);
            yield return null;
        }
        group.alpha = 1;
    }

    IEnumerator FadeOut(CanvasGroup group)
    {
        float timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime; // ����TimeScaleӰ��
            group.alpha = Mathf.Lerp(1, 0, timer / fadeDuration);
            yield return null;
        }
        group.alpha = 0;
    }

    /// <summary>
    /// ���붯����ʹ�ı���͸����Ϊ��ȫ�ɼ���
    /// </summary>
    IEnumerator FadeIn()
    {
        float timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime; // ����TimeScaleӰ��
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
            timer += Time.unscaledDeltaTime; // ����TimeScaleӰ��
            textCanvasGroup.alpha = Mathf.Lerp(1, 0, timer / fadeDuration); // ����ȫ�ɼ���1�����䵽͸����0��
            yield return null; // �ȴ���һ֡
        }
        textCanvasGroup.alpha = 0; // ȷ������͸����Ϊ 0
    }
}
