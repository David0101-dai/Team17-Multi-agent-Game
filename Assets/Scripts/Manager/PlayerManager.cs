using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, ISaveManager
{
    public static PlayerManager Instance { get; private set; }
    public GameObject player;
    public GameObject fx;
    public GameObject item;
    public  Scores scores;
    public int currency; 

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);
        else
            Instance = this;

        // 注册 SaveManager 以便进行数据保存和加载
        if (SaveManager.instance != null && SaveManager.instance.CurrentGameData() != null)
        {
            SaveManager.instance.RegisterSaveManager(this);
            Debug.Log("PlayerManager registered in SaveManager (Awake)");
        }
        else
        {
            StartCoroutine(RegisterWhenReady());
        }
    }

    private IEnumerator RegisterWhenReady()
    {   
        while (SaveManager.instance == null || SaveManager.instance.CurrentGameData() == null)
        {
            yield return null;  // 每帧检查，直到 SaveManager 和 gameData 完全加载
        }

        SaveManager.instance.RegisterSaveManager(this);
    }

    public bool HaveEnoughMoney(int _price)
    {
        if (_price > currency)
        {
            Debug.Log("Not Enough Money");
            return false;
        }

        currency -= _price;
        return true;
    }

    public void RefundMoney(int amount)
    {
        currency += amount;
        Debug.Log("Refunded " + amount + " currency. New total: " + currency);
    }

    public void LoadData(GameData _data)
    {
        this.currency = _data.currency;  // 加载游戏数据中的货币
    }

    public void SaveData(ref GameData _data)
    {
        _data.currency = this.currency;  // 保存当前货币数据
    }

    public void SaveFinaled()
    {
        scores.AddScore(currency);
    }

    public int currentCurrencyAmount() => currency;

        public void AddCurrency()
    {
        currency++;
        SaveManager.instance.SaveGame();  // 确保每次金币增加时都保存数据
    }

}

