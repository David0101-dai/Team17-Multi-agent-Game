using System;
using System.Collections;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class DeadState : PlayerState
{

    private float dissolveRate = 0.0125f;
    private float refreshRate = 0.025f;

    public DeadState(FSM fsm, Player character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        AudioManager.instance.PlaySFX(14, null);

        // 启动溶解效果协程
        Character.StartCoroutine(Dissolve());

        // 使用异步任务进行 dissolve 计时器更新
        Task.Run(async () =>
        {
            float counter = 1;
            while (Character.Sr.material.GetFloat("_DissoiveAmount") > 0)
            {
                counter -= dissolveRate;
                Character.Sr.material.SetFloat("_DissoiveAmount", counter);
                await Task.Delay((int)(refreshRate * 1000));
            }
        });

        // 使用协程等待 UI 初始化完成
        EnsureUIInitialized();
        Character.StartCoroutine(InitializeUI());
    }

    private void EnsureUIInitialized()
    {
        if (UI.Instance != null)
        {
            Debug.Log("UI 确保启用");
            var ui = UI.Instance;  // 使用单例 UI
            if (ui != null)
            {
                ui.gameObject.SetActive(true);
                ui.getFadeScreen().gameObject.SetActive(true);
                Debug.Log("UI 和 fadeScreen 确保启用");
            }
            else
            {
                Debug.LogError("UI 组件未找到");
            }
        }
        else
        {
            Debug.LogError("UI 实例未初始化");
        }
    }

    private IEnumerator InitializeUI()
    {
        Debug.Log("进入 InitializeUI 协程");

        // 等待 UI 单例初始化
        yield return new WaitUntil(() => UI.Instance != null);

        var ui = UI.Instance;  // 使用 UI 单例
        if (ui == null)
        {
            Debug.LogError("UI 组件未找到");
            yield break;  // 退出协程，避免后续代码执行
        }

        Debug.Log("UI 组件已找到");
        float waitTime = 0f;

        // 确保 UI 和 fadeScreen 正确初始化
        while (ui.getFadeScreen() == null && waitTime < 3f) // 等待 3 秒
        {
            yield return new WaitForSeconds(0.1f);
            waitTime += 0.1f;
            Debug.Log($"检查中... waitTime: {waitTime} fadeScreen 组件状态：{ui.getFadeScreen() != null}");
        }

        if (ui.getFadeScreen() != null)
        {
            Debug.Log("fadeScreen 初始化完成，执行 FadeOut 操作");
            ui.SwitchOnEndScreen();
        }
        else
        {
            Debug.LogError("fadeScreen 仍未正确初始化，即使等待了 3 秒");
        }
    }




    public override void Update()
    {
        base.Update();
        Rb.velocity = Vector2.zero;
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }

    IEnumerator Dissolve()
    {
        float counter = 1;
        while (Character.Sr.material.GetFloat("_DissoiveAmount") > 0)
        {
            counter -= dissolveRate;
            Character.Sr.material.SetFloat("_DissoiveAmount", counter);
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
