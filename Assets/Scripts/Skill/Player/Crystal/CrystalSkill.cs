using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSkill : Skill
{
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private float duration;
    [SerializeField] private float durationNonMoving;

    [Header("Crystal Mirage")]
    private SkillTreeSlot unlockCloneInsteadbutton;
    [SerializeField] private bool cloneInsteadOfCrystal;


    [Header("Crystal Simple")]
    private SkillTreeSlot unlockCrystalButton;
    public bool crystalUnlocked { get; private set; }

    [Header("Growing Crystal")]
    [SerializeField] private Vector2 maxSize;
    [SerializeField] private float growSpeed;


    [Header("Explosive Crystal")]
     private SkillTreeSlot unlockExplosiveButton;
     private bool canExplode;

    [Header("Moving Crystal")]
    private SkillTreeSlot unlockMovingButton;
    [SerializeField] public bool canMove;
    [SerializeField] private float moveSpeed;

    [Header("Multi Stacking Crystal")]
    private SkillTreeSlot unlockMultiStackButton;
    [SerializeField] private bool canUseMultiStacks;
    [SerializeField] private int amountOfStacks;
    [SerializeField] public float multiStackCooldown;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();

    private GameObject currentCrystal;


        protected override void OnEnable()
    {
        base.OnEnable();
    }

    private IEnumerator InitializeSkillButtons()
    {
        // 等待直到 UI 初始化完成，确保所有按钮组件已经被正确绑定
        while (UI.Instance == null || 
            UI.Instance.unlockCrystalButton == null || 
            UI.Instance.unlockCloneInsteadbutton == null || 
            UI.Instance.unlockMovingButton == null || 
            UI.Instance.unlockExplosiveButton == null || 
            UI.Instance.unlockMultiStackButton == null)
        {
            yield return null;  // 每一帧检查一次，直到UI实例和按钮完全加载
        }

        // 赋值并进行空值检查
        unlockCrystalButton = UI.Instance.unlockCrystalButton;
        unlockCloneInsteadbutton = UI.Instance.unlockCloneInsteadbutton;
        unlockMovingButton = UI.Instance.unlockMovingButton;
        unlockExplosiveButton = UI.Instance.unlockExplosiveButton;
        unlockMultiStackButton = UI.Instance.unlockMultiStackButton;

        // 添加按钮事件监听和空值检查
        if (unlockCrystalButton != null)
        {
            unlockCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockCrystal);
            unlockCrystalButton.OnSkillCancelled += OnCrystalSkillCancelled;
        }
        else
        {
            Debug.LogError("unlockCrystalButton is not assigned.");
        }

        if (unlockCloneInsteadbutton != null)
        {
            unlockCloneInsteadbutton.GetComponent<Button>().onClick.AddListener(UnlockCrystalMirage);
            unlockCloneInsteadbutton.OnSkillCancelled += OnCrystalMirageSkillCancelled;
        }
        else
        {
            Debug.LogError("unlockCloneInsteadbutton is not assigned.");
        }

        if (unlockMovingButton != null)
        {
            unlockMovingButton.GetComponent<Button>().onClick.AddListener(UnlockMovingCrystal);
            unlockMovingButton.OnSkillCancelled += OnMovingCrystalSkillCancelled;
        }
        else
        {
            Debug.LogError("unlockMovingButton is not assigned.");
        }

        if (unlockExplosiveButton != null)
        {
            unlockExplosiveButton.GetComponent<Button>().onClick.AddListener(UnlockExplosiveCrystal);
            unlockExplosiveButton.OnSkillCancelled += OnExplosiveCrystalSkillCancelled;
        }
        else
        {
            Debug.LogError("unlockExplosiveButton is not assigned.");
        }

        if (unlockMultiStackButton != null)
        {
            unlockMultiStackButton.GetComponent<Button>().onClick.AddListener(UnlockMulitStack);
            unlockMultiStackButton.OnSkillCancelled += OnMulitStackSkillCancelled;
        }
        else
        {
            Debug.LogError("unlockMultiStackButton is not assigned.");
        }

        // 确保按钮初始化完成后，再进行数据加载
        yield return StartCoroutine(WaitAndCheckUnlock());
    }

    private IEnumerator WaitAndCheckUnlock()
    {
        // 等待直到 SaveManager 实例存在并且 gameData 已加载
        while (SaveManager.instance == null || SaveManager.instance.CurrentGameData() == null)
        {
            yield return null;
        }

        // 主动刷新技能槽数据，先进行空值检查
        if (unlockCrystalButton != null)
        {
            unlockCrystalButton.LoadData(SaveManager.instance.CurrentGameData());
        }

        if (unlockCloneInsteadbutton != null)
        {
            unlockCloneInsteadbutton.LoadData(SaveManager.instance.CurrentGameData());
        }

        if (unlockMovingButton != null)
        {
            unlockMovingButton.LoadData(SaveManager.instance.CurrentGameData());
        }

        if (unlockExplosiveButton != null)
        {
            unlockExplosiveButton.LoadData(SaveManager.instance.CurrentGameData());
        }

        if (unlockMultiStackButton != null)
        {
            unlockMultiStackButton.LoadData(SaveManager.instance.CurrentGameData());
        }

        // 等待一帧，确保 SkillTreeSlot 的状态更新完成
        yield return new WaitForEndOfFrame();

        // 调用 CheckUnlock
        CheckUnlock();
    }


    protected override void Start() {
        base.Start();
        StartCoroutine(InitializeSkillButtons());
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
        canExplode = false;
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
        {
            canExplode = true;
            canMove = true;
        }
    }

    private void UnlockExplosiveCrystal()
    {
        if (unlockExplosiveButton.unlocked)
            canExplode = true;
    }

    private void UnlockMulitStack()
    {
        if (unlockMultiStackButton.unlocked)
        {
            canExplode = true;
            canUseMultiStacks = true;
        }
    }
    #endregion

    [SerializeField] private float fireInterval = 0.1f; // 每次发射间隔的秒数

    
    

    protected override void SkillFunction()
    {
        cooldown = 0.5f;
        if (canUseMultiStacks)
        {
            // 启动协程，依次发射多个水晶
            StartCoroutine(FireMultipleCrystals());
            cooldown = multiStackCooldown;
            return;
        }

        // 单个水晶的逻辑...
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

    private IEnumerator FireMultipleCrystals()
    {
        // 使用循环依次生成水晶，并在每次发射后等待 fireInterval 秒
        for (int i = 0; i < amountOfStacks; i++)
        {
            // 根据玩家位置稍微偏移，避免重叠（可以根据需要调整偏移量）
            Vector3 spawnPos = player.transform.position + new Vector3(0, 1, 0);
            spawnPos.x += (i - (amountOfStacks - 1) / 2f) * 0.5f;
            CreateCrystal(crystalPrefab);
            yield return new WaitForSeconds(fireInterval);
        }

        // 发射完毕后，若需要重置或者补充水晶可以在这里调用对应方法
        RefilCrystal();
    }




    /*
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
    }*/

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
        var parent = FxManager.Instance.fx.transform;
        var newCrystal = Instantiate(crystalToSpawn, pos, Quaternion.identity, parent);
        
        if (newCrystal.TryGetComponent(out CrystalSkillController sc))
        {
            sc.Setup(
                player,
                durationNonMoving,
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
