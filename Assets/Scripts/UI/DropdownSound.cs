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
            Debug.LogError("DropdownSound: δ�ҵ� TMP_Dropdown �����");
        }
    }

    public void PlayDropdownSound(int index)
    {
        if (GameAudioManager.instance != null)
        {
            GameAudioManager.instance.PlaySFX(0); // ѡ����ʵ���Ч����
            Debug.Log($"Dropdown ѡ�� {index} ��ѡ�񣬲�����Ч��");
        }
        else
        {
            Debug.LogError("GameAudioManager ʵ��δ�ҵ���");
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
