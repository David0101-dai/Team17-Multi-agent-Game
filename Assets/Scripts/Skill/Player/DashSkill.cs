using UnityEngine;
using UnityEngine.UI;

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
        SaveManager.OnSaveDataLoaded += CheckUnlock;
    }

    private void OnDisable()
    {
        SaveManager.OnSaveDataLoaded -= CheckUnlock;
    }
}
