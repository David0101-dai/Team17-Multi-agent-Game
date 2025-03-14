using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerAndScore
{
    public string playerName;
    public float score;

    public PlayerAndScore(string playerName, float score)
    {
        this.playerName = playerName;
        this.score = score;
    }
}
