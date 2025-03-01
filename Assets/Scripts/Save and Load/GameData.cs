using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currency;
    public SerializableDictionary<string, int> inventory; // 确保是 public 或 [SerializeField]

    public GameData()
    {
        this.currency = 0;
        this.inventory = new SerializableDictionary<string, int>();
    }
}
