using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialImage;  // 要显示的教程图片

    void Start()
    {
        // 确保图片在游戏开始时显示
        tutorialImage.SetActive(true);
    }

    // 可以设置一个方法来控制图片的消失
    public void HideTutorialImage()
    {
        tutorialImage.SetActive(false);
    }
}
