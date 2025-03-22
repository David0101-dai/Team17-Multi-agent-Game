using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Defence3 : Skill
{
    [Header("Defense")]
    public bool defence__new;   // 依旧用这个变量表示技能是否解锁
    private SkillTreeSlot defence;       // SkillTreeSlot 脚本引用

    private bool defense1 = false; // 记录是否已经给玩家加过护甲
    private const int armorBonus = 5;    // 要加的护甲数值

    protected override void Start()
    {
        base.Start();
        StartCoroutine(InitializeSkillButtons());
    }

    /// <summary>
    /// 每帧检测，如果技能已解锁，就执行技能功能
    /// </summary>
    protected override void Update()
    {
        if (defence__new)
        {
            SkillFunction();
        }
    }

    /// <summary>
    /// 具体的技能功能：给玩家增加 +5 护甲（只加一次）
    /// </summary>
    protected override void SkillFunction()
    {
        // 如果技能没解锁，或已经加过护甲，就不再执行
        if (!defence__new || defense1) return;

        Damageable dmg = player.GetComponent<Damageable>();
        if (dmg != null)
        {
            dmg.Armor.AddModifier(armorBonus);

            Debug.Log($"已为玩家增加 {armorBonus} 点护甲");

            defense1 = true;
        }

    }

    /// <summary>
    /// 初始化技能按钮，并加载解锁状态
    /// </summary>
    private IEnumerator InitializeSkillButtons()
    {
        // 等待 UI 和对应按钮初始化
        while (UI.Instance == null || UI.Instance.defence3 == null)
        {
            yield return null;
        }

        // 绑定 UI 上的 SkillTreeSlot
        defence = UI.Instance.defence3;

        if (defence != null)
        {
            // 点击按钮 -> 解锁技能
            defence.GetComponent<Button>().onClick.AddListener(UnlockAttackBoost);
            // 监听技能被取消
            defence.OnSkillCancelled += OnAttackBoostSkillCancelled;
        }


        // 等待存档管理器初始化
        while (SaveManager.instance == null || SaveManager.instance.CurrentGameData() == null)
        {
            yield return null;
        }

        // 从存档加载当前技能状态
        if (SaveManager.instance.CurrentGameData() != null)
        {
            defence.LoadData(SaveManager.instance.CurrentGameData());
        }


        // 等待一帧，确保加载完毕
        yield return new WaitForEndOfFrame();
        CheckUnlock();
    }

    /// <summary>
    /// 当技能被取消时，移除护甲加成，重置标记
    /// </summary>
    private void OnAttackBoostSkillCancelled()
    {
        defence__new = false;
        Damageable dmg = player.GetComponent<Damageable>();

        if (dmg != null)
        {
            dmg.Armor.RemoveModifier(armorBonus);

            Debug.Log($"已移除 {armorBonus} 点护甲加成");
        }
        defense1 = false;
    }

    /// <summary>
    /// 检测技能是否解锁
    /// </summary>
    protected override void CheckUnlock()
    {
        if (defence != null)
        {
            defence__new = defence.unlocked;
        }

    }

    /// <summary>
    /// 解锁技能（点击按钮时调用）
    /// </summary>
    private void UnlockAttackBoost()
    {
        if (defence.unlocked)
        {
            defence__new = true;
            // 不在这里直接加护甲，而是在 SkillFunction() 中执行
        }
    }
}
