using System.Collections;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    private DashSkill dashSkill;
    private CounterSkill counterSkill;


    [Header("文本_1")]
    public TextMeshProUGUI tutorialText; // 绑定 UI 文字组件
    public CanvasGroup textCanvasGroup;  // 绑定 UI 透明度组件

    [Header("文本_2")]
    public TextMeshProUGUI characterUIText;         // 第二个文本框
    public CanvasGroup characterTextCanvasGroup;    // 第二个 CanvasGroup

    private bool waitingForInput = false;
    public float fadeDuration = 1f;  // 控制渐隐渐显的时间，值越小动画越快，值越大动画越慢

    void Start()
    {
        // 如果不是新游戏，直接跳过教程
        if (!All.IsNewGame)
        {
            // 可以选择把教程文本隐藏、或直接Destroy(TutorialManager)
            return;
        }

        dashSkill = FindObjectOfType<DashSkill>();
        counterSkill = FindObjectOfType<CounterSkill>();
        StartCoroutine(RunTutorial()); // 启动教程流程
    }

    /// <summary>
    /// 主教程流程控制
    /// </summary>
    IEnumerator RunTutorial()
    {
        yield return new WaitForSeconds(1f);  // 初始等待 1 秒
        
        

        yield return ShowMessage("Welcome to the tutorial!", 1f); // 显示欢迎信息 2 秒

       

        yield return ShowMessage("Press A or D to move", 0f);
        yield return WaitForPlayerInput(KeyCode.A, KeyCode.D);

        yield return ShowMessage("Good Job!!!", 1f);
        yield return ShowMessage("Left Click to attack", 0f);
        yield return WaitForPlayerInput(KeyCode.Mouse0);

        yield return ShowMessage("Good Job!!!", 1f);
        yield return ShowMessage("Press the space bar", 0f); // 提示按空格
        yield return WaitForPlayerInput(KeyCode.Space); // 等待玩家按下空格
        yield return ShowMessage("Good Job!!!", 1f); // 显示 "Good Job!!!" 并等待 2 秒

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

        yield return FadeOut(); // 最后完全隐藏文本
    }

    /// <summary>
    /// 显示指定文本，并在显示前后执行渐隐渐显动画
    /// </summary>
    /// <param name="message">要显示的文本</param>
    /// <param name="duration">显示时长（0 表示直到玩家输入）</param>
    IEnumerator ShowMessage(string message, float duration)
    {
        yield return FadeOut(); // 先淡出旧文本
        tutorialText.text = message; // 更改文字内容

        // 如果是 "Good Job!!!"，改变颜色、字体大小和样式
        tutorialText.color = (message == "Good Job!!!") ? Color.green : Color.white;
        tutorialText.fontSize = (message == "Good Job!!!") ? 40 : 30;
        tutorialText.fontStyle = (message == "Good Job!!!") ? FontStyles.Bold | FontStyles.Italic : FontStyles.Normal;

        yield return FadeIn(); // 淡入新文本

        if (duration > 0) yield return new WaitForSecondsRealtime(duration); // 如果 duration 大于 0，等待指定时间
    }

    /// <summary>
    /// 等待玩家按下指定按键
    /// </summary>
    IEnumerator WaitForPlayerInput(params KeyCode[] keys)
    {
        waitingForInput = true;
        while (waitingForInput)
        {
            foreach (KeyCode key in keys)
            {
                if (Input.GetKeyDown(key)) // 监听玩家输入
                {
                    waitingForInput = false;
                    break;
                }
            }
            yield return null; // 等待下一帧继续检测输入
        }
    }



    IEnumerator ShowMessageOnCharacterUI(string message, float duration)
    {
        // 淡出旧文本
        yield return FadeOut(characterTextCanvasGroup);

        // 设置角色UI的文字
        characterUIText.text = message;

        // 如果是 "Good Job!!!"，改变颜色、字体大小和样式
        characterUIText.color = (message == "Good Job!!!") ? Color.green : Color.white;
        characterUIText.fontSize = (message == "Good Job!!!") ? 40 : 20;
        characterUIText.fontStyle = (message == "Good Job!!!") ? FontStyles.Bold | FontStyles.Italic : FontStyles.Normal;

        // 淡入角色UI文本
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
            timer += Time.unscaledDeltaTime; // 不受TimeScale影响
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
            timer += Time.unscaledDeltaTime; // 不受TimeScale影响
            group.alpha = Mathf.Lerp(1, 0, timer / fadeDuration);
            yield return null;
        }
        group.alpha = 0;
    }

    /// <summary>
    /// 渐入动画（使文本从透明变为完全可见）
    /// </summary>
    IEnumerator FadeIn()
    {
        float timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime; // 不受TimeScale影响
            textCanvasGroup.alpha = Mathf.Lerp(0, 1, timer / fadeDuration); // 从透明（0）渐变到完全可见（1）
            yield return null; // 等待下一帧
        }
        textCanvasGroup.alpha = 1; // 确保最终透明度为 1
    }

    /// <summary>
    /// 渐出动画（使文本从完全可见变为透明）
    /// </summary>
    IEnumerator FadeOut()
    {
        float timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime; // 不受TimeScale影响
            textCanvasGroup.alpha = Mathf.Lerp(1, 0, timer / fadeDuration); // 从完全可见（1）渐变到透明（0）
            yield return null; // 等待下一帧
        }
        textCanvasGroup.alpha = 0; // 确保最终透明度为 0
    }
}
