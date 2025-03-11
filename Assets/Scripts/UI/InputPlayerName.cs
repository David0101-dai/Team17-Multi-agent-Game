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

    // 提交玩家姓名的函数
    private void SubmitName()
    {
        string playerName = nameInputField.text.Trim(); // 获取玩家输入并移除首尾空格

        if (!string.IsNullOrEmpty(playerName))
        {
            Debug.Log($"玩家的名字是: {playerName}");
            //PlayerManager.playerName = playerName;
            PlayerPrefs.SetString("PlayerName", playerName);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.LogWarning("名字不能为空！");
        }
    }
}
