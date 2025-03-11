using UnityEngine;
using TMPro;

public class DropdownSound : MonoBehaviour
{
    private TMP_Dropdown dropdown;

    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        if (dropdown != null)
        {
            dropdown.onValueChanged.AddListener(PlayDropdownSound);
        }
        else
        {
            Debug.LogError("DropdownSound: 未找到 TMP_Dropdown 组件！");
        }
    }

    public void PlayDropdownSound(int index)
    {
        if (GameAudioManager.instance != null)
        {
            GameAudioManager.instance.PlaySFX(0); // 选择合适的音效索引
            Debug.Log($"Dropdown 选项 {index} 被选择，播放音效。");
        }
        else
        {
            Debug.LogError("GameAudioManager 实例未找到！");
        }
    }

    private void OnDestroy()
    {
        if (dropdown != null)
        {
            dropdown.onValueChanged.RemoveListener(PlayDropdownSound);
        }
    }
}
