using UnityEngine;
using UnityEngine.UI;

public class MagicBar : MonoBehaviour
{
    // �� Inspector �й��� Slider ���
    public Slider magicSlider;

    private void Start()
    {
        // ���� Slider �����ֵΪ 1����һ����
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
            // ����ǰħ��ֵ��һ����0 �� 1��
            magicSlider.value = MagicManager.Instance.currentMagic / MagicManager.Instance.maxMagic;
        }
    }
}
