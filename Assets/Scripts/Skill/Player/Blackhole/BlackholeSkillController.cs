using System.Collections.Generic;
using UnityEngine;

public class BlackholeSkillController : MonoBehaviour
{
    public GameObject hotkeyPrefab;
    public bool PlayerCanExitState { get; private set; }

    private float maxSize;
    private float growSpeed;
    private float shrinkSpeed;
    private int amountOfAttacks;
    private float cloneAttackCooldown;

    private float cloneAttackTimer;
    private bool cloneAttackReleased;

    private float blackholeTimer;
    private float blackholeDuration;

    private bool playerCanDisaper;

    private Player player;
    private List<KeyCode> keyCodes;
    private HashSet<Transform> targets;
    private List<GameObject> createdHotkeys;

    private enum BlackholeState
    {
        Growing,
        Shrinking,
        Idle
    }


    private void Awake()
    {
        keyCodes = new List<KeyCode>
            {
                KeyCode.A,
                KeyCode.S,
                KeyCode.D,
                KeyCode.W,
                KeyCode.E
            };
        targets = new HashSet<Transform>();
        createdHotkeys = new List<GameObject>();
        currentState = BlackholeState.Growing;
        //canGrow = true;
    }

    public void Setup(
        Player player,
        float maxSize,
        float growSpeed,
        float shrinkSpeed,
        int amountOfAttacks,
        float cloneAttackCooldown,
        float blackholeDuration)
    {
        this.player = player;
        this.maxSize = maxSize;
        this.growSpeed = growSpeed;
        this.shrinkSpeed = shrinkSpeed;
        this.amountOfAttacks = amountOfAttacks;
        this.cloneAttackCooldown = cloneAttackCooldown;
        this.blackholeDuration = blackholeDuration*2;
        blackholeTimer = blackholeDuration;

        //playerCanDisaper = !SkillManager.Instance.Clone.crystalInsteadOfClone;
    }


    private BlackholeState currentState = BlackholeState.Idle;

    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackholeDuration -= Time.deltaTime;
        blackholeTimer -= Time.deltaTime;
        // 黑洞状态管理
        if (currentState == BlackholeState.Growing)
        {
            var size = new Vector2(maxSize, maxSize);
            transform.localScale = Vector2.Lerp(transform.localScale, size, growSpeed * Time.deltaTime);
            if (transform.localScale.x >= maxSize) currentState = BlackholeState.Idle;
        }
        else if (currentState == BlackholeState.Shrinking)
        {
            var size = new Vector2(-1, -1);
            transform.localScale = Vector2.Lerp(transform.localScale, size, shrinkSpeed * Time.deltaTime);
            if (transform.localScale.x <= 0) Destroy(gameObject);
        }


        if (blackholeTimer < 0)
        {
            blackholeTimer = Mathf.Infinity;
            if (targets.Count > 0) ReleaseCloneAttack();
            else FinishBlackhole();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            ReleaseCloneAttack();
        }

        if(blackholeDuration < 0){
            FinishBlackhole();
        }

        CloneAttackLogic();

    }

    private void ReleaseCloneAttack()
    {
        if (targets.Count <= 0) return;
        cloneAttackReleased = true;
        DestroyHotkeys();
        if (!playerCanDisaper) return;
        player.MakeTransprent(true);
    }

    private void FinishBlackhole()
    {
        cloneAttackReleased = false;
        currentState = BlackholeState.Shrinking;
        PlayerCanExitState = true;
        DestroyHotkeys();
    }

        private void CloneAttackLogic()
    {
        if (cloneAttackReleased && amountOfAttacks > 0 && cloneAttackTimer <= 0)
        {
            cloneAttackTimer = cloneAttackCooldown;
            
            // 将 HashSet 转换为 List 进行随机访问
            List<Transform> targetList = new List<Transform>(targets);

            // 过滤掉已经死亡或销毁的目标
            targetList.RemoveAll(target => target == null); 

            if (targetList.Count == 0) return;  // 如果没有有效目标，则跳过攻击逻辑

            int randomIndex = Random.Range(0, targetList.Count);
            var pos = targetList[randomIndex].position;

            var offset = new Vector3(Random.Range(0, 100) > 50 ? 2 : -2, 0, 0);

            SkillManager.Instance.Clone.CreateClone(pos, Quaternion.identity, offset);
            

            amountOfAttacks--;

            // 若攻击次数已用尽，结束黑洞技能
            if (amountOfAttacks <= 0)
            {
                Invoke(nameof(FinishBlackhole), 0.5f);
            }
        }
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;

        if (other.TryGetComponent(out Enemy enemy)) enemy.FreezeTime(true);

        CreateHotkey(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;

        if (other.TryGetComponent(out Enemy enemy)) enemy.FreezeTime(false);
    }

    private Queue<GameObject> hotkeyPool = new Queue<GameObject>();

    private GameObject GetHotkeyFromPool()
    {
        if (hotkeyPool.Count > 0)
        {
            return hotkeyPool.Dequeue();
        }
        else
        {
            // 如果池空了，创建一个新的热键
            return Instantiate(hotkeyPrefab);
        }
    }

    private void ReturnHotkeyToPool(GameObject hotkey)
    {
        hotkey.SetActive(false);  // 使其不可见
        hotkeyPool.Enqueue(hotkey);
    }

    private void CreateHotkey(Collider2D other)
    {
        if (keyCodes.Count <= 0 || cloneAttackReleased) return;

        var pos = other.transform.position + new Vector3(0, 2);
        var newHotkey = GetHotkeyFromPool();
        newHotkey.transform.position = pos;
        newHotkey.SetActive(true);

        // 将热键添加到池中（避免重复创建）
        if (!createdHotkeys.Exists(h => h == newHotkey))
        {
            createdHotkeys.Add(newHotkey);
        }
        else
        {
            ReturnHotkeyToPool(newHotkey);
            return;
        }

        KeyCode choosenKey = keyCodes[Random.Range(0, keyCodes.Count)];
        keyCodes.Remove(choosenKey);

        var blackholeAC = newHotkey.GetComponent<BlackholeHotkeyController>();
        if (!blackholeAC) return;

        blackholeAC.SetupHotkey(choosenKey, other.transform, this);
    }


    private void DestroyHotkeys()
    {
        if (createdHotkeys.Count <= 0) return;
        for (int i = 0; i < createdHotkeys.Count; i++)
        {
            Destroy(createdHotkeys[i]);
        }
        createdHotkeys.Clear();
    }

    public void AddEnemyToList(Transform enemy)
    {
        if (!targets.Contains(enemy))
        {
            targets.Add(enemy);
        }
    }
}
