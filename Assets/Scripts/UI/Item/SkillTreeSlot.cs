using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;


public class SkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISaveManager
{
    private bool isRegistered = false;
    [SerializeField] public bool unlocked;
    [SerializeField] public SkillTreeSlot[] shouldBeUnlocked;
    [SerializeField] public SkillTreeSlot[] shouldBeLocked;
    private Image skillImage;
    [SerializeField] private int skillPrice;
    [SerializeField] private string skillName;
    [SerializeField][Multiline] private string skillDescription;
    [SerializeField] private Color lockedColor;
    private bool isSaved;
    private UI ui;
    // 用来记录鼠标是否悬停在该技能图标上
    private bool isHovered = false;

    private void OnValidate()
    {
        gameObject.name = $"Skill - {skillName}";
    }

    private void Awake()
    {
        // 仅在第一次加载时执行
        if (isRegistered) return;

        isRegistered = true;

         if (ui == null)
        {
            ui = UI.Instance;
            if (ui == null)
            {
                Debug.LogError("UI.Instance is not initialized.");
                return;
            }
        }

        Debug.Log(skillName + " awake 这个只应该发生一次");
        skillImage = GetComponent<Image>();
        if (skillImage == null)
        {
            Debug.LogError("skillImage is not assigned in SkillTreeSlot.");
        }


        ui = UI.Instance;

        var button = GetComponent<Button>();
        isSaved = false;

        if (button != null)
        {
            button.onClick.AddListener(UnlockSkill);
        }
        SaveManager.instance.RegisterSaveManager(this);
        Debug.Log(skillName + " SkillTreeSlot registered in SaveManager (Awake)"); 
        }

    private void Start()
    {
        skillImage.color = unlocked ? Color.white : lockedColor;
    }

    /// <summary>
    /// 左键点击时解锁技能
    /// </summary>
public void UnlockSkill()
{
    if (unlocked)
        return;

    // 检查先决条件：先决技能必须都解锁，依赖该技能的子技能不能解锁
    foreach (var item in shouldBeUnlocked)
    {
        if (!item.unlocked) return;
    }
    foreach (var item in shouldBeLocked)
    {
        if (item.unlocked) return;
    }

    if (!PlayerManager.Instance.HaveEnoughMoney(skillPrice))
        return;

    unlocked = true;
    skillImage.color = Color.white;
}
    /// <summary>
    /// 取消技能，要求先取消依赖该技能的子技能，然后返还花费
    /// </summary>
    public delegate void SkillStatusChanged();
    public event SkillStatusChanged OnSkillCancelled;


    private bool HasUnlockedDescendants()
    {
        foreach (SkillTreeSlot child in shouldBeLocked)
        {
            // 如果直接依赖的子技能已解锁，则返回 true
            if (child.unlocked)
            {
                Debug.Log($"技能 {child.gameObject.name} 仍处于解锁状态。");
                return true;
            }
            // 递归检测该子技能是否还有后续依赖处于解锁状态
            if (child.HasUnlockedDescendants())
                return true;
        }
        return false;
    }


    public void CancelSkill()
    {

        if (!unlocked)
            return;

        // 借助 SkillTreeManager 全局检索后续技能
        if (SkillTreeManager.Instance.HasUnlockedDescendants(this))
        {
            Debug.Log("请先取消所有依赖该技能的后续技能！");
            return;
        }

        unlocked = false;
        skillImage.color = lockedColor;
        PlayerManager.Instance.RefundMoney(skillPrice);
        // 通知其他系统技能已取消
        if (OnSkillCancelled != null)
            OnSkillCancelled.Invoke();
    }

public void OnPointerEnter(PointerEventData eventData)
{
    // 确保 ui.skillToolTip 不为 null
    if (ui.skillToolTip != null)
    {
        ui.skillToolTip.ShowToolTip(skillDescription, skillName);
        isHovered = true;
    }
    else
    {
        Debug.LogError("skillToolTip is not assigned in UI.");
    }
}

public void OnPointerExit(PointerEventData eventData)
{
    // 确保 ui.skillToolTip 不为 null
    if (ui.skillToolTip != null)
    {
        ui.skillToolTip.HideToolTip();
        isHovered = false;
    }
    else
    {
        Debug.LogError("skillToolTip is not assigned in UI.");
    }
}


    private void Update()
    {
        // 当鼠标悬停在该技能图标上且该技能已解锁时，按下鼠标右键取消技能
        if (isHovered && unlocked && Input.GetMouseButtonDown(1))
        {
            CancelSkill();
        }

        Debug.Log(skillName +" "+ unlocked);

      
    }

public void SaveData(ref GameData _data)
{
     if (isSaved)
    {
        Debug.Log($"{skillName} has already been saved.");
        return;
    }
    if (isSaved) return;  // 避免在保存过程中再次触发保存
    isSaved = true;
    Debug.Log($" 保存 Saving skill: {skillName} with state: {unlocked}");

    // 检查和打印保存前后的技能状态
    if (_data.skillTree.ContainsKey(skillName))
    {
        Debug.Log($"保存 Before Save: {_data.skillTree[skillName]}");
    }

    // 保存技能数据
    if (_data.skillTree.ContainsKey(skillName))
    {
        _data.skillTree[skillName] = unlocked;
    }
    else
    {
        _data.skillTree.Add(skillName, unlocked);
    }

    Debug.Log($"After Save: {_data.skillTree[skillName]}");
}

public void LoadData(GameData _data)
{
    //Debug.Log($"加载 Loading skill: {skillName}");

    if (_data == null || _data.skillTree == null)
    {
        Debug.LogError("GameData or skillTree is null. Cannot load data.");
        return;
    }

    if (_data.skillTree.ContainsKey(skillName))
    {
        unlocked = _data.skillTree[skillName];
//        Debug.Log($"加载 Loaded skill state: {unlocked}");
    }
    else
    {
        unlocked = false;
        Debug.Log("Skill not found in game data, setting to locked.");
    }

    if (skillImage != null)
    {
        skillImage.color = unlocked ? Color.white : lockedColor;
    }
}




}
