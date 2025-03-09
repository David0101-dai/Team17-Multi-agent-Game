
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewScoreData", menuName = "Data/ScoreData")]
public class Scores : ScriptableObject
{
    //private Dictionary<string, int> playerScore = new Dictionary<string, int>();
    public List<int> scores = new List<int>();
    public void AddScore(int score)
    {
        scores.Add(score);
    }
    public void printfScores()
    {
        if (isEmpty())
        {
            Debug.Log("No scores");
        }

        foreach (var score in scores)
        {
            Debug.Log("Score:" + score);
        }
    }

    private bool isEmpty()
    {
        return scores.Count == 0;
    }

    public void initialScores()
    {
        scores.Clear();
    }
}
