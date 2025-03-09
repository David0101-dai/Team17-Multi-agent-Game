using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackholeSkill : Skill
{
    [SerializeField] private GameObject blackholePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;
    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float cloneAttackCooldown;
    [SerializeField] private float blackholeDuration;

    private BlackholeSkillController currentBlackhole;


    [Header("Black Hole")]
    private SkillTreeSlot unlockBlackHolebutton;
    [SerializeField] public bool blackHole;

        protected override void OnEnable()
    {
        base.OnEnable();
    }

    private IEnumerator InitializeSkillButtons()
    {
        // 等待直到 UI 初始化完成
        while (UI.Instance == null || UI.Instance.unlockBlackHolebutton == null)
        {
            yield return null;  // 每一帧检查一次
        }

        // 现在 UI 已经完全初始化，安全地访问按钮
        unlockBlackHolebutton = UI.Instance.unlockBlackHolebutton;

        if (unlockBlackHolebutton != null)
        {
            unlockBlackHolebutton.GetComponent<Button>().onClick.AddListener(UnlockBlackHole);
            unlockBlackHolebutton.OnSkillCancelled += OnBlackHoleSkillCancelled;
        }
        else
        {
            Debug.LogError("unlockBlackHolebutton is not assigned.");
        }

        // 等待直到 SaveManager 实例和 gameData 完全加载
        yield return StartCoroutine(WaitAndCheckUnlock());
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
            if (unlockBlackHolebutton != null)
            {
                unlockBlackHolebutton.LoadData(SaveManager.instance.CurrentGameData());
            }
            else
            {
                Debug.LogError("unlockBlackHolebutton is null during LoadData call.");
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


    protected override void Start()
    {
        base.Start();
        StartCoroutine(InitializeSkillButtons());
    }

    private void OnBlackHoleSkillCancelled()
    {
        blackHole = false;
    }

    private void UnlockBlackHole()
    {
        if (unlockBlackHolebutton.unlocked)
            blackHole = true;
    }
    protected override void SkillFunction()
    {
        var pos = player.transform.position + new Vector3(0, 1);
        var parent = PlayerManager.Instance.fx.transform;
        var newBlackhole = Instantiate(blackholePrefab, pos, Quaternion.identity, parent);
        if (!newBlackhole.TryGetComponent(out currentBlackhole)) return;
        currentBlackhole.Setup(
            player,
            maxSize,
            growSpeed,
            shrinkSpeed,
            amountOfAttacks,
            cloneAttackCooldown,
            blackholeDuration);
    }

    public bool BlackholeFinished()
    {
        if (!currentBlackhole) return false;
        return currentBlackhole.PlayerCanExitState;
    }

    public float GetBlackholeRadius()
    {
        return maxSize / 2;
    }

    protected override void CheckUnlock()
    {
        base.CheckUnlock();
        UnlockBlackHole();
    }
}
