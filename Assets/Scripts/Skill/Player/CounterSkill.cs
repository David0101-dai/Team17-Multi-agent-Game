using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CounterSkill : Skill
{
    [Header("Counter")]
    public bool counterUnlocked;
    [SerializeField] public SkillTreeSlot counterUnlockedButton;

    [Header("CounterBuff")]
    public bool counterBuffUnlocked;
    [SerializeField] public SkillTreeSlot counterBuffUnlockedButton;

    [Header("CounterMirage")]
    public bool counterMiragelUnlocked;
    [SerializeField] public SkillTreeSlot counterMiragelUnlockedButton;

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
        counterUnlockedButton.LoadData(SaveManager.instance.CurrentGameData);
        counterBuffUnlockedButton.LoadData(SaveManager.instance.CurrentGameData);
        counterMiragelUnlockedButton.LoadData(SaveManager.instance.CurrentGameData);
        // 等待一帧，确保 SkillTreeSlot 的状态更新完成
        yield return new WaitForEndOfFrame();
        
        CheckUnlock();
    }

    protected override void SkillFunction()
    {
    }

    protected override void Start()
    {
        base.Start();

        counterUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockCounter);
        counterBuffUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockCounterBuff);
        counterMiragelUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockCounterMirage);
    }

    protected override void CheckUnlock()
    {
        UnlockCounter();
        UnlockCounterBuff();
        UnlockCounterMirage();
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
