using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialImage;  // Ҫ��ʾ�Ľ̳�ͼƬ

    void Start()
    {
        // ȷ��ͼƬ����Ϸ��ʼʱ��ʾ
        tutorialImage.SetActive(true);
    }

    // ��������һ������������ͼƬ����ʧ
    public void HideTutorialImage()
    {
        tutorialImage.SetActive(false);
    }
}
