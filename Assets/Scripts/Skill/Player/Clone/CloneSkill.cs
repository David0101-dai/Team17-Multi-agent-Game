using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CloneSkill : Skill
{
    [Header("Clone Value")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private Color cloneColor;
    [SerializeField] private bool isDelayAttack;
    [SerializeField] private float attackDelay;
    [SerializeField] private float attackMultiplier;

    [SerializeField] private bool createCloneOnDashStart;
    [SerializeField] private bool createCloneOnDashOver;
    [SerializeField] private bool createCloneOnCounterAttack;

    [Header("Clone Attack")]
    private SkillTreeSlot unlockCloneAttackbutton;
    [SerializeField] private float cloneMultiplier;
    //[SerializeField] private bool canAttack;
   
    public bool canApplyOnHitEffect { get; private set; }


    [Header("Duplicate Clone")]
    private SkillTreeSlot unlockMultipleClonebutton;
    [SerializeField] private float multipleCloneMultiplier;
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float duplicateProbability;
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    private IEnumerator InitializeSkillButtons()
    {
        // 等待直到 UI 初始化完成
        while (UI.Instance == null ||
            UI.Instance.unlockCloneAttackbutton == null ||
            UI.Instance.unlockMultipleClonebutton == null)
        {
            yield return null;  // 每一帧检查一次
        }

        // 现在 UI 已经完全初始化，安全地访问按钮
        unlockCloneAttackbutton = UI.Instance.unlockCloneAttackbutton;
        unlockMultipleClonebutton = UI.Instance.unlockMultipleClonebutton;

        if (unlockCloneAttackbutton != null)
        {
            unlockCloneAttackbutton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
            unlockCloneAttackbutton.OnSkillCancelled += OnCloneAttackSkillCancelled;
        }
        else
        {
            Debug.LogError("unlockCloneAttackbutton is not assigned.");
        }

        if (unlockMultipleClonebutton != null)
        {
            unlockMultipleClonebutton.GetComponent<Button>().onClick.AddListener(UnlockMultipleClone);
            unlockMultipleClonebutton.OnSkillCancelled += OnMultipleCloneSkillCancelled;
        }
        else
        {
            Debug.LogError("unlockMultipleClonebutton is not assigned.");
        }

        // 在初始化按钮后，调用 WaitAndCheckUnlock 来加载技能数据
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
            if (unlockCloneAttackbutton != null)
            {
                unlockCloneAttackbutton.LoadData(SaveManager.instance.CurrentGameData());
            }
            else
            {
                Debug.LogError("unlockCloneAttackbutton is null during LoadData call.");
            }

            if (unlockMultipleClonebutton != null)
            {
                unlockMultipleClonebutton.LoadData(SaveManager.instance.CurrentGameData());
            }
            else
            {
                Debug.LogError("unlockMultipleClonebutton is null during LoadData call.");
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

    #region Unlock Skill Region

    protected override void CheckUnlock()
    {
        UnlockCloneAttack();
        UnlockMultipleClone();
    }

    private void OnCloneAttackSkillCancelled()
    {
        attackMultiplier = 0;
    }

    private void OnMultipleCloneSkillCancelled()
    {
        canDuplicateClone = false;
    }

    private void UnlockCloneAttack()
    {
        if (unlockCloneAttackbutton.unlocked) {
            //canAttack = true;
            attackMultiplier = cloneMultiplier;
        }
    }

    private void UnlockMultipleClone()
    {
        if (unlockMultipleClonebutton.unlocked) {
            canDuplicateClone = true;
            attackMultiplier = multipleCloneMultiplier;
        }
    }
    #endregion


    public void CreateClone(Vector3 position, Quaternion rotation, Vector3 offset)
    {
        var newClone = Instantiate(clonePrefab);
        newClone.transform.SetParent(PlayerManager.Instance.fx.transform);
        var controller = newClone.GetComponent<CloneSkillController>();
        controller.Setup(
            player,
            position,
            rotation,
            cloneDuration,
            cloneColor,
            isDelayAttack,
            offset,
            attackDelay,
            canDuplicateClone,
            duplicateProbability,
            FindClosestEnemy,
            attackMultiplier);
    }

    public void CreateCloneOnDashStart(Transform playerTransform)
    {
        if (!createCloneOnDashStart || !SkillManager.Instance.Clone.CanUseSkill()) return;
        CreateClone(playerTransform.position, playerTransform.rotation, Vector3.zero);
    }

    public void CreateCloneOnDashOver(Transform playerTransform)
    {
        if (!createCloneOnDashOver || !SkillManager.Instance.Clone.CanUseSkill()) return;
        CreateClone(playerTransform.position, playerTransform.rotation, Vector3.zero);
    }

    public void CreateCloneOnCounterAttack(Transform enemyTransform)
    {
        if (!createCloneOnCounterAttack || !SkillManager.Instance.Clone.CanUseSkill()) return;
        CreateClone(enemyTransform.position, enemyTransform.rotation, new Vector3(2 * player.Flip.facingDir, 0));
    }

    protected override void SkillFunction()
    {

    }
}
