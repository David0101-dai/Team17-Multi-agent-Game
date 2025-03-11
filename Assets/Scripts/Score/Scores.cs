
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
[CreateAssetMenu(fileName = "NewScoreData", menuName = "Data/ScoreData")]
public class Scores : ScriptableObject
{
    public List<PlayerAndScore> scores = new List<PlayerAndScore>();
    public void AddScore(int score)
    {
        PlayerAndScore playerAndScore = new PlayerAndScore(PlayerPrefs.GetString("PlayerName","no name"),score);
        var existingPlayer = scores.Find(p => p.playerName == playerAndScore.playerName);
        if (existingPlayer==null)
        {
            scores.Add(playerAndScore);
        }
        else
        {
            if (existingPlayer.score < score)
            {
                existingPlayer.score = score;
            }
        }
    }
    public void printfScores()
    {
        Debug.Log("List:"+scores.Count);
    }
}
