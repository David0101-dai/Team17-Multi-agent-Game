using System.Collections;
using System.Collections.Generic;
using System.Data;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using UnityEngine.U2D.IK;

public enum BossStage {stage1, stage2, stage3, stage4}
public class Boss : Enemy
{
   [SerializeField] public BossStage BossStage;
   public GameObject Door;
   private bool firstEnterStage2 = true;
   private bool firstEnterStage3 = true;
   private bool firstEnterStage4 = true;  
   [Header("Teleport detail")]
   [SerializeField] private BoxCollider2D teleportArea;
   [SerializeField] private Vector2 surroundingCheckSize;
   [SerializeField] private BoxCollider2D cd;
   public float chanceToTeleport;
   public float defaultChanceToTeleport = 25f;

   [Header("Spell Cast detail")]
   public GameObject spellPrefab;
   public float spellCastCoolDown = 10f;
   public float spellCastCoolDownTimer = 0f;
   public int amountOfSpells = 3;
   public float spellCoolDown = 0.5f;

#region State
    public IState IdleState { get; private set; }
    public IState PatrolState { get; private set; }
    public IState ChaseState { get; private set; }
    public IState AttackState { get; private set; }
    public IState HitState { get; private set; }
    public IState DeadState { get; private set; }
    public IState StunState { get; private set; }
    public IState TeleportState { get; private set; }
    public IState SpellCastState { get; private set; } 
#endregion

    protected override void Start()
    {
        base.Start();
        cd = GetComponent<BoxCollider2D>();
        IdleState = new BossIdleState(Fsm, this, "Idle");
        PatrolState = new BossPatrolState(Fsm, this, "Move");
        ChaseState = new BossChaseState(Fsm, this, "Move");
        AttackState = new BossAttackState(Fsm, this, "Attack");
        HitState = new BossHitState(Fsm, this, "Hit");
        DeadState = new BossDeadState(Fsm, this, "Dead");
        StunState = new BossStunState(Fsm, this, "Stun");
        TeleportState = new BossTeleportState(Fsm, this, "Teleport");
        SpellCastState = new BossSpellCastState(Fsm, this, "Cast");
        BossStage = BossStage.stage1;
        Fsm.SwitchState(IdleState);

        // 重写 Damageable.OnTakeDamage 事件
        Damageable.OnTakeDamage += (from, to) =>
        {
            // 你可以在这里加入一些Boss特定的处理逻辑

            // 调用 AudioManager 播放音效
            AudioManager.instance.PlaySFX(6,null); // 替换为你想播放的音效名称
        };
    }

    public void SpellCast()
    {
        //GameObject player = PlayerManager.Instance.player;
        Player player = PlayerManager.Instance.player.GetComponent<Player>();
        //Vector3 spellPosition = new Vector3(player.transform.position.x + player.GetComponent<Player>().Flip.facingDir*3, player.transform.position.y + 1);
        if(player.Rb.velocity.x == 0)
        {
            Vector3 spellPosition = new Vector3(player.transform.position.x+2, player.transform.position.y+2.5f);
            var spell = Instantiate(spellPrefab, spellPosition, Quaternion.identity);
            spell.GetComponent<Hand>().SetUp(gameObject);  // 修改为 SetUp 而不是 Setup
        }
        else if(player.Rb.velocity.x > 0)
        {
            Vector3 spellPosition = new Vector3(player.transform.position.x+4, player.transform.position.y+2.5f);
            var spell = Instantiate(spellPrefab, spellPosition, Quaternion.identity);
            spell.GetComponent<Hand>().SetUp(gameObject);  // 修改为 SetUp 而不是 Setup
        }
        else if(player.Rb.velocity.x < 0)
        {
            Vector3 spellPosition = new Vector3(player.transform.position.x, player.transform.position.y+2.5f);
            var spell = Instantiate(spellPrefab, spellPosition, Quaternion.identity);
            spell.GetComponent<Hand>().SetUp(gameObject);  // 修改为 SetUp 而不是 Setup
        }
    }


    private RaycastHit2D GroundBelow() => Physics2D.Raycast(transform.position, Vector2.down, 100, LayerMask.GetMask("Platform"));
    private RaycastHit2D SomethingIsAround() => Physics2D.BoxCast(transform.position, surroundingCheckSize, 0, Vector2.zero, 0, LayerMask.GetMask("Platform"));
    
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y-GroundBelow().distance));
        Gizmos.DrawWireCube(transform.position, surroundingCheckSize);
    }

   public void FindPosition(int maxAttempts = 10, int currentAttempt = 0){

    if (currentAttempt > maxAttempts)
    {
        Debug.LogWarning("Max attempts reached, teleportation failed.");
        return;  // 达到最大尝试次数，停止递归
    }

    float x = Random.Range(teleportArea.bounds.min.x + 3, teleportArea.bounds.max.x - 3);
    float y = Random.Range(teleportArea.bounds.min.y + 3, teleportArea.bounds.max.y - 3);

    // 根据新的 teleportArea 位置来设置目标位置
    transform.position = new Vector3(x, y);
    transform.position = new Vector3(transform.position.x, transform.position.y - GroundBelow().distance + (cd.size.y / 2));

    // 如果碰到障碍或不在地面上，则重新寻找位置
    if (!GroundBelow() || SomethingIsAround())
    {
        Debug.Log("Find again");
        FindPosition(maxAttempts, currentAttempt + 1);  // 递归时增加当前尝试次数
    }
}
    public bool CanSpellCast() => spellCastCoolDownTimer <= 0;
    public void ResetSpellCoolDown() => spellCastCoolDownTimer = spellCastCoolDown;
    public void DecreaseSpellCoolDown(float amount) => spellCastCoolDown -= amount;
    public void AddSpellAmount(int amount) => amountOfSpells += amount;
    public bool CanTeleport() => Random.Range(0, 100) < chanceToTeleport;
    public void AddChanceToTeleport(float amount) => chanceToTeleport += amount;
    public void AddDefaultChanceToTeleport(float amount) => defaultChanceToTeleport += amount;
    public void ResetChanceToTeleport() => chanceToTeleport = defaultChanceToTeleport;
    
    protected override void Update()
    {
        base.Update();
        spellCastCoolDownTimer -= Time.deltaTime;
        if(Damageable.currentHp <= Damageable.MaxHp.GetValue()*0.67 && BossStage == BossStage.stage1 && firstEnterStage2)
        {
            BossStage = BossStage.stage2;
            Damageable.MakeInvincible(true);
            spellCastCoolDownTimer = 0;
            Fsm.SwitchState(TeleportState);
            AddDefaultChanceToTeleport(10);
            DecreaseSpellCoolDown(1f);
            AddSpellAmount(2);
            firstEnterStage2 = false;
            Damageable.MakeInvincible(false);
        } 

        if(Damageable.currentHp <= Damageable.MaxHp.GetValue()*0.33 && BossStage == BossStage.stage2 && firstEnterStage3)
        {
            BossStage = BossStage.stage3;
            spellCastCoolDownTimer = 0;
            Damageable.MakeInvincible(true);
            Fsm.SwitchState(TeleportState);
            AddDefaultChanceToTeleport(5);
            Anim.speed = 1.25f;
            DecreaseSpellCoolDown(1f);
            AddSpellAmount(3);
            firstEnterStage3 = false;
            Damageable.MakeInvincible(false);
        }

        if(Damageable.currentHp <= Damageable.MaxHp.GetValue()*0.1 && BossStage == BossStage.stage3 && firstEnterStage4)
        {
            BossStage = BossStage.stage4;
            spellCastCoolDownTimer = 0;
            Damageable.MakeInvincible(true);
            Fsm.SwitchState(TeleportState);
            FlashFX.RedBlink(true);
            AddDefaultChanceToTeleport(5);
            Anim.speed = 1.5f;
            DecreaseSpellCoolDown(3f);
            AddSpellAmount(5);
            firstEnterStage4 = false;
            Damageable.MakeInvincible(false);
        }
    }

    

    protected override void SwitchHitState()
    {
        Fsm.SwitchState(HitState);
    }

    protected override void SwitchStunState()
    {
        Fsm.SwitchState(StunState);
    }

    protected override bool IsInStunState()
    {
        return Fsm.CurrentState == StunState;
    }

    public override void Die()
    {
        PlayerManager.Instance.AddScore(1000);
        Door.SetActive(true);
        Debug.Log(PlayerManager.finalscore);
        Fsm.SwitchState(DeadState);
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }

    public override void SlowBy(float slowPercentage, float slowDuration)
    {
        StartCoroutine(Slow(slowPercentage, slowDuration));
        IEnumerator Slow(float slowPercentage, float slowDuration)
        {
            var slow = 1 - slowPercentage;
            Anim.speed = slow;
            moveSpeed *= slow;
            yield return new WaitForSeconds(slowDuration);
            Anim.speed = 1;
            moveSpeed = defaultMoveSpeed;
        }
    }


        
}
