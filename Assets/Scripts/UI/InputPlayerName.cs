using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputPlayerName : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private Button submitButton; 

    private void Start()
    {
        submitButton.onClick.AddListener(SubmitName);
    }

    // �ύ��������ĺ���
    private void SubmitName()
    {
        string playerName = nameInputField.text.Trim(); // ��ȡ������벢�Ƴ���β�ո�

        if (!string.IsNullOrEmpty(playerName))
        {
            Debug.Log($"��ҵ�������: {playerName}");
            //PlayerManager.playerName = playerName;
            PlayerPrefs.SetString("PlayerName", playerName);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.LogWarning("���ֲ���Ϊ�գ�");
        }
    }
}
