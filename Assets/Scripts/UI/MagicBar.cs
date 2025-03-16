using UnityEngine;
using UnityEngine.UI;

public class MagicBar : MonoBehaviour
{
    // 在 Inspector 中关联 Slider 组件
    public Slider magicSlider;

    private void Start()
    {
        // 设置 Slider 的最大值为 1（归一化）
        if (magicSlider != null)
        {
            magicSlider.minValue = 0f;
            magicSlider.maxValue = 1f;
        }
    }

    private void Update()
    {
        if (magicSlider != null && MagicManager.Instance != null)
        {
            // 将当前魔法值归一化（0 到 1）
            magicSlider.value = MagicManager.Instance.currentMagic / MagicManager.Instance.maxMagic;
        }
    }
}
