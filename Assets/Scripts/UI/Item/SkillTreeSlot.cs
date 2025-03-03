using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISaveManager
{
    [SerializeField] public bool unlocked;
    [SerializeField] private SkillTreeSlot[] shouldBeUnlocked;
    [SerializeField] private SkillTreeSlot[] shouldBeLocked;
    private Image skillImage;
    [SerializeField] private int skillPrice;
    [SerializeField] private string skillName;
    [SerializeField] private string skillDescription;
    [SerializeField] private Color lockedColor;
    private UI ui;

    private void OnValidate()
    {
        gameObject.name = $"Skill - {skillName}";
    }

        private void Awake()
        {
            skillImage = GetComponent<Image>();
            ui = GetComponentInParent<UI>();

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
        // 再次注册，防止被禁用后再启用的情况
        if (SaveManager.instance != null)
        {
            SaveManager.instance.RegisterSaveManager(this);
        }
    }

    private void Start()
    {
        if (unlocked)
        {
            skillImage.color = Color.white;
        }
        else
        {
            skillImage.color = lockedColor;
        }
    }

    public void UnlockSkill()
    {
        if (unlocked)
            return;

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

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(skillDescription, skillName);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.HideToolTip();
    }

        public void LoadData(GameData _data)
    {
        Debug.Log("Loading skill tree data");

        if (_data.skillTree == null)
        {
            Debug.LogError("SkillTree dictionary is null.");
            return;
        }

        Debug.Log($"SkillTree keys: {string.Join(", ", _data.skillTreeKeys)}");
        Debug.Log($"Current skillName: {skillName}");

        if (_data.skillTree.TryGetValue(skillName, out bool value))
        {
            unlocked = value;
            Debug.Log($"Loaded skill: {skillName}, unlocked: {unlocked}");
        }
        else
        {
            Debug.LogWarning($"Skill {skillName} not found in skillTree.");
        }

        skillImage.color = unlocked ? Color.white : lockedColor;
    }
        
    public void SaveData(ref GameData _data)
    {
        Debug.Log($"Saving skill: {skillName}, unlocked: {unlocked}");
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
        Debug.Log("Inventory registered in SaveManager (Coroutine)");
    }
}
