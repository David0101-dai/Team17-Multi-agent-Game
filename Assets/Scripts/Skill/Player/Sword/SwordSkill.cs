using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class SwordSkill : Skill
{
    [Header("Skill Value")]
    [SerializeField] private SkillTreeSlot swordUnlockButton;
    public bool swordUnlocked {  get; private set; }

    [SerializeField] private SwordType swordType = SwordType.Regular;
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float returnSpeed;
    [SerializeField] private float freezeTime;

    [Header("Dots Value")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotsPrefab;

    [Header("Regular Value")]

    [SerializeField] private float regularGravity;

    [Header("Bounce Value")]
    [SerializeField] private SkillTreeSlot bounceUnlockButton;
    [SerializeField] private float bounceGravity;
    [SerializeField] private float bounceSpeed;
    [SerializeField] private int bounceAmount;

    [Header("Pierce Value")]
    [SerializeField] private SkillTreeSlot pierceUnlockButton;
    [SerializeField] private float pierceGravity;
    [SerializeField] private int pierceAmount;

    [Header("Spin Value")]
    [SerializeField] private SkillTreeSlot spinUnlockButton;
    [SerializeField] private float SpinGravity;
    [SerializeField] private float maxTravelDistance;
    [SerializeField] private float spinDuration;
    [SerializeField] private float hitCooldown;


    [Header("Passive Skills")]
    [SerializeField] private SkillTreeSlot timeStopUnlockButton;
    public bool timeStopUnlocked { get; private set; }
    [SerializeField] private SkillTreeSlot volnurableUnlockButton;
    public bool volnurableUnlocked { get; private set; }

    private float swordGravity;


    private GameObject[] dots;
    private Vector2 finalDir;

    protected override void OnEnable()
    {
        base.OnEnable();
        // 使用协程确保在 SaveManager 数据加载完成后再检查技能解锁状态
        StartCoroutine(WaitAndCheckUnlock());
    }

    private IEnumerator WaitAndCheckUnlock()
    {
        // 等待直到 SaveManager 实例存在并且 gameData 已加载
        while (SaveManager.instance == null || SaveManager.instance.CurrentGameData == null)
        {
            yield return null;
        }
        // 主动刷新技能槽数据
        timeStopUnlockButton.LoadData(SaveManager.instance.CurrentGameData);
        volnurableUnlockButton.LoadData(SaveManager.instance.CurrentGameData);
        swordUnlockButton.LoadData(SaveManager.instance.CurrentGameData);
        bounceUnlockButton.LoadData(SaveManager.instance.CurrentGameData);
        pierceUnlockButton.LoadData(SaveManager.instance.CurrentGameData);
        spinUnlockButton.LoadData(SaveManager.instance.CurrentGameData);
        // 等待一帧，确保 SkillTreeSlot 的状态更新完成
        yield return new WaitForEndOfFrame();
        
        CheckUnlock();
    }

    protected override void Start()
    {
        base.Start();

        SetGravity();

        GenereateDots();

        timeStopUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockTimeStop);
        volnurableUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockVolnurable);
        swordUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSwordUnlock);
        bounceUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBounce);
        pierceUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockPierce);
        spinUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSpin);

        timeStopUnlockButton.OnSkillCancelled += OnTimeStopSkillCancelled;
        volnurableUnlockButton.OnSkillCancelled += OnVolnurableSkillCancelled;
        swordUnlockButton.OnSkillCancelled += OnSwordUnlockSkillCancelled;
        bounceUnlockButton.OnSkillCancelled += OnBounceSkillCancelled;
        pierceUnlockButton.OnSkillCancelled += OnPierceSkillCancelled;
        spinUnlockButton.OnSkillCancelled += OnSpinSkillCancelled;

    }

    protected override void Update()
    {
        SetGravity();

        if (!player.InputController.isAimSwordPressed)
        {
            cooldownTimer -= Time.deltaTime;
            var dir = AimDirection();
            finalDir = new Vector2(dir.x * launchForce.x, dir.y * launchForce.y);
        }
        else
        {
            for (int i = 0; i < numberOfDots; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }
    }

    public void CreateSword()
    {
        var newSword = Instantiate(swordPrefab, player.transform.position 
        + new Vector3(0, 1, 0), transform.rotation);
        newSword.transform.SetParent(PlayerManager.Instance.fx.transform);
        var newSwordController = newSword.GetComponent<SwordSKillController>();

        newSwordController.Setup(
            swordType,
            player,
            finalDir,
            swordGravity,
            returnSpeed,
            bounceSpeed,
            bounceAmount,
            pierceAmount,
            maxTravelDistance,
            spinDuration,
            hitCooldown,
            freezeTime);

        player.UsedSword = newSword;

        SetDotsActive(false);
    }

    private void SetGravity()
    {
        switch (swordType)
        {
            case SwordType.Regular:
                swordGravity = regularGravity;
                break;
            case SwordType.Bounce:
                swordGravity = bounceGravity;
                break;
            case SwordType.Pierce:
                swordGravity = pierceGravity;
                break;
            case SwordType.Spin:
                swordGravity = SpinGravity;
                break;
            default:
                break;
        }
    }

    private void OnTimeStopSkillCancelled()
    {
        timeStopUnlocked = false;
        // 此处可以加入关闭技能效果的逻辑
    }

    private void OnVolnurableSkillCancelled()
    {
        volnurableUnlocked = false;
        // 此处可以加入关闭技能效果的逻辑
    }

    private void OnSwordUnlockSkillCancelled()
    {
        swordUnlocked = false;
        // 此处可以加入关闭技能效果的逻辑
    }

    private void OnBounceSkillCancelled()
    {
        swordType = SwordType.Regular;
    }

    private void OnPierceSkillCancelled()
    {
        swordType = SwordType.Regular;
    }

    private void OnSpinSkillCancelled()
    {
        swordType = SwordType.Regular;
    }




    #region Unlock Skill Region
    protected override void CheckUnlock()
    {
        UnlockTimeStop();
        UnlockVolnurable();
        UnlockSwordUnlock();
        UnlockBounce();
        UnlockPierce();
        UnlockSpin();
    }
    private void UnlockTimeStop()
    {
        if (timeStopUnlockButton.unlocked)
            timeStopUnlocked = true;
    }

    private void UnlockVolnurable()
    {
        if (volnurableUnlockButton.unlocked)
            volnurableUnlocked = true;
    }
    private void UnlockSwordUnlock()
    {
        if (swordUnlockButton.unlocked){
            swordType = SwordType.Regular;
            swordUnlocked = true;
        }   
    }
    private void UnlockBounce()
    {
        if (bounceUnlockButton.unlocked)
            swordType = SwordType.Bounce;
    }
    private void UnlockPierce()
    {
        if (pierceUnlockButton.unlocked)
            swordType = SwordType.Pierce;
    }
    private void UnlockSpin()
    {
        if (spinUnlockButton.unlocked)
            swordType = SwordType.Spin;
    }
    #endregion


    #region Aim
    public void SetDotsActive(bool isActive)
    {
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i].SetActive(isActive);
        }
    }

    private void GenereateDots()
    {
        dots = new GameObject[numberOfDots];
        var parent = PlayerManager.Instance.fx.transform;
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotsPrefab, player.transform.position 
            + new Vector3(0, 1, 0), Quaternion.identity, parent);
            dots[i].SetActive(false);
        }
    }

    private Vector2 AimDirection()
    {
        var playerPos = player.transform.position + new Vector3(0, 1, 0);
        var mousePos = Camera.main.ScreenToWorldPoint(player.InputController.mousePosition);
        return (mousePos - playerPos).normalized;
    }

    private Vector2 DotsPosition(float space)
    {
        var aimPos = AimDirection();
        var pos = (Vector2)player.transform.position + new Vector2(0, 1)
                + new Vector2(aimPos.x * launchForce.x, aimPos.y * launchForce.y) * space
                + 0.5f * (Physics2D.gravity * swordGravity) * (space * space);
        return pos;
    }
    #endregion

    protected override void SkillFunction()
    {
    }
}
