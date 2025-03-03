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

        dashUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
        cloneOnDashUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
        //cloneOnArrivalUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnArrival);
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

    private IEnumerator WaitAndCheckUnlock()
    {
        // 等待直到 SaveManager 实例存在并且 gameData 已加载
        while (SaveManager.instance == null || SaveManager.instance.CurrentGameData == null)
        {
            yield return null;
        }
        // 主动刷新技能槽数据
        dashUnlockedButton.LoadData(SaveManager.instance.CurrentGameData);
        cloneOnDashUnlockedButton.LoadData(SaveManager.instance.CurrentGameData);
        
        // 等待一帧，确保 SkillTreeSlot 的状态更新完成
        yield return new WaitForEndOfFrame();
        
        CheckUnlock();
    }

}
