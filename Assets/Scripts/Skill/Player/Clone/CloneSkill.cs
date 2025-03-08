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
    [SerializeField] private SkillTreeSlot unlockCloneAttackbutton;
    [SerializeField] private float cloneMultiplier;
    //[SerializeField] private bool canAttack;
   
    public bool canApplyOnHitEffect { get; private set; }


    [Header("Duplicate Clone")]
    [SerializeField] private SkillTreeSlot unlockMultipleClonebutton;
    [SerializeField] private float multipleCloneMultiplier;
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float duplicateProbability;
        protected override void OnEnable()
    {
        base.OnEnable();
        // 使用协程确保在 SaveManager 数据加载完成后再检查技能解锁状态
        StartCoroutine(WaitAndCheckUnlock());
    }

    private IEnumerator WaitAndCheckUnlock()
    {
        // 等待直到 SaveManager 实例存在并且 gameData 已加载
        while (SaveManager.instance == null || SaveManager.instance.CurrentGameData() == null)
        {
            yield return null;
        }
        // 主动刷新技能槽数据
        unlockCloneAttackbutton.LoadData(SaveManager.instance.CurrentGameData());
        unlockMultipleClonebutton.LoadData(SaveManager.instance.CurrentGameData());
        // 等待一帧，确保 SkillTreeSlot 的状态更新完成
        yield return new WaitForEndOfFrame();
        
        CheckUnlock();
    }

    protected override void Start()
    {
        base.Start();

        unlockCloneAttackbutton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
        unlockMultipleClonebutton.GetComponent<Button>().onClick.AddListener(UnlockMultipleClone);

        unlockCloneAttackbutton.OnSkillCancelled += OnCloneAttackSkillCancelled;
        unlockMultipleClonebutton.OnSkillCancelled += OnMultipleCloneSkillCancelled;
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
