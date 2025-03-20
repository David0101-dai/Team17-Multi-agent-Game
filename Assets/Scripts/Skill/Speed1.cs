using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Speed1 : Skill
{
    [Header("Attack Speed Boost")]
    // 是否解锁此技能
    public bool attackSpeedBoostUnlocked;

    // 技能树中的槽位（UI）
    private SkillTreeSlot attackSpeedSlot;

    // 是否已经应用过加成，防止重复加
    private bool boostApplied = false;

    protected override void SkillFunction()
    {
        // 如果技能未解锁或已经应用过加成，则不再执行
        if (!attackSpeedBoostUnlocked || boostApplied) return;

        // 获取 Player 脚本（你的玩家身上挂的脚本）
        Player p = player.GetComponent<Player>();
        if (p != null)
        {
            // 将攻速从 1.0 提高到 1.5（可根据需求改为别的数值）
            p.attackSpeed = 1.5f;
            Debug.Log("Attack Speed Boost: x1.5");
            boostApplied = true;
        }
       
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(InitializeSkillButtons());
    }

    private IEnumerator InitializeSkillButtons()
    {
        // 等待 UI 和技能槽位加载
        while (UI.Instance == null || UI.Instance.speed1 == null)
        {
            yield return null;
        }

        attackSpeedSlot = UI.Instance.speed1;

        // 如果拿到了槽位，就绑定按钮点击事件
        if (attackSpeedSlot != null)
        {
            attackSpeedSlot.GetComponent<Button>().onClick.AddListener(UnlockAttackSpeedBoost);
            attackSpeedSlot.OnSkillCancelled += OnAttackSpeedSkillCancelled;
        }
        

        // 等待存档数据加载
        while (SaveManager.instance == null || SaveManager.instance.CurrentGameData() == null)
        {
            yield return null;
        }

        if (SaveManager.instance.CurrentGameData() != null)
        {
            // 读取存档，恢复解锁状态
            attackSpeedSlot.LoadData(SaveManager.instance.CurrentGameData());
        }
       

        yield return new WaitForEndOfFrame();
        CheckUnlock();
    }

    protected override void Update()
    {
        // 如果技能已解锁，则尝试执行技能逻辑
        if (attackSpeedBoostUnlocked)
        {
            SkillFunction();
        }
    }

    // 当技能被取消时，移除加成
    private void OnAttackSpeedSkillCancelled()
    {
        attackSpeedBoostUnlocked = false;
        if (boostApplied)
        {
            Player p = player.GetComponent<Player>();
            if (p != null)
            {
                // 恢复攻速
                p.attackSpeed = 1f;
                Debug.Log("Attack Speed Boost Cancelled: revert to normal");
            }
            boostApplied = false;
        }
    }

    // 检查当前槽位是否已解锁
    protected override void CheckUnlock()
    {
        if (attackSpeedSlot != null)
        {
            attackSpeedBoostUnlocked = attackSpeedSlot.unlocked;
        }
       
    }

    // 点击按钮后解锁此技能
    private void UnlockAttackSpeedBoost()
    {
        if (attackSpeedSlot.unlocked)
        {
            attackSpeedBoostUnlocked = true;
        }
    }
}
