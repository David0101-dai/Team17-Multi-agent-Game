using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData : MonoBehaviour
{
    public int currency;

    public GameData()
    {
        this.currency = 0;
    }
}
