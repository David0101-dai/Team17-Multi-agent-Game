using System;
using System.Collections;
using Cysharp.Threading.Tasks;  // 如果不需要UniTask可以删掉这个using
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

        // 死亡次数检查，保存进度
        if (deadCount > 0)
        {
            deadCount--;
            PlayerManager.Instance.SaveFinaled();
        }

        // 播放死亡音效
        AudioManager.instance.PlaySFX(14, null);

        // 启动溶解效果协程（主线程，不会触发线程安全问题）
        Character.StartCoroutine(Dissolve());

        // 确保 UI 已经初始化并启用
        EnsureUIInitialized();

        // 等待 UI 初始化完成后，弹出死亡界面
        Character.StartCoroutine(InitializeUI());
    }

    private void EnsureUIInitialized()
    {
        if (UI.Instance != null)
        {
            var ui = UI.Instance;  // 使用单例 UI
            if (ui != null)
            {
                ui.gameObject.SetActive(true);
                ui.getFadeScreen().gameObject.SetActive(true);
            }
        }
    }

    private IEnumerator InitializeUI()
    {
        // 等待 UI 单例真正初始化
        yield return new WaitUntil(() => UI.Instance != null);
        var ui = UI.Instance;  // 使用 UI 单例
        float waitTime = 0f;

        // 等待 UI 的 fadeScreen 准备好（最多等 3 秒）
        while (ui.getFadeScreen() == null && waitTime < 3f)
        {
            yield return new WaitForSeconds(0.1f);
            waitTime += 0.1f;
        }

        // 如果 fadeScreen 已经初始化完毕，就显示死亡界面
        if (ui.getFadeScreen() != null)
        {
            ui.SwitchOnEndScreen();
        }
    }

    public override void Update()
    {
        base.Update();
        // 确保角色死亡后速度为 0
        Rb.velocity = Vector2.zero;
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }

    /// <summary>
    /// 在主线程用协程播放溶解效果。
    /// </summary>
    IEnumerator Dissolve()
    {
        float counter = 1f;
        while (Character.Sr.material.GetFloat("_DissoiveAmount") > 0)
        {
            counter -= dissolveRate;
            Character.Sr.material.SetFloat("_DissoiveAmount", counter);
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
