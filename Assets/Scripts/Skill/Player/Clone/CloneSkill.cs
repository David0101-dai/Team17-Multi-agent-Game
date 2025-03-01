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
   
    public bool canApplyOnHitEffect { get; private set; }


    [Header("Duplicate Clone")]
    [SerializeField] private SkillTreeSlot unlockMultipleClonebutton;
    [SerializeField] private float multipleCloneMultiplier;
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float duplicateProbability;

    



    protected override void Start()
    {
        base.Start();

        unlockCloneAttackbutton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
        unlockMultipleClonebutton.GetComponent<Button>().onClick.AddListener(UnlockMultipleClone);
        

    }

    #region Unlock Skill Region
    private void UnlockCloneAttack()
    {
        if (unlockCloneAttackbutton.unlocked) {
            canAttack = true;
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
