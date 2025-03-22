using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBGM : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 检查碰到的是玩家
        if (other.CompareTag("Player"))
        {
            // 播放BGM，假设4是要播放的背景音乐的ID
            AudioManager.instance.PlayBGM(4);

            // 销毁这个物体
            Destroy(gameObject);
        }
    }
}
