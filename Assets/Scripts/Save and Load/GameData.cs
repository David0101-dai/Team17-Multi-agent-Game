using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currency;

    // 用于存储字典的键和值
    public List<string> inventoryKeys = new List<string>();
    public List<int> inventoryValues = new List<int>();

    // 存储当前穿戴的装备 ID
    public List<string> equipmentId = new List<string>();

    // 实际使用的字典（标记为不序列化，避免重复）
    [System.NonSerialized]
    public SerializableDictionary<string, int> inventory = new SerializableDictionary<string, int>();

    public GameData()
    {
        this.currency = 0;
        this.inventory = new SerializableDictionary<string, int>();
        this.equipmentId = new List<string>();
    }

    // 在序列化前调用，将字典的键和值填充到列表中
    public void OnBeforeSerialize()
    {
        inventoryKeys.Clear();
        inventoryValues.Clear();

        foreach (var pair in inventory)
        {
            inventoryKeys.Add(pair.Key);
            inventoryValues.Add(pair.Value);
        }
    }

    // 在反序列化后调用，将列表中的键和值填充到字典中
    public void OnAfterDeserialize()
    {
        inventory.Clear();

        if (inventoryKeys.Count != inventoryValues.Count)
            throw new System.Exception("Keys and values count mismatch after deserialization.");

        for (int i = 0; i < inventoryKeys.Count; i++)
        {
            inventory.Add(inventoryKeys[i], inventoryValues[i]);
        }
    }
}