using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class PlayerManager : MonoBehaviour, ISaveManager
{
    [SerializeField] private EnemyLevel level;
    public static PlayerManager Instance { get; private set; }
    public GameObject player;
    public  Scores scores;
    public int currency; 
    public bool isDead;
    public static int finalscore;
    private void Awake()
    {
        //playerName=PlayerPrefs.GetString()
            if (Instance != null)
        {
            Debug.LogWarning("PlayerManager instance already exists!");
            Destroy(Instance.gameObject);
        }
        else
        {
            Instance = this;
            Debug.Log("PlayerManager initialized!");
        }
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

        isDead = false;
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
      //  Debug.Log("Refunded " + amount + " currency. New total: " + currency);
    }

    public void LoadData(GameData _data)
    {
        if (_data != null)
        {
            this.currency = _data.currency; 
            finalscore = _data.finalScore;
        }
    }


        public void SaveData(ref GameData _data)
    {
        _data.currency = this.currency;
        _data.finalScore = finalscore;
    }

    public void SaveFinaled()
    {
        scores.AddScore(calculateScore());
    }
    public float calculateScore()
    {
        return finalscore + currency * 0.5f - TimeManager.Instance.getTime() * 0.03f;
    }

    public void AddCurrency()
    {
        currency++;
        SaveManager.instance.SaveGame();  // 确保数据被保存
    }
    public void AddScore(int add)
    {
        finalscore = finalscore + level.GetLevel() * add;
    }
    //public void initialScore()
    //{
    //    currency = 0;
    //    finalscore = 0;
    //}
}

