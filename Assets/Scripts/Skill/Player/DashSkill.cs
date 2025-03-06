using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class DashSkill : Skill
{
    [Header("Dash")]
    public bool dashUnlocked;
    [SerializeField] public SkillTreeSlot dashUnlockedButton;

    [Header("CloneOnDash")]
    public bool cloneOnDashUnlocked;
    [SerializeField] public SkillTreeSlot cloneOnDashUnlockedButton;

    protected override void SkillFunction()
    {
    }

protected override void Start()
{
    base.Start();

    // 确保按钮和SkillTreeSlot不为空
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
}

private IEnumerator WaitAndCheckUnlock()
{
    // 等待直到 SaveManager 实例存在并且 gameData 已加载
    while (SaveManager.instance == null || SaveManager.instance.CurrentGameData() == null)
    {
        Debug.LogWarning("Waiting for SaveManager and gameData to load...");
        yield return null;  // 每一帧检查一次
    }

    // 确保 CurrentGameData 不为 null
    if (SaveManager.instance.CurrentGameData() != null)
    {
        // 调用 LoadData 并传递已加载的 gameData
        dashUnlockedButton.LoadData(SaveManager.instance.CurrentGameData());
        cloneOnDashUnlockedButton.LoadData(SaveManager.instance.CurrentGameData());
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
        Debug.Log($"[DashSkill] dashUnlocked: {dashUnlocked}, cloneOnDashUnlocked: {cloneOnDashUnlocked}");
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

    protected override void OnEnable()
    {
        base.OnEnable();
        // 使用协程确保在 SaveManager 数据加载完成后再检查技能解锁状态
        StartCoroutine(WaitAndCheckUnlock());
    }

    // private IEnumerator WaitAndCheckUnlock()
    // {
    //     // 等待直到 SaveManager 实例存在并且 gameData 已加载
    //     while (SaveManager.instance == null || SaveManager.instance.CurrentGameData == null)
    //     {
    //         yield return null;
    //     }
    //     // 主动刷新技能槽数据
    //     dashUnlockedButton.LoadData(SaveManager.instance.CurrentGameData);
    //     cloneOnDashUnlockedButton.LoadData(SaveManager.instance.CurrentGameData);
        
    //     // 等待一帧，确保 SkillTreeSlot 的状态更新完成
    //     yield return new WaitForEndOfFrame();
        
    //     CheckUnlock();
    // }

}
