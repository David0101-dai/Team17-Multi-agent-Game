using System.Collections;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public TextMeshProUGUI tutorialText; // 绑定 UI 文字组件
    private bool waitingForInput = false;

    void Start()
    {
        StartCoroutine(RunTutorial());
    }

    IEnumerator RunTutorial()
    {
        yield return new WaitForSeconds(1f);
        yield return ShowMessage("Welcome to the tutorial!", 1f);

        yield return ShowMessage("Press the space bar", 0f);
        yield return WaitForPlayerInput(KeyCode.Space);

        yield return ShowMessage("Good Job!!!", 1f);
        yield return ShowMessage("Press A or D to move", 0f);
        yield return WaitForPlayerInput(KeyCode.A, KeyCode.D);

        yield return ShowMessage("Good Job!!!", 1f);
        yield return ShowMessage("Press Shift to sprint", 0f);
        yield return WaitForPlayerInput(KeyCode.LeftShift);

        yield return ShowMessage("Good Job!!!", 1f);
        yield return ShowMessage("Left Click to attack", 0f);
        yield return WaitForPlayerInput(KeyCode.Mouse0);

        yield return ShowMessage("Good Job!!!", 1f);
        yield return ShowMessage("Right Click to block", 0f);
        yield return WaitForPlayerInput(KeyCode.Mouse1);

        yield return ShowMessage("Good Job!!!", 1f);
        tutorialText.gameObject.SetActive(false); // 教程结束，隐藏文本
    }

    IEnumerator ShowMessage(string message, float duration)
    {
        tutorialText.text = message;
        tutorialText.color = (message == "Good Job!!!") ? Color.green : Color.white;
        tutorialText.fontSize = (message == "Good Job!!!") ? 40 : 30;
        tutorialText.fontStyle = (message == "Good Job!!!") ? FontStyles.Bold | FontStyles.Italic : FontStyles.Normal;
        tutorialText.gameObject.SetActive(true);

        if (duration > 0) yield return new WaitForSeconds(duration);
    }

    IEnumerator WaitForPlayerInput(params KeyCode[] keys)
    {
        waitingForInput = true;
        while (waitingForInput)
        {
            foreach (KeyCode key in keys)
            {
                if (Input.GetKeyDown(key))
                {
                    waitingForInput = false;
                    break;
                }
            }
            yield return null;
        }
    }
}
