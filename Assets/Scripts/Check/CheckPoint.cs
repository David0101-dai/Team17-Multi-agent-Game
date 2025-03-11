using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public Animator anim;
    public string checkPointId;
    public bool activated;
     private void Start()
    {
        anim = GetComponent<Animator>();
        if(checkPointId == null){
            Debug.Log("没有命名的存档点,赶紧命名");
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>()!= null)
        {
            ActivateCheckPointWhenCollision();
        }
    }
    void LateUpdate()
{
    if (anim == null)
    {
        anim = GetComponent<Animator>();  // 尝试再次获取 Animator
        if (anim != null)
        {
            Debug.Log($"Animator initialized for CheckPoint {checkPointId}");
        }
    }
}


public void ActivateCheckPoint()
{
    // 检查 Animator 是否初始化
    if (anim == null)
    {
        Debug.LogWarning($"Animator not found on CheckPoint {checkPointId}. Activation skipped.");
        return;  // 如果找不到 Animator，就跳过激活
    }
    anim.SetBool("Active", true);  // 激活动画
    activated = true;

    // 确保立即更新存档数据
    GameData gameData = SaveManager.instance.CurrentGameData();
    if (gameData != null)
    {
        if (!gameData.checkpoint.ContainsKey(checkPointId))
        {
            gameData.checkpoint.Add(checkPointId, activated);
        }
        else
        {
            gameData.checkpoint[checkPointId] = activated;
        }
        SaveManager.instance.SaveGame();
    }
}

public void ActivateCheckPointWhenCollision()
{
    // 检查 Animator 是否初始化
    if (anim == null)
    {
        Debug.LogWarning($"Animator not found on CheckPoint {checkPointId}. Activation skipped.");
        return;  // 如果找不到 Animator，就跳过激活
    }

    Debug.Log("激活存档点" + checkPointId);

    anim.SetBool("Active", true);  // 激活动画
    activated = true;

    // 确保立即更新存档数据
    GameData gameData = SaveManager.instance.CurrentGameData();
    if (gameData != null)
    {
        if (!gameData.checkpoint.ContainsKey(checkPointId))
        {
            gameData.checkpoint.Add(checkPointId, activated);
        }
        else
        {
            gameData.checkpoint[checkPointId] = activated;
        }
        gameData.closetCheckPointId =  checkPointId;
        SaveManager.instance.SaveGame();
    }

}

}
