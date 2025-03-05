using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private EnemyLevel levelstage;
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private string sceneName = "MainScene";
    [SerializeField] private GameObject continueButton;
    // Start is called before the first frame update 
    private void Awake()
    {

        if (dropdown != null)
        {
            dropdown.onValueChanged.AddListener(setLevel);
        }
        else
        {
            Debug.LogError("Dropdown 组件未绑定！");
        }
    }

    private IEnumerator Start()
    {
        // 等待直到 SaveManager 完全初始化
        while (SaveManager.instance == null || SaveManager.instance.CurrentGameData == null)
        {
            yield return null;  // 每帧检查，直到 SaveManager 和 gameData 完全加载
        }

        // 确保 SaveManager 初始化完成后，才检查是否有存档
        if (!SaveManager.instance.HasSaveData())
        {
            continueButton.SetActive(false);
        }
    }
 
    public void ContinueGame(){
        SceneManager.LoadScene(sceneName);
    }

      public void StartNewGame(){
        SaveManager.instance.DeleteSaveData();
        SceneManager.LoadScene(sceneName);
    }
    public void setLevel(int index)
    {
        int level = index + 1;
        Debug.Log(level);
        levelstage.SetLevel(level);
    }
    public void ExitGame(){
       #if UNITY_EDITOR
        Debug.Log("Exit game is called in the editor.");
    #else
        Application.Quit();
    #endif
    }
}

