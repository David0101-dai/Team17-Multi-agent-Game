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
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        fadeScreen.gameObject.SetActive(true);
    }
    private void Start()
    {
        SwitchTo(null);
    }


    private void Update()
    {
        if (IsFading()) return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            SimulateCKey(); // 调用统一方法
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SimulateEscapeKey(); // 调用统一方法
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

    private bool IsFading()
{
    // 检查动画是否在执行 fadeOut 动画
    return fadeScreen.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("fadeOut");
}

    public void SimulateEscapeKey()
    {
        Debug.Log("=== ESC 按钮触发 ===");

        // 定义需要检查的所有面板
        GameObject[] uiPanels = new GameObject[] {
        characterUI,
        craftUI,
        skillTreeUI,
        optionsUI,
        Stat.gameObject,
        Inventory.gameObject,
        Stash.gameObject
    };

        bool anyPanelActive = false;

        // 检查是否有面板处于激活状态
        foreach (var panel in uiPanels)
        {
            if (panel != null && panel.activeSelf)
            {
                Debug.Log($"关闭面板: {panel.name}");
                panel.SetActive(false);
                anyPanelActive = true;
            }
        }

        // 如果有面板被关闭，可能需要解除暂停
        if (anyPanelActive)
        {
            if (PauseManager.isPaused)
            {
                Debug.Log("解除暂停状态");
                pauseManager.TogglePauseUI();
            }
        }
        else // 没有面板被关闭时，切换暂停状态
        {
            Debug.Log("没有打开的面板，切换暂停菜单");
            pauseManager.TogglePauseUI();
        }
    }

    public void SimulateCKey()
    {
        // 直接调用原有 Update 中的 C 键处理逻辑
        if (!characterUI.activeSelf)
        {
            SwitchTo(null);
            if (!PauseManager.isPaused) pauseManager.TogglePauseUI();
            characterUI.SetActive(true);
        }
        else
        {
            characterUI.SetActive(false);
            if (PauseManager.isPaused) pauseManager.TogglePauseUI();
        }
    }

}