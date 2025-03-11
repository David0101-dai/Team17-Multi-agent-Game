using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    // ͨ����ť��������Ĳ�����Ч�ķ���
    public void PlaySound()
    {
        if (GameAudioManager.instance != null)
        {
            int soundIndex = 0; // �������벥�ŵ���Ч����
            GameAudioManager.instance.PlaySFX(soundIndex); // ����AudioManager������Ч
            Debug.Log("������Ч " + soundIndex);
        }
        else
        {
            Debug.LogError("AudioManager ʵ��δ�ҵ���");
        }
    }
}
