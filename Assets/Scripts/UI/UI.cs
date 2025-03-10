using System.Collections;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    public TextMeshProUGUI endText;    
    public UI_FadeScreen fadeScreen;
    public GameObject ReStartButton;
    public INGAMEUI inGameUI; 
    public Transform Equipment;
    public Transform Inventory;
    public Transform Stash;
    public Transform Stat;
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
    public SkillTreeSlot timeStopUnlockButton;
    public SkillTreeSlot volnurableUnlockButton;
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
        
    }
    private void Start()
    {
        SwitchTo(null);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)||(characterUI.activeSelf&&Input.GetKeyDown(KeyCode.Escape)))
        {
            pauseManager.closeUI();
            if (!PauseManager.isPaused)
            {
                pauseManager.TogglePauseUI();
            }
            SwitchWithKeyTo(characterUI);
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

    IEnumerator EndScreenCorutione(){
        yield return new WaitForSeconds(1);
        endText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        ReStartButton.SetActive(true);
    }

    public UI_FadeScreen getFadeScreen(){
        return fadeScreen;
    }

    public void ReStartGameButton() => GameManager.Instance.ReStartGame();   
}