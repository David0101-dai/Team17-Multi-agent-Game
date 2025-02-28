using UnityEngine;
using UnityEngine.UI;


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
    [SerializeField] private bool canAttack;
    [Header("Aggresive Clone")]         
    [SerializeField] private SkillTreeSlot unlockAggresiveClonebutton;
    [SerializeField] private float aggresiveCloneMultiplier;
    public bool canApplyOnHitEffect { get; private set; }


    [Header("Duplicate Clone")]
    [SerializeField] private SkillTreeSlot unlockMultipleClonebutton;
    [SerializeField] private float multipleCloneMultiplier;
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float duplicateProbability;

    [Header("Crystal Instead Of Clone")]
    [SerializeField] private SkillTreeSlot unlockCrystalInsteadClonebutton;
    [SerializeField] public bool crystalInsteadOfClone;



    protected override void Start()
    {
        base.Start();

        unlockCloneAttackbutton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
        unlockAggresiveClonebutton.GetComponent<Button>().onClick.AddListener(UnlockAggresiveClone);
        unlockMultipleClonebutton.GetComponent<Button>().onClick.AddListener(UnlockMultipleClone);
        unlockCrystalInsteadClonebutton.GetComponent<Button>().onClick.AddListener(UnlockCrystalInsteadClone);

    }

    #region Unlock Skill Region
    private void UnlockCloneAttack()
    {
        if (unlockCloneAttackbutton.unlocked) {
            canAttack = true;
            attackMultiplier = cloneMultiplier;
        }
            
    }

    private void UnlockAggresiveClone()
    {
        if (unlockAggresiveClonebutton.unlocked) {
            canApplyOnHitEffect = true;
            attackMultiplier = aggresiveCloneMultiplier;
        }
            
    }

    private void UnlockMultipleClone()
    {
        if (unlockMultipleClonebutton.unlocked) {
            canDuplicateClone = true;
            attackMultiplier = multipleCloneMultiplier;
        }
    }

    private void UnlockCrystalInsteadClone()
    {
        if (unlockCrystalInsteadClonebutton.unlocked)
            crystalInsteadOfClone = true;
    }


    #endregion

    public void CreateClone(Vector3 position, Quaternion rotation, Vector3 offset)
    {
        if (crystalInsteadOfClone)
        {
            SkillManager.Instance.Crystal.CreateCrystal();
            return;
        }

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
