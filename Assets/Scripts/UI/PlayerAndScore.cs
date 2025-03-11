using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerAndScore
{
    public string playerName;
    public int score;

    public PlayerAndScore(string playerName, int score)
    {
        this.playerName = playerName;
        this.score = score;
    }
}
