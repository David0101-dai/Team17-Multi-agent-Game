using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CounterSkill : Skill
{
    [Header("Counter")]
    public bool counterUnlocked;
    private SkillTreeSlot counterUnlockedButton;

    [Header("CounterBuff")]
    public bool counterBuffUnlocked;
    private SkillTreeSlot counterBuffUnlockedButton;

    [Header("CounterMirage")]
    public bool counterMiragelUnlocked;
    private SkillTreeSlot counterMiragelUnlockedButton;

    protected override void OnEnable()
    {
        base.OnEnable();
    }
    protected override void Start()
    {
        base.Start();
        StartCoroutine(InitializeSkillButtons());
    }
private IEnumerator InitializeSkillButtons()
{
    // 等待直到 UI 初始化完成，确保所有按钮组件已经被正确绑定
    while (UI.Instance == null || UI.Instance.counterUnlockedButton == null || UI.Instance.counterBuffUnlockedButton == null || UI.Instance.counterMiragelUnlockedButton == null)
    {
        yield return null;  // 每一帧检查一次，直到UI实例和按钮完全加载
    }

    // 赋值并进行空值检查
    counterUnlockedButton = UI.Instance.counterUnlockedButton;
    counterBuffUnlockedButton = UI.Instance.counterBuffUnlockedButton;
    counterMiragelUnlockedButton = UI.Instance.counterMiragelUnlockedButton;

    // 添加按钮事件监听和空值检查
    if (counterUnlockedButton != null)
    {
        counterUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockCounter);
        counterUnlockedButton.OnSkillCancelled += OnCounterSkillCancelled;
    } 
    else
    {
        Debug.LogError("counterUnlockedButton is not assigned.");
    }

    if (counterBuffUnlockedButton != null)
    {
        counterBuffUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockCounterBuff);
        counterBuffUnlockedButton.OnSkillCancelled += OnCounterBuffSkillCancelled;
    } 
    else
    {
        Debug.LogError("counterBuffUnlockedButton is not assigned.");
    }

    if (counterMiragelUnlockedButton != null)
    {
        counterMiragelUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockCounterMirage);
        counterMiragelUnlockedButton.OnSkillCancelled += OnCounterMirageSkillCancelled;
    } 
    else
    {
        Debug.LogError("counterMiragelUnlockedButton is not assigned.");
    }

    // 确保按钮初始化完成后，再进行数据加载
    yield return StartCoroutine(WaitAndCheckUnlock());
}

private IEnumerator WaitAndCheckUnlock()
{
    // 等待直到 SaveManager 实例存在并且 gameData 已加载
    while (SaveManager.instance == null || SaveManager.instance.CurrentGameData() == null)
    {
        yield return null;
    }

    // 主动刷新技能槽数据，先进行空值检查
    if (counterUnlockedButton != null)
    {
        counterUnlockedButton.LoadData(SaveManager.instance.CurrentGameData());
    }

    if (counterBuffUnlockedButton != null)
    {
        counterBuffUnlockedButton.LoadData(SaveManager.instance.CurrentGameData());
    }

    if (counterMiragelUnlockedButton != null)
    {
        counterMiragelUnlockedButton.LoadData(SaveManager.instance.CurrentGameData());
    }

    // 等待一帧，确保 SkillTreeSlot 的状态更新完成
    yield return new WaitForEndOfFrame();

    // 调用 CheckUnlock
    CheckUnlock();
}


    protected override void SkillFunction()
    {
    }



    protected override void CheckUnlock()
    {
        UnlockCounter();
        UnlockCounterBuff();
        UnlockCounterMirage();
    }

    private void OnCounterSkillCancelled()
    {
        counterUnlocked = false;
        
    }

    private void OnCounterBuffSkillCancelled()
    {
        counterBuffUnlocked = false;
        
    }

    private void OnCounterMirageSkillCancelled()
    {
        counterMiragelUnlocked = false;
    }
        

    private void UnlockCounter()
    {
        if (counterUnlockedButton.unlocked)
            counterUnlocked = true;
    }

    private void UnlockCounterBuff()
    {
        if (counterBuffUnlockedButton.unlocked)
            counterBuffUnlocked = true;
    }

    private void UnlockCounterMirage()
    {
        if (counterMiragelUnlockedButton.unlocked)
            counterMiragelUnlocked = true;
    }
}
