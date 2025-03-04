using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSkill : Skill
{
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private float duration;

    [Header("Crystal Mirage")]
    [SerializeField] private SkillTreeSlot unlockCloneInsteadbutton;
    [SerializeField] private bool cloneInsteadOfCrystal;


    [Header("Crystal Simple")]
    [SerializeField] private SkillTreeSlot unlockCrystalButton;
    public bool crystalUnlocked { get; private set; }

    [Header("Growing Crystal")]
    [SerializeField] private Vector2 maxSize;
    [SerializeField] private float growSpeed;


    [Header("Explosive Crystal")]
    [SerializeField] private SkillTreeSlot unlockExplosiveButton;
    [SerializeField] private bool canExplode;

    [Header("Moving Crystal")]
    [SerializeField] private SkillTreeSlot unlockMovingButton;
    [SerializeField] public bool canMove;
    [SerializeField] private float moveSpeed;

    [Header("Multi Stacking Crystal")]
    [SerializeField] private SkillTreeSlot unlockMultiStackButton;
    [SerializeField] private bool canUseMultiStacks;
    [SerializeField] private int amountOfStacks;
    [SerializeField] public float multiStackCooldown;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();

    private GameObject currentCrystal;


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
        unlockCrystalButton.LoadData(SaveManager.instance.CurrentGameData);
        unlockCloneInsteadbutton.LoadData(SaveManager.instance.CurrentGameData);
        unlockMovingButton.LoadData(SaveManager.instance.CurrentGameData);
        unlockExplosiveButton.LoadData(SaveManager.instance.CurrentGameData);
        unlockMultiStackButton.LoadData(SaveManager.instance.CurrentGameData);
        // 等待一帧，确保 SkillTreeSlot 的状态更新完成
        yield return new WaitForEndOfFrame();
        
        CheckUnlock();
    }

    protected override void Start() {
        base.Start();

        unlockCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockCrystal);
        unlockCloneInsteadbutton.GetComponent<Button>().onClick.AddListener(UnlockCrystalMirage);
        unlockMovingButton.GetComponent<Button>().onClick.AddListener(UnlockMovingCrystal);
        unlockExplosiveButton.GetComponent<Button>().onClick.AddListener(UnlockExplosiveCrystal);
        unlockMultiStackButton.GetComponent<Button>().onClick.AddListener(UnlockMulitStack);

        unlockCrystalButton.OnSkillCancelled += OnCrystalSkillCancelled;
        unlockCloneInsteadbutton.OnSkillCancelled += OnCrystalMirageSkillCancelled;
        unlockMovingButton.OnSkillCancelled += OnMovingCrystalSkillCancelled;
        unlockExplosiveButton.OnSkillCancelled += OnExplosiveCrystalSkillCancelled;
        unlockMultiStackButton.OnSkillCancelled += OnMulitStackSkillCancelled;

    }

    #region Unlock Skill Region

    protected override void CheckUnlock()
    {
        UnlockCrystal();
        UnlockCrystalMirage();
        UnlockMovingCrystal();
        UnlockExplosiveCrystal();
        UnlockMulitStack();
    }

    private void OnCrystalSkillCancelled()
    {
        crystalUnlocked = false;
    }

    private void OnCrystalMirageSkillCancelled()
    {
        cloneInsteadOfCrystal = false;
    }

    private void OnMovingCrystalSkillCancelled()
    {
        canMove = false;
    }

    private void OnExplosiveCrystalSkillCancelled()
    {
        canExplode = false;
    }

    private void OnMulitStackSkillCancelled()
    {
        canUseMultiStacks = false;
    }

    private void UnlockCrystal()
    {
        if (unlockCrystalButton.unlocked)
            crystalUnlocked = true;
    }

    private void UnlockCrystalMirage()
    {
        if (unlockCloneInsteadbutton.unlocked)
            cloneInsteadOfCrystal = true;
    }

    private void UnlockMovingCrystal()
    {
        if (unlockMovingButton.unlocked)
            canMove = true;
    }

    private void UnlockExplosiveCrystal()
    {
        if (unlockExplosiveButton.unlocked)
            canExplode = true;
    }

    private void UnlockMulitStack()
    {
        if (unlockMultiStackButton.unlocked)
            canUseMultiStacks = true;
    }
    #endregion

    protected override void SkillFunction()
    {
        if (CanUseMultiCrystal()) return;

        if (currentCrystal == null)
        {
            currentCrystal = CreateCrystal(crystalPrefab);
        }
        else
        {
            if (canMove)
            {
                cooldown = multiStackCooldown;
                return;
            }

            var playerPos = player.transform.position + new Vector3(0, 1);
            var crystalPos = currentCrystal.transform.position + new Vector3(0, -1);

            player.transform.position = crystalPos;

            //这里可以决定爆炸发生在回溯的起点还是重点
            currentCrystal.transform.position = playerPos;


            if (cloneInsteadOfCrystal)
            {
                var pos = currentCrystal.transform.position + new Vector3(0, -1);
                SkillManager.Instance.Clone.CreateClone(pos, player.transform.rotation, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
                if (currentCrystal.TryGetComponent(out CrystalSkillController sc))
                {
                    sc.FinishCrystal();
                }
            }
        }
    }

    private bool CanUseMultiCrystal()
    {
        if (canUseMultiStacks)
        {
            if (crystalLeft.Count > 0)
            {
                if (crystalLeft.Count == amountOfStacks)
                {
                    Invoke(nameof(ResetAbility), useTimeWindow);
                }

                cooldown = 0;
                var crystalToSpawn = crystalLeft[crystalLeft.Count - 1];
                CreateCrystal(crystalToSpawn);
                crystalLeft.Remove(crystalToSpawn);

                if (crystalLeft.Count <= 0)
                {
                    cooldown = multiStackCooldown;
                    RefilCrystal();
                }
                return true;
            }
        }
        return false;
    }

    public void CreateCrystal()
    {
        currentCrystal = CreateCrystal(crystalPrefab);
    }

    private GameObject CreateCrystal(GameObject crystalToSpawn)
    {
        var pos = player.transform.position + new Vector3(0, 1);
        var parent = PlayerManager.Instance.fx.transform;
        var newCrystal = Instantiate(crystalToSpawn, pos, Quaternion.identity, parent);
        
        if (newCrystal.TryGetComponent(out CrystalSkillController sc))
        {
            sc.Setup(
                player,
                duration,
                canExplode,
                canMove,
                moveSpeed,
                maxSize,
                growSpeed,
                FindClosestEnemy);
        }
        return newCrystal;
    }

    public void CurrentCrystalChooseRandomTarget()
    {
        if (!currentCrystal.TryGetComponent(out CrystalSkillController sc)) return;
        sc.ChooseRandomEnemy();
    }

    private void RefilCrystal()
    {
        int amountToAdd = amountOfStacks - crystalLeft.Count;

        for (int i = 0; i < amountToAdd; i++)
        {
            crystalLeft.Add(crystalPrefab);
        }
    }

    private void ResetAbility()
    {
        if (cooldownTimer > 0) return;
        cooldownTimer = multiStackCooldown;
        RefilCrystal();
    }
}
