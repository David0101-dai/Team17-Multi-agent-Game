using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private Transform check;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Vector2 boxSize;
     [SerializeField] private List<GameObject> enemyPrefabs;  // 变为一个列表，存储多个敌人预制体
    public GameObject boss;
    
    private List<GameObject> availableEnemies;  // 用于追踪还未被召唤的怪物

    public void SetUp(GameObject boss){
        this.boss = boss;
        availableEnemies = new List<GameObject>(enemyPrefabs);  // 初始化可用怪物列表
    }


    private void AnimationExplodeEvent(){
        Collider2D[] colliders = Physics2D.OverlapBoxAll(check.position, boxSize,playerLayer);
        foreach (var hit in colliders)
        {
            if(hit.GetComponent<Player>() != null){
                Damageable damageable = hit.GetComponent<Damageable>();
                damageable.TakeDamage(boss, true, false, false, false, false, false);
                 // 每次造成伤害时，按20%的概率召唤怪物
                TrySummonEnemy();
            }
        }
    }

     private void TrySummonEnemy()
    {
        // 检查是否还有可召唤的怪物
        if (availableEnemies.Count == 0)
        {
            return;  // 如果没有可召唤的怪物，直接返回
        }

        if(boss.GetComponent<Boss>().BossStage == BossStage.stage4){
            // 20% 的概率
        if (Random.Range(0f, 1f) <= 0.4f)
        {
            // 随机选择一个怪物预制体
            int index = Random.Range(0, availableEnemies.Count);
            GameObject enemyToSummon = availableEnemies[index];

            // 召唤怪物并从可用列表中移除
            Instantiate(enemyToSummon, transform.position, Quaternion.identity);
            availableEnemies.RemoveAt(index);  // 确保每个怪物只能召唤一次
        }
        }

        // 20% 的概率
        if (Random.Range(0f, 1f) <= 0.2f)
        {
            // 随机选择一个怪物预制体
            int index = Random.Range(0, availableEnemies.Count);
            GameObject enemyToSummon = availableEnemies[index];

            // 召唤怪物并从可用列表中移除
            Instantiate(enemyToSummon, transform.position, Quaternion.identity);
            availableEnemies.RemoveAt(index);  // 确保每个怪物只能召唤一次
        }
    }

    private void SelfDestroy(){
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(check.position, boxSize);
    }

}
