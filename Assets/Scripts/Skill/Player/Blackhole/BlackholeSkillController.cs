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
    private List<Transform> targets;  // Changed to List for easier random access
    private Queue<GameObject> hotkeyPool;  // Optimized pool for hotkeys

    private enum BlackholeState
    {
        Growing,
        Shrinking,
        Idle
    }

    private BlackholeState currentState = BlackholeState.Idle;

    private void Awake()
    {
        // Initialize keycodes for hotkeys
        keyCodes = new List<KeyCode>
        {
            KeyCode.A,
            KeyCode.S,
            KeyCode.D,
            KeyCode.W,
            KeyCode.E
        };

        // Initialize targets list and hotkey pool
        targets = new List<Transform>();
        hotkeyPool = new Queue<GameObject>();

        // Start blackhole in Growing state
        currentState = BlackholeState.Growing;
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
        this.blackholeDuration = blackholeDuration;
        blackholeTimer = blackholeDuration;

        // Whether the player can disappear depends on whether they use crystals instead of clones
        playerCanDisaper = !SkillManager.Instance.Clone.crystalInsteadOfClone;
    }

    private void Update()
    {
        // Update timers
        cloneAttackTimer -= Time.deltaTime;
        blackholeTimer -= Time.deltaTime;

        // Blackhole state management: Growing -> Idle -> Shrinking
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
            if (transform.localScale.x <= 0) Destroy(gameObject);  // Destroy when scale reaches 0
        }

        // Manage the blackhole timer and decide when to release clones or finish the blackhole
        if (blackholeTimer < 0)
        {
            blackholeTimer = Mathf.Infinity;
            if (targets.Count > 0) ReleaseCloneAttack();
            else FinishBlackhole();
        }

        // Trigger clone attack manually
        if (Input.GetKeyDown(KeyCode.F))
        {
            ReleaseCloneAttack();
        }

        // Logic for performing clone attacks while the blackhole is growing
        CloneAttackLogic();
    }

    private void ReleaseCloneAttack()
    {
        if (targets.Count <= 0) return;  // No targets to attack

        cloneAttackReleased = true;
        DestroyHotkeys();  // Destroy any created hotkeys when releasing clone attack

        if (playerCanDisaper) player.MakeTransprent(true);  // Make player transparent if allowed
    }

    private void FinishBlackhole()
    {
        cloneAttackReleased = false;
        currentState = BlackholeState.Shrinking;  // Change to shrinking state when finished
        PlayerCanExitState = true;
        DestroyHotkeys();
    }

    private void CloneAttackLogic()
    {
        if (cloneAttackReleased && amountOfAttacks > 0 && cloneAttackTimer <= 0)
        {
            // Reset the timer for the next attack
            cloneAttackTimer = cloneAttackCooldown;

            // Randomly select a target from the list
            int randomIndex = Random.Range(0, targets.Count);
            var pos = targets[randomIndex].position;

            var offset = new Vector3(Random.Range(0, 100) > 50 ? 2 : -2, 0, 0);  // Random horizontal offset

            // Create clone or crystal based on player settings
            if (SkillManager.Instance.Clone.crystalInsteadOfClone)
            {
                SkillManager.Instance.Crystal.CreateCrystal();
                SkillManager.Instance.Crystal.CurrentCrystalChooseRandomTarget();
            }
            else
            {
                SkillManager.Instance.Clone.CreateClone(pos, Quaternion.identity, offset);
            }

            amountOfAttacks--;

            // If no more attacks left, finish blackhole after a short delay
            if (amountOfAttacks <= 0)
            {
                Invoke(nameof(FinishBlackhole), 0.5f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;

        if (other.TryGetComponent(out Enemy enemy)) enemy.FreezeTime(true);  // Freeze enemy in time

        CreateHotkey(other);  // Create a hotkey for this enemy
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;

        if (other.TryGetComponent(out Enemy enemy)) enemy.FreezeTime(false);  // Unfreeze enemy when exiting the trigger
    }

    private GameObject GetHotkeyFromPool()
    {
        // If the pool has any hotkeys, reuse one; otherwise, instantiate a new one
        if (hotkeyPool.Count > 0)
        {
            return hotkeyPool.Dequeue();
        }
        else
        {
            return Instantiate(hotkeyPrefab);  // Create a new hotkey if pool is empty
        }
    }

    private void ReturnHotkeyToPool(GameObject hotkey)
    {
        hotkey.SetActive(false);  // Set the hotkey as inactive before returning to the pool
        hotkeyPool.Enqueue(hotkey);  // Reuse the hotkey
    }

    private void CreateHotkey(Collider2D other)
    {
        if (keyCodes.Count <= 0 || cloneAttackReleased) return;  // Prevent hotkey creation if no keys left or clone attack is released

        var pos = other.transform.position + new Vector3(0, 2);  // Position above the enemy
        var newHotkey = GetHotkeyFromPool();
        newHotkey.transform.position = pos;
        newHotkey.SetActive(true);

        // Randomly choose a key for the hotkey and remove it from available key codes
        KeyCode choosenKey = keyCodes[Random.Range(0, keyCodes.Count)];
        keyCodes.Remove(choosenKey);

        var blackholeAC = newHotkey.GetComponent<BlackholeHotkeyController>();
        if (blackholeAC != null)
        {
            blackholeAC.SetupHotkey(choosenKey, other.transform, this);  // Setup the hotkey with the selected key
        }
    }

    private void DestroyHotkeys()
    {
        // Return all hotkeys to the pool
        while (hotkeyPool.Count > 0)
        {
            GameObject hotkey = hotkeyPool.Dequeue();
            hotkey.SetActive(false);  // Set inactive before pooling
        }
    }

    public void AddEnemyToList(Transform enemy)
    {
        if (!targets.Contains(enemy))  // Add enemy to the target list if not already present
        {
            targets.Add(enemy);
        }
    }
}
