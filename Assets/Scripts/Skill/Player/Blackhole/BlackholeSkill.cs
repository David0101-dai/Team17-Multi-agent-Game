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
    [SerializeField] private SkillTreeSlot unlockBlackHolebutton;
    [SerializeField] public bool blackHole;

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
        unlockBlackHolebutton.LoadData(SaveManager.instance.CurrentGameData);
        // 等待一帧，确保 SkillTreeSlot 的状态更新完成
        yield return new WaitForEndOfFrame();
        
        CheckUnlock();
    }


    protected override void Start()
    {
        base.Start();
        unlockBlackHolebutton.GetComponent<Button>().onClick.AddListener(UnlockBlackHole);
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
