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

    [Header("CloneOnArrival")]
    public bool cloneOnArrivalUnlocked;
    [SerializeField] public SkillTreeSlot cloneOnArrivalUnlockedButton;

    protected override void SkillFunction()
    {
    }

    protected override void Start()
    {
        base.Start();

        dashUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
        cloneOnDashUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
        cloneOnArrivalUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnArrival);
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

    private void UnlockCloneOnArrival()
    {
        if (cloneOnArrivalUnlockedButton.unlocked)
            cloneOnArrivalUnlocked = true;
    }
}
