using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private EnemyLevel levelstage;
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private string sceneName = "Prototype";
    [SerializeField] private UI_FadeScreen fadeScreen;
    [SerializeField] private Scores scores;
    public Button continueButton; // 继续游戏按钮
    public Button newGameButton;  // 新游戏按钮

    // 新增部分：玩家名称输入界面组件
    [SerializeField] private GameObject playerNamePanel;         // 玩家名称输入面板
    [SerializeField] private TMP_InputField playerNameInputField;  // 输入框组件
    [SerializeField] private Button submitNameButton;              // 提交名称按钮

    private SaveManager saveManager;

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
     // 初始化时隐藏名称输入面板
    if (playerNamePanel != null)
    {
        playerNamePanel.SetActive(false);
    }

    saveManager = SaveManager.instance;
    // 等待直到 SaveManager 完全初始化
    while (saveManager == null || saveManager.CurrentGameData() == null)
    {
        saveManager = SaveManager.instance;
        // 等待直到 SaveManager 完全初始化
        while (saveManager == null || saveManager.CurrentGameData() == null)
        {
            yield return null;  // 每帧检查，直到 SaveManager 和 gameData 完全加载
        }

        // 确保 SaveManager 初始化完成后，才检查是否有存档
        if (saveManager.HasSaveData())
        {
            continueButton.interactable = true;
        }
        else
        {
            continueButton.interactable = false;
        }
        continueButton.onClick.AddListener(ContinueGame);

        // 修改为点击新游戏按钮时显示玩家名称输入面板
        newGameButton.onClick.RemoveAllListeners();
        //newGameButton.onClick.AddListener(ShowPlayerNamePanel);
        //newGameButton.onClick.AddListener(StartNewGame);

    }

    // 确保 SaveManager 初始化完成后，才检查是否有存档
    if (saveManager.HasSaveData())
    {
        continueButton.interactable = true;
    }
    else
    {
        continueButton.interactable = false;
    }

    continueButton.onClick.AddListener(ContinueGame);
    newGameButton.onClick.RemoveAllListeners();
    //newGameButton.onClick.AddListener(ShowPlayerNamePanel);
    //newGameButton.onClick.AddListener(StartNewGame);


    // 为提交名称按钮添加监听
    if (submitNameButton != null)
    {
        submitNameButton.onClick.AddListener(OnSubmitPlayerName);
    }


}


public void ContinueGame()
{
    if (saveManager.CurrentGameData() != null)
    {
        StartCoroutine(LoadSceneWithFadeEffect(1.5f));  // 游戏场景名称
    }
    else
    {
        Debug.LogError("Failed to load game data, cannot continue.");
    }
    PauseManager.isPaused = false;
}
    // 点击新游戏按钮后显示输入玩家名称的面板
    public void ShowPlayerNamePanel()
    {
        if (playerNamePanel != null)
        {
            playerNamePanel.SetActive(true);
        }
    }

    // 提交玩家名称时调用
    public void OnSubmitPlayerName()
    {
        string playerName = playerNameInputField.text.Trim();
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.Log("请输入玩家名称！");
            // 可在此处增加 UI 提示，比如显示错误信息
            return;
        }

        // 保存玩家名称（这里使用 PlayerPrefs，你也可以结合 SaveManager 存储）
        PlayerPrefs.SetString("PlayerName", playerName);
        Debug.Log("玩家名称已保存：" + playerName);

        // 隐藏名称输入面板
        playerNamePanel.SetActive(false);

        // 开始新游戏流程
        StartNewGame();
    }

    public void StartNewGame()
    {
        scores.printfScores();
        saveManager.DeleteSaveData();
        saveManager.NewGame();
        StartCoroutine(LoadSceneWithFadeEffect(1.5f));
        PauseManager.isPaused = false;
    }

    public void setLevel(int index)
    {
        int level = index + 1;
        Debug.Log(level);
        levelstage.SetLevel(level);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        Debug.Log("Exit game is called in the editor.");
#else
        Application.Quit();
#endif
    }

    // 场景加载时执行淡出效果
    IEnumerator LoadSceneWithFadeEffect(float _delay)
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(_delay);
        SceneManager.LoadScene(sceneName);
    }
}
