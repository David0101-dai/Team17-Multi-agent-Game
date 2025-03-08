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
    public Button continueButton; // 继续游戏按钮
    public Button newGameButton;  // 新游戏按钮
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
        newGameButton.onClick.AddListener(StartNewGame);
    }

    public void ContinueGame()
    {
        StartCoroutine(LoadSceneWithFadeEffect(1.5f));
        if (saveManager.CurrentGameData() != null)
        {
            StartCoroutine(LoadSceneWithFadeEffect(1.5f));  // 游戏场景名称
        }
        else
        {
            Debug.LogError("Failed to load game data, cannot continue.");
        }
    }

    public void StartNewGame()
    {
        saveManager.DeleteSaveData();
        saveManager.NewGame();
        StartCoroutine(LoadSceneWithFadeEffect(1.5f));
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
