using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyLevel", menuName = "GameSettings/EnemyLevel")]
public class EnemyLevel : ScriptableObject
{
    [SerializeField] private int enemyLevel = 1;
    public int GetLevel()
    {
        return enemyLevel;
    }

    internal void SetLevel(int level)
    {
        enemyLevel=level;
    }
}
