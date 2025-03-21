using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Security.Cryptography;


public class Inventory : MonoBehaviour, ISaveManager
{
    public static Inventory Instance { get; private set; }

    public List<ItemData> startingEquipment;

    public List<InventoryItem> equipmentItems;
    public Dictionary<ItemDataEquipment, InventoryItem> equipmentDic;

    public List<InventoryItem> inventoryItems;
    public Dictionary<ItemData, InventoryItem> inventoryDic;

    public List<InventoryItem> stashItems;
    public Dictionary<ItemData, InventoryItem> stashDic;

    [Header("UI")]
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform statSlotParent;

    private EquipmentSlot[] equipmentSlots;
    private ItemSlot[] inventoryItemSlots;
    private ItemSlot[] stashItemSlots;
    private StatsSlot[] statSlots;

    [Header("ItemsCooldown")]
    private float lastTimeUsedFlask;
    private float lastTimeExcute;

    [Header("EquipmentCooldown")]
    [SerializeField] private float equipmentCooldown = 5f;

    [Header("Flask Settings")]
    [SerializeField] public float flaskCooldown = 10f;


    [Header("Data Base")]
    public List<ItemData> itemDataBase;
    public List<InventoryItem> loadedItems;
    public List<ItemDataEquipment> loadedEquipment;

    private void Awake()
    {
        if (UI.Instance == null)
    {
        Debug.LogError("UI.Instance is not initialized.");
        return;
    }

        Debug.Log("Inventory Awake");

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // 如果 SaveManager 尚未创建，则等待一帧后再注册
        if (SaveManager.instance != null && SaveManager.instance.CurrentGameData() != null)
        {
            SaveManager.instance.RegisterSaveManager(this);
            Debug.Log("Inventory registered in SaveManager (Awake)");
        }
        else
        {
            // 如果 SaveManager 在此时还没初始化，使用协程等待
            StartCoroutine(RegisterWhenReady());
        }

        equipmentSlots = UI.Instance.Equipment.GetComponentsInChildren<EquipmentSlot>();
        stashItemSlots = UI.Instance.Stash.GetComponentsInChildren<ItemSlot>();
        statSlots = UI.Instance.Stat.GetComponentsInChildren<StatsSlot>();
        
    }
    private IEnumerator RegisterWhenReady()
    {   // 等待 SaveManager 初始化完成
        while (SaveManager.instance == null || SaveManager.instance.CurrentGameData() == null)
        {
            yield return null;  // 每帧检查，直到 SaveManager 和 gameData 完全加载
        }// 当 SaveManager 准备好时，注册
        SaveManager.instance.RegisterSaveManager(this);
        //Debug.Log("Inventory registered in SaveManager (Coroutine)");
    }

    private void Start()
    {
        equipmentItems = new List<InventoryItem>();
        equipmentDic = new Dictionary<ItemDataEquipment, InventoryItem>();

        inventoryItems = new List<InventoryItem>();
        inventoryDic = new Dictionary<ItemData, InventoryItem>();

        stashItems = new List<InventoryItem>();
        stashDic = new Dictionary<ItemData, InventoryItem>();

        // 确保 inventoryItemSlots 正确初始化
        inventoryItemSlots = UI.Instance.Inventory.GetComponentsInChildren<ItemSlot>();
        //Debug.Log($"inventoryItemSlots count: {inventoryItemSlots.Length}");
        if (inventoryItemSlots == null || inventoryItemSlots.Length == 0)
        {
            Debug.LogError("inventoryItemSlots is null or empty. Please check your UI setup.");
        }
        // equipmentSlots = equipmentSlotParent.GetComponentsInChildren<EquipmentSlot>();
        // stashItemSlots = stashSlotParent.GetComponentsInChildren<ItemSlot>();
        // statSlots = statSlotParent.GetComponentsInChildren<StatsSlot>();
        AddStartingItems();
    }
    private void Update()
    {
        ExcuteEffect();
    }
    public bool HasEquippedItem(EquipmentType equipmentType)
    {
        return equipmentDic.Any(x => x.Key.equipmentType == equipmentType);
    }

    private void AddInitialItem()
    {
       Debug.Log($"Starting Equipment Count: {startingEquipment.Count}");
       for (int i = 0; i < startingEquipment.Count; i++)
       {
           Debug.Log($"Adding item: {startingEquipment[i].itemName}");
           AddItem(startingEquipment[i]);
       }
    }
    private void AddStartingItems()
    {
        // 加载装备
        foreach (ItemDataEquipment item in loadedEquipment)
        {
            if (item == null)
            {
                Debug.LogWarning("Loaded equipment item is null.");
                continue;
            }
            EquipItem(item);
        }

        // 加载其他物品
        if (loadedItems.Count > 0)
        {
            foreach (var item in loadedItems)
            {
                if (item.data == null)
                {
                    Debug.LogWarning("Loaded item data is null.");
                    continue;
                }
                for (int i = 0; i < item.stackSize; i++)
                {
                    AddItem(item.data);
                }
            }
        }
    }
        public void EquipItem(ItemData item)
    {
        var newEquipment = item as ItemDataEquipment;
        var newItem = new InventoryItem(newEquipment);
        var old = equipmentDic.FirstOrDefault(x => x.Key.equipmentType == newEquipment.equipmentType).Key;

        if (old)
        {
            UnEquipMethod(old);
            AddItemMethod(inventoryItems, inventoryDic, old);
        }

        RemoveItemMethod(inventoryItems, inventoryDic, item);
        EquipMethod(newEquipment, newItem);

        UpdateSlotUI(inventoryItemSlots, inventoryItems);
        UpdateSlotUI(equipmentSlots, equipmentItems);
        //Effect(item);
    }

    private void Effect(ItemData item)
    {
        if (item.name == "IceNecklace")
        {
            ExcuteSpecialEffect("IceNecklace");
        }
    }

    public void ExcuteSpecialEffect(String tagName)
    {
        var amulet = GetEquipmentByType(EquipmentType.Amulet);
        if (amulet != null && amulet.name == tagName)
        {
            Debug.Log("CreatFX!!!");
            amulet.ExecuteItemEffect(PlayerManager.Instance.player, PlayerManager.Instance.player);
        }

    }

    public void setEuipmentTimer()
    {
        lastTimeExcute = Time.time;
    }

    public bool canExcuteEquipment()
    {
        return Time.time > lastTimeExcute + equipmentCooldown;
    }

    public void UnEquipItem(ItemData item)
    {
        var unequipData = item as ItemDataEquipment;
        var unequipItem = new InventoryItem(unequipData);

        UnEquipMethod(unequipData);
        AddItemMethod(inventoryItems, inventoryDic, unequipData);

        UpdateSlotUI(inventoryItemSlots, inventoryItems);
        UpdateSlotUI(equipmentSlots, equipmentItems);
        //DestroyEffect(item);
    }

    //private void DestroyEffect(ItemData item)
    //{
    //    if (item.name == "IceNecklace")
    //    {
    //        Debug.Log("DeatroyFX!!!!");
    //        DestroyEffectWithTag("IceNecklace");
    //    }
    //}

    public bool ExistEffect(string tagName)
    {
        GameObject effect = GameObject.FindWithTag(tagName);
        if (effect != null)
        {
            return true;
            //Debug.Log($"Tag 为 {tagName} 的特效已成功销毁。");
        }
        else
        {
            Debug.Log($"未找到 Tag 为 {tagName} 的特效。");
            return false;
        }
    }

    private void EquipMethod(ItemDataEquipment newEquipment, InventoryItem newItem)
    {
        equipmentItems.Add(newItem);
        equipmentDic.Add(newEquipment, newItem);
        newEquipment.AddModifiers();
    }

    private void UnEquipMethod(ItemDataEquipment old)
    {
        if (equipmentDic.TryGetValue(old, out var oldValue))
        {
            equipmentItems.Remove(oldValue);
            equipmentDic.Remove(old);
            old.RemoveModifiers();
        }
    }

        public void AddItem(ItemData item)
    {
        if (item == null)
        {
            Debug.LogError("Item is null. Cannot add item.");
            return;
        }

        if (!CanAddItem(item)) return;

        switch (item.itemType)
        {
            case ItemType.Material:
                AddItemMethod(stashItems, stashDic, item);
                UpdateSlotUI(stashItemSlots, stashItems);
                break;
            case ItemType.Equipment:
                AddItemMethod(inventoryItems, inventoryDic, item);
                UpdateSlotUI(inventoryItemSlots, inventoryItems);
                break;
            case ItemType.Coin:
            PlayerManager.Instance.AddCurrency();
//            Debug.Log("add a currency now the currency: " + PlayerManager.Instance.currency);
            break;
            default:
                break;
        }
    }

    public void RemoveItem(ItemData item)
    {
        if (item == null)
        {
            Debug.LogError("Item is null. Cannot remove item.");
            return;
        }

        switch (item.itemType)
        {
            case ItemType.Material:
                RemoveItemMethod(stashItems, stashDic, item);
                UpdateSlotUI(stashItemSlots, stashItems);
                break;
            case ItemType.Equipment:
                RemoveItemMethod(inventoryItems, inventoryDic, item);
                UpdateSlotUI(inventoryItemSlots, inventoryItems);
                break;
            default:
                break;
        }
    }


    private void AddItemMethod(List<InventoryItem> items, Dictionary<ItemData, InventoryItem> itemDic, ItemData item)
    {
        if (itemDic.TryGetValue(item, out InventoryItem value))
        {
           // Debug.Log("addstack:" + item.itemName);
            value.AddStack();
        }
        else
        {
          //  Debug.Log("addnew:" + item.itemName);
            var newItem = new InventoryItem(item);
            items.Add(newItem);
            itemDic.Add(item, newItem);
        }
    }

    private void RemoveItemMethod(List<InventoryItem> items, Dictionary<ItemData, InventoryItem> itemDic, ItemData item)
    {
        if (itemDic.TryGetValue(item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                items.Remove(value);
                itemDic.Remove(item);
            }
            else
            {
                value.RemoveStack();
            }
        }
    }

public void UpdateSlotUI(ItemSlot[] slots, List<InventoryItem> items)
{
    if (slots == null)
    {// Debug.Log("Slots array is null in UpdateSlotUI.");
        return;
    }

    if (items == null)
    {//Debug.Log("Items list is null in UpdateSlotUI.");
        return;
    }

    for (int i = 0; i < slots.Length; i++)
    {
        var slot = slots[i] as EquipmentSlot;

        if (!slot)
        {
            slots[i].UpdateSlot(i < items.Count ? items[i] : null);
        }
        else
        {
            slots[i].UpdateSlot(null);
            for (int j = 0; j < items.Count; j++)
            {
                var data = items[j].data as ItemDataEquipment;
                if (!data || slot.slotType != data.equipmentType) continue;
                slots[i].UpdateSlot(items[j]);
            }
        }
    }

    for (int i = 0; i < statSlots.Length; i++)
    {
        statSlots[i].UpdateStatValue();
    }
}


    public ItemDataEquipment GetEquipmentByType(EquipmentType equipmentType)
    {
        ItemDataEquipment equipment = null;
        foreach (var item in equipmentItems)
        {
            var equipmentData = item.data as ItemDataEquipment;
            if (!equipmentData || equipmentData.equipmentType != equipmentType) continue;
            equipment = equipmentData;
        }
        return equipment;
    }

    public void UsedFlask()
    {
        var currentFlask = GetEquipmentByType(EquipmentType.Flask);
        lastTimeUsedFlask = -10f;
        if (!currentFlask) return;

        // 使用 Inventory 中独立的 flaskCooldown
        var canUseFlask = Time.time > lastTimeUsedFlask + flaskCooldown;
        if (canUseFlask)
        {
            if (currentFlask.name == "Shield")
            {
                currentFlask.ExecuteItemEffect(PlayerManager.Instance.player, PlayerManager.Instance.player);
            }
            else
            {
                currentFlask.ExecuteItemEffect(null, null);
                lastTimeUsedFlask = Time.time;
            }
            UnEquipItem(currentFlask);
            RemoveItem(currentFlask);
        }
    }
    public bool CanAddItem(ItemData item)
    {
        if (item == null)
        {
            Debug.LogError("Cannot add item: item is null.");
            return false;
        }
        if(item.itemType== ItemType.Coin)
        {
                return true;
        }
        if (inventoryItemSlots == null || inventoryItemSlots.Length == 0)
        {
            Debug.LogError("inventoryItemSlots is null or empty. Cannot add item.");
            return false;
        }

        if (item.itemType == ItemType.Material)
        {
            var old = stashDic.ContainsKey(item);
            if (old) return true;
            return stashItems.Count < stashItemSlots.Length;
        }
        else
        {
            var old = inventoryDic.ContainsKey(item);
            if (old) return true;

            // 检查物品数量是否超过插槽数量
            if (inventoryItems.Count >= inventoryItemSlots.Length)
            {
                Debug.Log("Cannot add item: inventory is full.");
                return false;
            }
            //Debug.Log("Can Add:" + inventoryItemSlots.Length);
            //Debug.Log("Now we have:" + inventoryItems.Count);

            return inventoryItems.Count < inventoryItemSlots.Length;
        }
    }
    public void LoadData(GameData _data)
    {
        Debug.Log("Loading Inventory Data");

        // 确保 inventoryDic 已经初始化
        if (inventoryDic == null) inventoryDic = new Dictionary<ItemData, InventoryItem>();
        if (stashDic == null) stashDic = new Dictionary<ItemData, InventoryItem>();
        if (equipmentDic == null) equipmentDic = new Dictionary<ItemDataEquipment, InventoryItem>();
        // 清空当前数据
        inventoryItems.Clear();
        stashItems.Clear();
        equipmentItems.Clear();

        // 清空字典
        inventoryDic.Clear();
        stashDic.Clear();
        equipmentDic.Clear();

        // 加载 inventory 和 stash
        foreach (KeyValuePair<string, int> pair in _data.inventory)
        {
            foreach (var item in itemDataBase)
            {
                if (item.itemId != pair.Key) continue;
                var newItem = new InventoryItem(item);
                newItem.stackSize = pair.Value;
                loadedItems.Add(newItem);

                // 将加载的物品添加到 inventoryItems 或 stashItems 中
                if (item.itemType == ItemType.Equipment)
                {
                    inventoryItems.Add(newItem);
                    inventoryDic.Add(item, newItem);  // 添加前要确保 dictionary 已初始化
                }
                else if (item.itemType == ItemType.Material)
                {
                    stashItems.Add(newItem);
                    stashDic.Add(item, newItem);
                }
            }
        }

        // 加载装备
        foreach (string loadedItemId in _data.equipmentId)
        {
            foreach (var item in itemDataBase)
            {
                if (item.itemId != loadedItemId) continue;
                var newItem = new InventoryItem(item);
                newItem.stackSize = 1;
                loadedEquipment.Add(item as ItemDataEquipment);

                // 将加载的装备添加到 equipmentItems 和 equipmentDic 中
                equipmentItems.Add(newItem);
                equipmentDic.Add(item as ItemDataEquipment, newItem);
            }
        }

        // 更新 UI
        UpdateSlotUI(inventoryItemSlots, inventoryItems);
        UpdateSlotUI(stashItemSlots, stashItems);
        UpdateSlotUI(equipmentSlots, equipmentItems);
    }

    private void ExcuteEffect()
    {
        if (HasEquippedItem(EquipmentType.Amulet)&&!ExistEffect("IceNecklace"))
        {
            if (canExcuteEquipment())
            {
                ExcuteSpecialEffect("IceNecklace");
                setEuipmentTimer();
                DestroyByTime("IceNecklace");
            }
        }
    }

    private static void DestroyByTime(String tagName)
    {
        GameObject effect = GameObject.FindWithTag(tagName);
        Destroy(effect, 1f);
    }

    public void SaveData(ref GameData _data)
    {
        //Debug.Log("Saving Inventory Data");
        _data.inventory.Clear();
        _data.equipmentId.Clear(); // 清空装备 ID 列表

        // 保存 inventoryDic
        foreach (KeyValuePair<ItemData, InventoryItem> pair in inventoryDic)
        {
          //  Debug.Log($"Saving item: {pair.Key.itemId}, stack size: {pair.Value.stackSize}");
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }

        // 保存 stashDic
        foreach (KeyValuePair<ItemData, InventoryItem> pair in stashDic)
        {
           // Debug.Log($"Saving item: {pair.Key.itemId}, stack size: {pair.Value.stackSize}");
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }

        // 保存 equipmentDic
        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> pair in equipmentDic)
        {
           // Debug.Log($"Saving equipment: {pair.Key.itemId}");
            _data.equipmentId.Add(pair.Key.itemId); // 保存装备 ID
        }
    }

    
// #if UNITY_EDITOR
//     [ContextMenu("Get ItemDataBase")]
//     private void GetItemDataBaseContextMenu()
//     {
//         itemDataBase = new List<ItemData>(GetItemDataBase()) ;
//     }
//     private List<ItemData> GetItemDataBase()
//     {
//         List<ItemData> itemDataBase = new List<ItemData>();


//         string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/Scripts/Item/ScriptableObject" });
//         foreach (var SOName in assetNames)
//         {
//             var SOpath = AssetDatabase.GUIDToAssetPath(SOName); // 修正：将 GUID 转换为路径
//             var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath); // 确保 SOpath 是 string 类型
//             if (itemData != null)
//             {
//                 itemDataBase.Add(itemData); // 添加 ItemData 对象
//             }
//         }


//         return itemDataBase;
//     }
// #endif

    public bool CanCraft(ItemDataEquipment itemToCraft, List<InventoryItem> requiredMaterials)
    {
        if (!CanAddItem(itemToCraft))
        {
            Debug.Log("Please empty your inventory");
            return false;
        }
        List<InventoryItem> materialsToRemove = new List<InventoryItem>();

        for (int i = 0; i < requiredMaterials.Count; i++)
        {
            if (stashDic.TryGetValue(requiredMaterials[i].data, out InventoryItem stashValue))
            {
                if (stashValue.stackSize < requiredMaterials[i].stackSize)
                {
                    Debug.Log("not enough materials");
                    return false;
                }
                else
                {
                    materialsToRemove.Add(stashValue);
                }
            }
            else
            {
                Debug.Log("not enough materials");
                return false;
            }
        }
        for (int i = 0; i < materialsToRemove.Count; i++)
        {
            RemoveItem(materialsToRemove[i].data);
        }
        AddItem(itemToCraft);
        Debug.Log("Here is your item " + itemToCraft.name);
        return true;
    }
}