using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour , ISaveManager
{
    public static PlayerManager Instance { get; private set; }

    public GameObject player;
    public GameObject fx;
    public GameObject item;

    public int currency; 
    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);
        else
            Instance = this;
    }

    public bool HaveEnoughMoney(int _price)
    {
        if (_price > currency)
        {
            Debug.Log("Not Enough Money");
            return false;
        }

        currency = currency - _price;
        return true;
        
    }

    public void LoadData(GameData _data)
    {
        this.currency = _data.currency;
    }

    public void SaveData(ref GameData _data)
    {
        _data.currency = this.currency;
    }
}
