using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISaveManager
{
    [SerializeField] public bool unlocked;
    [SerializeField] public SkillTreeSlot[] shouldBeUnlocked;
    [SerializeField] public SkillTreeSlot[] shouldBeLocked;
    private Image skillImage;
    [SerializeField] private int skillPrice;
    [SerializeField] private string skillName;
    [SerializeField] private string skillDescription;
    [SerializeField] private Color lockedColor;
    private UI ui;

    // 用来记录鼠标是否悬停在该技能图标上
    private bool isHovered = false;

    private void OnValidate()
    {
        gameObject.name = $"Skill - {skillName}";
    }

    private void Awake()
    {
        skillImage = GetComponent<Image>();
        ui = GetComponentInParent<UI>();

        // 如果存在 Button 组件，则绑定左键解锁逻辑
        var button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(UnlockSkill);
        }

        if (SaveManager.instance != null)
        {
            SaveManager.instance.RegisterSaveManager(this);
        }
        else
        {
            StartCoroutine(RegisterWhenReady());
        }
    }

    private void OnEnable()
    {
        if (SaveManager.instance != null)
        {
            SaveManager.instance.RegisterSaveManager(this);
        }
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
        ui.skillToolTip.ShowToolTip(skillDescription, skillName);
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.HideToolTip();
        isHovered = false;
    }

    private void Update()
    {
        // 当鼠标悬停在该技能图标上且该技能已解锁时，按下鼠标右键取消技能
        if (isHovered && unlocked && Input.GetMouseButtonDown(1))
        {
            CancelSkill();
        }

      
    }

    public void LoadData(GameData _data)
    {
        if (_data.skillTree == null)
        {
            Debug.LogError("SkillTree dictionary is null.");
            return;
        }
        if (_data.skillTree.TryGetValue(skillName, out bool value))
        {
            unlocked = value;
        }
        skillImage.color = unlocked ? Color.white : lockedColor;
    }

    public void SaveData(ref GameData _data)
    {
        if (_data.skillTree.ContainsKey(skillName))
        {
            _data.skillTree[skillName] = unlocked;
        }
        else
        {
            _data.skillTree.Add(skillName, unlocked);
        }
    }

    private IEnumerator RegisterWhenReady()
    {
        while (SaveManager.instance == null)
        {
            yield return null;
        }
        SaveManager.instance.RegisterSaveManager(this);
        Debug.Log("SkillTreeSlot registered in SaveManager (Coroutine)");
    }
}
