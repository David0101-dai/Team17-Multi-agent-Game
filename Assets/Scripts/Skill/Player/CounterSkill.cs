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
