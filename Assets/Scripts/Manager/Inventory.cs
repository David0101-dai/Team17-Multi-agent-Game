using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour, ISaveManager
{
    public static Inventory Instance { get; private set; }

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

    [Header("Data Base")]
    public List<InventoryItem> loadedItems;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        equipmentItems = new List<InventoryItem>();
        equipmentDic = new Dictionary<ItemDataEquipment, InventoryItem>();

        inventoryItems = new List<InventoryItem>();
        inventoryDic = new Dictionary<ItemData, InventoryItem>();

        stashItems = new List<InventoryItem>();
        stashDic = new Dictionary<ItemData, InventoryItem>();

        equipmentSlots = equipmentSlotParent.GetComponentsInChildren<EquipmentSlot>();
        inventoryItemSlots = inventorySlotParent.GetComponentsInChildren<ItemSlot>();
        stashItemSlots = stashSlotParent.GetComponentsInChildren<ItemSlot>();
        statSlots = statSlotParent.GetComponentsInChildren<StatsSlot>();

        AddStartingItems();
    }

    private void AddStartingItems()
    {
        if (loadedItems.Count > 0)
        {
            foreach (var item in loadedItems)
            {
                for (int i = 0; i < item.stackSize; i++)
                {
                    AddItem(item.data);
                }
            }
            return;
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
    }

    public void UnEquipItem(ItemData item)
    {
        var unequipData = item as ItemDataEquipment;
        var unequipItem = new InventoryItem(unequipData);

        UnEquipMethod(unequipData);
        AddItemMethod(inventoryItems, inventoryDic, unequipData);

        UpdateSlotUI(inventoryItemSlots, inventoryItems);
        UpdateSlotUI(equipmentSlots, equipmentItems);
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
        if (!CanAddItem(item)) return;
        switch (item.itemType)
        {
            case ItemType.Material:
                Debug.Log($"Adding material item: {item.itemId}");
                AddItemMethod(stashItems, stashDic, item);
                UpdateSlotUI(stashItemSlots, stashItems);
                break;
            case ItemType.Equipment:
                Debug.Log($"Adding equipment item: {item.itemId}");
                AddItemMethod(inventoryItems, inventoryDic, item);
                UpdateSlotUI(inventoryItemSlots, inventoryItems);
                break;
            default:
                break;
        }
    }

    public void RemoveItem(ItemData item)
    {
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
            value.AddStack();
        }
        else
        {
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
        if (!currentFlask) return;
        var canUseFlask = Time.time > lastTimeUsedFlask + currentFlask.ItemCooldown;
        if (canUseFlask)
        {
            currentFlask.ExecuteItemEffect(null, null);
            lastTimeUsedFlask = Time.time;
        }
    }

    public bool CanAddItem(ItemData item)
    {
        //switch (item.itemType)
        //{
        //    case ItemType.Material:
        //        return stashItems.Count < stashItemSlots.Length;
        //    case ItemType.Equipment:
        //        return inventoryItems.Count < inventoryItemSlots.Length;
        //}
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
            return inventoryItems.Count < inventoryItemSlots.Length;
        }
    }

    public void CanCraft(ItemDataEquipment craftData, object craftingMaterials)
    {
        throw new NotImplementedException();
    }

    public void LoadData(GameData _data)
    {
        foreach (KeyValuePair<string, int> pair in _data.inventory)
        {
            foreach (var item in GetItemDataBase())
            {
                if (item.itemId != pair.Key) continue;
                var newItem = new InventoryItem(item);
                newItem.stackSize = pair.Value;
                loadedItems.Add(newItem);

                // 将加载的物品添加到 inventoryItems 或 stashItems 中
                if (item.itemType == ItemType.Equipment)
                {
                    inventoryItems.Add(newItem);
                    inventoryDic.Add(item, newItem);
                }
                else if (item.itemType == ItemType.Material)
                {
                    stashItems.Add(newItem);
                    stashDic.Add(item, newItem);
                }
            }
        }

        // 更新 UI
        UpdateSlotUI(inventoryItemSlots, inventoryItems);
        UpdateSlotUI(stashItemSlots, stashItems);
    }

    public void SaveData(ref GameData _data)
    {
        Debug.Log("Saving Inventory Data");
        _data.inventory.Clear();
        
        foreach (KeyValuePair<ItemData, InventoryItem> pair in inventoryDic)
        {
            Debug.Log($"Saving item: {pair.Key.itemId}, stack size: {pair.Value.stackSize}");
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }

        foreach (KeyValuePair<ItemData, InventoryItem> pair in stashDic)
        {
            Debug.Log($"Saving item: {pair.Key.itemId}, stack size: {pair.Value.stackSize}");
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }
    }

private List<ItemData> GetItemDataBase()
    {
        List<ItemData> itemDataBase = new List<ItemData>();

#if UNITY_EDITOR
        string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/Scripts/Item/ScriptableObject" });
        foreach (var SOName in assetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName); // 修正：将 GUID 转换为路径
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath); // 确保 SOpath 是 string 类型
            if (itemData != null)
            {
                itemDataBase.Add(itemData); // 添加 ItemData 对象
            }
        }
#endif

        return itemDataBase;
    }
}