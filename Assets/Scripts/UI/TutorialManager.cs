using System.Collections;
using UnityEngine;
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

    private bool waitingForInput = false;
    public float fadeDuration = 1f;  // ���ƽ������Ե�ʱ�䣬ֵԽС����Խ�죬ֵԽ�󶯻�Խ��

    void Start()
    {
        // �����������Ϸ��ֱ�������̳�
        if (!All.IsNewGame)
        {
            // ����ѡ��ѽ̳��ı����ء���ֱ��Destroy(TutorialManager)
            return;
        }

        dashSkill = FindObjectOfType<DashSkill>();
        counterSkill = FindObjectOfType<CounterSkill>();
        StartCoroutine(RunTutorial()); // �����̳�����
    }

    /// <summary>
    /// ���̳����̿���
    /// </summary>
    IEnumerator RunTutorial()
    {
        yield return new WaitForSeconds(1f);  // ��ʼ�ȴ� 1 ��
        
        

        yield return ShowMessage("Welcome to the tutorial!", 1f); // ��ʾ��ӭ��Ϣ 2 ��

       

        yield return ShowMessage("Press A or D to move", 0f);
        yield return WaitForPlayerInput(KeyCode.A, KeyCode.D);

        yield return ShowMessage("Good Job!!!", 1f);
        yield return ShowMessage("Left Click to attack", 0f);
        yield return WaitForPlayerInput(KeyCode.Mouse0);

        yield return ShowMessage("Good Job!!!", 1f);
        yield return ShowMessage("Press the space bar", 0f); // ��ʾ���ո�
        yield return WaitForPlayerInput(KeyCode.Space); // �ȴ���Ұ��¿ո�
        yield return ShowMessage("Good Job!!!", 1f); // ��ʾ "Good Job!!!" ���ȴ� 2 ��

        yield return ShowMessage("Press Space while in the air to double jump", 0f);
        yield return WaitForPlayerInput(KeyCode.Space);
        yield return WaitForPlayerInput(KeyCode.Space);

        yield return ShowMessage("Good Job!!!", 1f);
        yield return ShowMessage("Now let's learn some skills", 1f);
        yield return ShowMessage("Press \"C\" and open \"Skill Tree\"", 1f);
        
        

        while (dashSkill != null && !dashSkill.dashUnlocked && counterSkill != null && !counterSkill.counterUnlocked)
        {
            yield return ShowMessageOnCharacterUI("Click \"Dash\" & \"Counter\" to unlock them", 0.5f);
        }

        yield return ShowMessageOnCharacterUI("Good Job!!!", 1f);

        //yield return WaitForPlayerInput(KeyCode.Escape);


        yield return ShowMessage("Press Shift to dash", 0f);
        yield return WaitForPlayerInput(KeyCode.LeftShift);

        yield return ShowMessage("Good Job!!!", 1f);
        yield return ShowMessage("Right Click to counter", 0f);
        yield return WaitForPlayerInput(KeyCode.Mouse1);

        yield return ShowMessage("Good Job!!!", 1f);
       

        yield return ShowMessage("Press Esc to pause", 0f);
        yield return WaitForPlayerInput(KeyCode.Escape);
        
        yield return ShowMessage("Good Job!!!", 1f);
        yield return ShowMessage("Now let's install some equipments", 1f);
        yield return ShowMessage("Press \"C\" open Equipment Bar", 0f);
        yield return WaitForPlayerInput(KeyCode.C);
        yield return WaitForPlayerInput(KeyCode.C);

        

        yield return ShowMessage("Now, kill all the enemys.", 1f);

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
        characterUIText.color = (message == "Good Job!!!") ? Color.green : Color.white;
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
