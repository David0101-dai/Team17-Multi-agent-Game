using System.Collections;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    public TextMeshProUGUI endText;    
    public UI_FadeScreen fadeScreen;
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

    public static UI Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // 确保 UI 在场景切换时不被销毁
        }
        else
        {
            Destroy(gameObject);  // 如果实例已存在，则销毁当前对象
        }
    }
    private void Start()
    {
        if (fadeScreen == null)
        {
            Debug.LogError("fadeScreen 未在 UI 脚本中正确初始化！");
        }
        else
        {
            Debug.Log("fadeScreen 已初始化！");
        }

        SwitchTo(null);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchWithKeyTo(characterUI);
            return;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            SwitchWithKeyTo(craftUI);
            return;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            SwitchWithKeyTo(skillTreeUI);
            return;
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            SwitchWithKeyTo(optionsUI);
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
            if (!isFadeScreen && !isInGameUI)
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
    }

    public UI_FadeScreen getFadeScreen(){
        return fadeScreen;
    }
}