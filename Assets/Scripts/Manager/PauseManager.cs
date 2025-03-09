using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;  // 标记游戏是否已暂停

    void Update()
    {
        // 按下 "Escape" 切换暂停状态
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        // 切换时间缩放
        Time.timeScale = isPaused ? 0 : 1;

        // 显示/隐藏暂停菜单
        if (isPaused)
        {
            Debug.Log("Game Paused");
        }
        else
        {
            Debug.Log("Game Resumed");
        }
    }
}
