using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currency;
    public int finalScore;
    public string closetCheckPointId;
    // 用于存储字典的键和值
    public List<string> inventoryKeys = new List<string>();
    public List<int> inventoryValues = new List<int>();
    public List<string> checkPointKeys = new List<string>();

    // 存储技能树的键和值
    public List<string> skillTreeKeys = new List<string>();
    public List<bool> skillTreeValues = new List<bool>();
    public List<bool> checkPointValues = new List<bool>();

    // 存储当前穿戴的装备 ID
    public List<string> equipmentId = new List<string>();

    // 实际使用的字典（标记为不序列化，避免重复）
    [System.NonSerialized]
    public SerializableDictionary<string, int> inventory = new SerializableDictionary<string, int>();
    [System.NonSerialized]
    public SerializableDictionary<string, bool> skillTree = new SerializableDictionary<string, bool>();
    [System.NonSerialized]
    public SerializableDictionary<string, bool> checkpoint = new SerializableDictionary<string, bool>();

    public GameData()
    {
        this.currency = 0;
        this.finalScore = 0;
        this.inventory = new SerializableDictionary<string, int>();
        this.equipmentId = new List<string>();
        this.skillTree = new SerializableDictionary<string, bool>();
        this.checkpoint = new SerializableDictionary<string, bool>();
        this.closetCheckPointId = string.Empty;
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
        // 序列化 skillTree
        skillTreeKeys.Clear();
        skillTreeValues.Clear();
        foreach (var pair in skillTree)
        {
            skillTreeKeys.Add(pair.Key);
            skillTreeValues.Add(pair.Value);
        }
        checkPointKeys.Clear();
        checkPointValues.Clear();
        foreach (var pair in checkpoint)
        {
            checkPointKeys.Add(pair.Key);
            checkPointValues.Add(pair.Value);
        }
    }

    // 在反序列化后调用，将列表中的键和值填充到字典中
    public void OnAfterDeserialize()
    {
        inventory.Clear();
        skillTree.Clear();
        checkpoint.Clear();

        // 反序列化 inventory
        if (inventoryKeys.Count != inventoryValues.Count)
        {
            Debug.LogError("Inventory keys and values count mismatch after deserialization.");
        }
        else
        {
            for (int i = 0; i < inventoryKeys.Count; i++)
            {
                this.inventory.Add(inventoryKeys[i], inventoryValues[i]);
            }
        }

        // 反序列化 skillTree
        if (skillTreeKeys.Count != skillTreeValues.Count)
        {
            Debug.LogError("SkillTree keys and values count mismatch after deserialization.");
        }
        else
        {
            for (int i = 0; i < skillTreeKeys.Count; i++)
            {

                this.skillTree.Add(skillTreeKeys[i], skillTreeValues[i]);
            }
        }

        if (checkPointKeys.Count != checkPointValues.Count)
        {
            Debug.LogError("CheckPoint keys and values count mismatch after deserialization.");
        }
        else
        {
            for (int i = 0; i < checkPointKeys.Count; i++)
            {
                this.checkpoint.Add(checkPointKeys[i], checkPointValues[i]);
            }
        }
    }
}