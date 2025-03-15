using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject leaderboardPanel;  // 排行榜面板
    [SerializeField] private Transform leaderboardContent; // 排行榜内容容器
    [SerializeField] private TMP_Text leaderboardEntryPrefab;

    [SerializeField] private EnemyLevel levelstage;
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private string sceneName = "Prototype";
    [SerializeField] private UI_FadeScreen fadeScreen;
    [SerializeField] private Scores scores;
    public Button continueButton; // 继续游戏按钮
    public Button newGameButton;  // 新游戏按钮
    private bool hasOpenedBefore = false;
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
        All.IsNewGame = false;  // 标记：不是新游戏
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
        All.IsNewGame = true;  // 标记：是新游戏
        Debug.Log("Final" + PlayerManager.finalscore);
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
    public void ShowLeaderboard()
    {
        leaderboardPanel.SetActive(true);
        UpdateLeaderboard();
    }

    public void HideLeaderboard()
    {
        leaderboardPanel.SetActive(false);
    }

    private void UpdateLeaderboard()
    {
        foreach (Transform child in leaderboardContent)
        {
            if (child.gameObject.name != "Score") // 只删除非 "score" 的对象
            {
                Destroy(child.gameObject);
            }
        }

        // 获取玩家分数并按分数降序排序
        List<PlayerAndScore> sortedScores = scores.getScore();
        sortedScores.Sort((a, b) => b.score.CompareTo(a.score));

        int maxEntries = Mathf.Min(5, sortedScores.Count);
        // 遍历排名数据并创建 UI 显示
        for (int i = 0; i < maxEntries; i++)
        {
            TMP_Text entry = Instantiate(leaderboardEntryPrefab, leaderboardContent);
            entry.text = $"{i + 1}.{sortedScores[i].playerName}:{sortedScores[i].score:F2}";
            RectTransform rectTransform = entry.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(500f, 50f);
            Debug.Log("克隆对象状态1：" + entry.gameObject.activeSelf);
            entry.gameObject.SetActive(true);
            Debug.Log("克隆对象状态2：" + entry.gameObject.activeSelf);
        }
    }
    //public void cleanHistory()
    //{
    //    // 清空旧的排行榜数据
    //    foreach (Transform child in leaderboardContent)
    //    {
    //        //Destroy(child.gameObject);
            
    //    }
    //}
}
