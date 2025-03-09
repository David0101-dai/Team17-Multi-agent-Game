using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class DashSkill: Skill
{
    [Header("Dash")]
    public bool dashUnlocked;
    private SkillTreeSlot dashUnlockedButton;
    [Header("CloneOnDash")]
    public bool cloneOnDashUnlocked;
    private SkillTreeSlot cloneOnDashUnlockedButton;
    protected override void SkillFunction(){}

    protected override void Start()
    {
        base.Start();
        StartCoroutine(InitializeSkillButtons());
    }

    private IEnumerator InitializeSkillButtons()
    {
        // 等待直到 UI 初始化完成
        while (UI.Instance == null ||
         UI.Instance.dashUnlockedButton == null ||
          UI.Instance.cloneOnDashUnlockedButton == null)
        {
            yield return null;  // 每一帧检查一次
        }

        // 现在 UI 已经完全初始化，安全地访问按钮
        dashUnlockedButton = UI.Instance.dashUnlockedButton;
        cloneOnDashUnlockedButton = UI.Instance.cloneOnDashUnlockedButton;

        if (dashUnlockedButton != null)
        {
            dashUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
            dashUnlockedButton.OnSkillCancelled += OnDashSkillCancelled;
        } 
        else
        {
            Debug.LogError("dashUnlockedButton is not assigned.");
        }

        if (cloneOnDashUnlockedButton != null)
        {
            cloneOnDashUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
            cloneOnDashUnlockedButton.OnSkillCancelled += OnCloneOnDashSkillCancelled;
        } 
        else
        {
            Debug.LogError("cloneOnDashUnlockedButton is not assigned.");
        }

        // 在初始化按钮后，调用 WaitAndCheckUnlock 来加载技能数据
    yield return StartCoroutine(WaitAndCheckUnlock());
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    private IEnumerator WaitAndCheckUnlock()
    {
        // 等待直到 SaveManager 实例存在并且 gameData 已加载
        while (SaveManager.instance == null || SaveManager.instance.CurrentGameData() == null)
        {
            yield return null;  // 每一帧检查一次
        }
  // 确保 CurrentGameData 不为 null
    if (SaveManager.instance.CurrentGameData() != null)
    {
        if (dashUnlockedButton != null)
        {
            dashUnlockedButton.LoadData(SaveManager.instance.CurrentGameData());
        }
        else
        {
            Debug.LogError("dashUnlockedButton is null during LoadData call.");
        }

        if (cloneOnDashUnlockedButton != null)
        {
            cloneOnDashUnlockedButton.LoadData(SaveManager.instance.CurrentGameData());
        }
        else
        {
            Debug.LogError("cloneOnDashUnlockedButton is null during LoadData call.");
        }
    }
    else
    {
        Debug.LogError("CurrentGameData is null!");
    }

    // 等待一帧，确保 SkillTreeSlot 的状态更新完成
    yield return new WaitForEndOfFrame();
    // 检查技能是否解锁
    CheckUnlock();
    }

    private void OnDashSkillCancelled()
    {
        dashUnlocked = false;
        // 此处可以加入关闭技能效果的逻辑
    }

    private void OnCloneOnDashSkillCancelled()
    {
        cloneOnDashUnlocked = false;
        // 此处可以加入关闭技能效果的逻辑
    }

    protected override void CheckUnlock()
    {
        UnlockDash();
        UnlockCloneOnDash();
        // Debug.Log($"[DashSkill] dashUnlocked: {dashUnlocked}, cloneOnDashUnlocked: {cloneOnDashUnlocked}");
    }

    private void UnlockDash()
    {
        if (dashUnlockedButton.unlocked)
            dashUnlocked = true;
    }

    private void UnlockCloneOnDash()
    {
        if (cloneOnDashUnlockedButton.unlocked)
            cloneOnDashUnlocked = true;
    }
}