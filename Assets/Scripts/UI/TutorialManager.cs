using System.Collections;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public TextMeshProUGUI tutorialText; // 绑定 UI 文字组件
    public CanvasGroup textCanvasGroup;  // 绑定 UI 透明度组件

    private bool waitingForInput = false;
    public float fadeDuration = 1f;  // 控制渐隐渐显的时间，值越小动画越快，值越大动画越慢

    void Start()
    {
        StartCoroutine(RunTutorial()); // 启动教程流程
    }

    /// <summary>
    /// 主教程流程控制
    /// </summary>
    IEnumerator RunTutorial()
    {
        yield return new WaitForSeconds(1f);  // 初始等待 1 秒
        yield return ShowMessage("Welcome to the tutorial!", 2f); // 显示欢迎信息 2 秒

        yield return ShowMessage("Press the space bar", 0f); // 提示按空格
        yield return WaitForPlayerInput(KeyCode.Space); // 等待玩家按下空格

        yield return ShowMessage("Good Job!!!", 2f); // 显示 "Good Job!!!" 并等待 2 秒
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

        if (duration > 0) yield return new WaitForSeconds(duration); // 如果 duration 大于 0，等待指定时间
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

    /// <summary>
    /// 渐入动画（使文本从透明变为完全可见）
    /// </summary>
    IEnumerator FadeIn()
    {
        float timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime; // 计时
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
            timer += Time.deltaTime; // 计时
            textCanvasGroup.alpha = Mathf.Lerp(1, 0, timer / fadeDuration); // 从完全可见（1）渐变到透明（0）
            yield return null; // 等待下一帧
        }
        textCanvasGroup.alpha = 0; // 确保最终透明度为 0
    }
}
