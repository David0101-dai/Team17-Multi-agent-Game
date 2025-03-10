using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    // 通过按钮点击触发的播放音效的方法
    public void PlaySound()
    {
        if (GameAudioManager.instance != null)
        {
            int soundIndex = 0; // 设置你想播放的音效索引
            GameAudioManager.instance.PlaySFX(soundIndex); // 调用AudioManager播放音效
            Debug.Log("播放音效 " + soundIndex);
        }
        else
        {
            Debug.LogError("AudioManager 实例未找到！");
        }
    }
}
