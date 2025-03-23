using System.Collections;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    public TextMeshProUGUI endText;    
    public TextMeshProUGUI SuccessText; 
    public UI_FadeScreen fadeScreen;
    public GameObject ReStartButton;
    public GameObject ReturnHomeButton;
    public INGAMEUI inGameUI; 
    public Transform Equipment;
    public Transform Inventory;
    public Transform Stash;
    public Transform Stat;
    //public static UI Instance { get; private set; }

    public ConfirmPopup confirmPopup; // 在 Inspector 里拖拽绑定
    [Space]
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionsUI;

    [SerializeField] public Tooltip tooltip;
    [SerializeField] public UI_SkillToolTip skillToolTip;
    [SerializeField] private PauseManager pauseManager;


    [Header("SKILL")]
    public SkillTreeSlot dashUnlockedButton;
    public SkillTreeSlot cloneOnDashUnlockedButton;
    public SkillTreeSlot counterUnlockedButton;
    public SkillTreeSlot counterBuffUnlockedButton;
    public SkillTreeSlot counterMiragelUnlockedButton;
    //public SkillTreeSlot timeStopUnlockButton;
    //public SkillTreeSlot volnurableUnlockButton;
    public SkillTreeSlot swordUnlockButton;
    public SkillTreeSlot bounceUnlockButton;
    public SkillTreeSlot pierceUnlockButton;
    public SkillTreeSlot spinUnlockButton;
    public SkillTreeSlot unlockCrystalButton;
    public SkillTreeSlot unlockCloneInsteadbutton;
    public SkillTreeSlot unlockMovingButton;
    public SkillTreeSlot unlockExplosiveButton;
    public SkillTreeSlot unlockMultiStackButton;
    public SkillTreeSlot unlockCloneAttackbutton;
    public SkillTreeSlot unlockMultipleClonebutton;
    public SkillTreeSlot unlockBlackHolebutton;

    public SkillTreeSlot attack1;
    public SkillTreeSlot attack2;
    public SkillTreeSlot attack3;

    public SkillTreeSlot health1;
    public SkillTreeSlot health2;
    public SkillTreeSlot health3;

    public SkillTreeSlot speed1;
    public SkillTreeSlot speed2;
    public SkillTreeSlot speed3;

    public SkillTreeSlot defence1;
    public SkillTreeSlot defence2;
    public SkillTreeSlot defence3;

    public static UI Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
            Debug.Log("UI Initialized.");
        }
        else
        {
            Destroy(gameObject);  // 如果实例已存在，则销毁当前对象
            Debug.Log("UI instance already exists, destroying this duplicate.");
            return;
        }

        // 确保 skillToolTip 已初始化
        if (skillToolTip == null)
        {
            Debug.LogError("skillToolTip is not assigned in UI.");
        }
        else
        {
            Debug.Log("skillToolTip has been initialized.");
        }

        if(dashUnlockedButton == null){
            Debug.Log("dashUnlockedButton is not assigned in UI.");
        }else{
            Debug.Log("dashUnlockedButton has been initialized.");
        }
        fadeScreen.gameObject.SetActive(true);
    }
    private void Start()
    {
        SwitchTo(null);
    }


    private void Update()
    {
        // 1) 处理按下 C 键 -> 打开/关闭角色面板
        if (Input.GetKeyDown(KeyCode.C))
        {
            // 如果角色面板是关的，则打开并暂停游戏
            // 如果角色面板是开的，则关闭并恢复游戏
            if (!characterUI.activeSelf)
            {
                // 先保证其他UI都关掉
                SwitchTo(null);
                // 如果游戏没暂停就暂停
                if (!PauseManager.isPaused) pauseManager.TogglePauseUI();
                characterUI.SetActive(true);
            }
            else
            {
                // 关闭角色UI并恢复游戏
                characterUI.SetActive(false);
                if (PauseManager.isPaused) pauseManager.TogglePauseUI();
            }
            return;
        }

        // 2) 处理按下 ESC 键 -> 关闭当前打开的 UI
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 如果角色面板是开着的
            if (characterUI.activeSelf)
            {
                characterUI.SetActive(false);
                if (PauseManager.isPaused) pauseManager.TogglePauseUI();
            }
            // 如果合成面板是开着的
            else if (craftUI.activeSelf)
            {
                craftUI.SetActive(false);
                if (PauseManager.isPaused) pauseManager.TogglePauseUI();
            }
            // 如果技能树是开着的
            else if (skillTreeUI.activeSelf)
            {
                skillTreeUI.SetActive(false);
                if (PauseManager.isPaused) pauseManager.TogglePauseUI();
            }
            // 如果选项面板是开着的
            else if (optionsUI.activeSelf)
            {
                optionsUI.SetActive(false);
                if (PauseManager.isPaused) pauseManager.TogglePauseUI();
            }
            // 如果都没开，则可根据需求来决定做什么
            // 比如：打开或关闭暂停菜单 pauseManager.TogglePause();
            // 或者什么也不做。
            else
            {
                // 可以在此写：pauseManager.TogglePause();
            }
            return;
        }
    }

    public void SwitchTo(GameObject menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i).gameObject;
            bool isFadeScreen = child.GetComponent<UI_FadeScreen>() != null;
            bool isInGameUI = child.GetComponent<INGAMEUI>() != null;
            
            // 仅非FadeScreen且非InGameUI的对象会被禁用
            if (!isFadeScreen && !isInGameUI && child != pauseManager.gameObject)
            {
                child.SetActive(false);
            }
        }
        menu?.SetActive(true);
    }

    public void SwitchWithKeyTo(GameObject menu)
    {
        if (menu && menu.activeSelf)
        {
            pauseManager.TogglePauseUI();
            menu.SetActive(false);
            return;
        }
        SwitchTo(menu);
    }

    public void SwitchOnEndScreen(){
        fadeScreen.FadeOut();
        StartCoroutine(EndScreenCorutione());
    }

    public void SwitchOnVictoryScreen(){
        fadeScreen.FadeOut();
        StartCoroutine(VictoryScreenCorutione());
    }

    IEnumerator EndScreenCorutione(){
        yield return new WaitForSeconds(1);
        endText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        ReStartButton.SetActive(true);
        ReturnHomeButton.SetActive(true);
    }

    IEnumerator VictoryScreenCorutione(){
        yield return new WaitForSeconds(1);
        SuccessText.gameObject.SetActive(true);
        yield return new WaitForSeconds(30);
        PlayerManager.Instance.player.GetComponent<Damageable>().MakeInvincible(true);
        ReStartButton.SetActive(true);
        ReturnHomeButton.SetActive(true);
    }

    public UI_FadeScreen getFadeScreen(){
        return fadeScreen;
    }

    public void ReStartGameButton()
    {
        // 标记为 false，表示这不是一次需要教程的新游戏
        All.IsNewGame = false;

        // 然后调用你的重开游戏逻辑
        GameManager.Instance.ReStartGame();
    }

    public void ReturnHome() => GameManager.Instance.ReturnHome();  
}