using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

public abstract class Damageable : MonoBehaviour
{
    public int currentHp;

    [Header("Major Stats")]
    public Stats Str;  // 力量，提高物理伤害
    public Stats Agi;  // 敏捷，提高闪避率
    public Stats Int;  // 理智，提高魔法伤害
    public Stats Vit;  // 防御，减少物理伤害

    [Header("Offensive Stats")]
    public Stats Damage;      // 基础物理攻击
    public Stats CritChance;  // 暴击率
    public Stats CritPower;   // 暴击伤害
    private bool triggerCriticalStrike;

    [Header("Defensive Stats")]
    public Stats MaxHp;            // 最大生命值
    public Stats Armor;            // 护甲
    public Stats Evasion;          // 闪避
    public Stats MagicResistance;  // 抗魔

    [Header("Magic Stats")]
    public Stats FireDamage;      // 火焰伤害
    public Stats IceDamage;       // 冰霜伤害
    public Stats LightingDamage;  // 雷电伤害

    public bool IsIgnited;  // 持续火焰伤害
    public bool IsChilled;  // 减少护甲
    public bool IsShocked;  // 减少闪避率

    [Header("Ignite")]
    public float igniteDuration;
    private float ignitedTimer;
    private float igniteDamageCooldown = 0.5f;
    private float igniteDamageTimer;
    private int igniteDamage;

    [Header("Chill")]
    public float chillDuration;
    private float chilledTimer;

    [Header("Shock")]
    public GameObject thunderStrikePrefab;
    public float shockDuration;
    private float shockedTimer;
    private bool isDead;
    private bool isVulnerable;
    public bool isInvincible = false;
    public bool Miss = false;

    public event Action<GameObject, GameObject> OnTakeDamage;
    private FlashFX flashFX;
    private Character character;

    // 攻击倍率（通用）
    public float attackMultiplier = 1f;

    protected virtual void Start()
    {
        currentHp = MaxHp.GetValue();
        CritPower.SetDefaultValue(150); // 默认暴击伤害 150% (基础+50%)
        flashFX = GetComponent<FlashFX>();
        character = GetComponent<Character>();
        isDead = false;
        triggerCriticalStrike = false;
    }

    private void Update()
    {
        ignitedTimer -= Time.deltaTime;
        igniteDamageTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;

        if (ignitedTimer < 0) IsIgnited = false;
        if (chilledTimer < 0) IsChilled = false;
        if (shockedTimer < 0) IsShocked = false;

        // 如果处于点燃状态，每隔一段时间造成一次伤害
        if (IsIgnited && igniteDamageTimer < 0)
        {
            currentHp -= igniteDamage;
            if (currentHp <= 0)
            {
                Die();
            }
            igniteDamageTimer = igniteDamageCooldown;
        }
    }

    public void MakeVulnerableFor(float _duration) => StartCoroutine(VulnerableCorutine(_duration));
    public void MakeInvincible(bool _invincible) => isInvincible = _invincible;

    private IEnumerator VulnerableCorutine(float _duration)
    {
        isVulnerable = true;
        yield return new WaitForSeconds(_duration);
        isVulnerable = false;
    }

    public virtual void TakeDamage(
        GameObject from,
        bool isMagic = false,
        bool canEffect = true,
        bool isFromSwordSkill = false,
        bool isFireDamage = false,
        bool isIceDamage = false,
        bool isShockDamage = false)
    {
        if (isDead) return;

        if (currentHp <= 0)
        {
            isDead = true;
            Die();
            currentHp = 0;
            return;
        }

        if (isInvincible)
        {
            Debug.Log($"{gameObject.name}处于无敌状态，忽略伤害");
            return;
        }

        var damageFrom = from.GetComponent<Damageable>();
        var damage = isMagic
            ? CalculateMagicDamage(damageFrom, this, isFireDamage, isIceDamage, isShockDamage)
            : CalculateDamage(damageFrom, this);

        if (isFromSwordSkill)
        {
            // 剑技伤害可能只有30%
            damage = Mathf.RoundToInt(damage * 0.3f);
        }

        if (isVulnerable)
        {
            // 易伤时伤害翻倍
            damage = Mathf.RoundToInt(damage * 2f);
        }

        currentHp -= damage;

        if (from.CompareTag("Player") && canEffect)
        {
            // 如果攻击来自玩家并且允许触发特效，则执行装备特效
            Inventory.Instance.GetEquipmentByType(EquipmentType.Weapon)?.ExecuteItemEffect(from, gameObject);
        }

        if (CompareTag("Player") && ((float)currentHp / MaxHp.GetValue()) < 0.3f)
        {
            // 如果是玩家，并且血量低于30%，则触发护甲特效
            Inventory.Instance.GetEquipmentByType(EquipmentType.Armor)?.ExecuteItemEffect(from, gameObject);
        }

        if (isMagic)
        {
            var fireDamageVal = damageFrom.FireDamage.GetValue();
            var iceDamageVal = damageFrom.IceDamage.GetValue();
            var lightingDamageVal = damageFrom.LightingDamage.GetValue();

            if (Mathf.Max(fireDamageVal, iceDamageVal, lightingDamageVal) <= 0) return;

            bool canApplyIgnite = isFireDamage && fireDamageVal > 0;
            bool canApplyChill = isIceDamage && iceDamageVal > 0;
            bool canApplyShock = isShockDamage && lightingDamageVal > 0;

            if (canApplyIgnite)
            {
                SetupIgniteDamage(Mathf.RoundToInt(fireDamageVal * 0.2f));
            }

            ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
        }

        if (currentHp > 0)
        {
            if (damage != 0)
            {
                OnTakeDamage?.Invoke(from, gameObject);
                if (triggerCriticalStrike)
                {
                    AttackSense.Instance.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
                    triggerCriticalStrike = false;
                }
            }
            else
            {
                Debug.Log($"{gameObject.name} 回避了来自 {from.name} 的攻击");
            }
        }
        else
        {
            isDead = true;
            Die();
        }
    }

    private int CalculateDamage(Damageable from, Damageable to)
    {
        var finalEvasion = to.Evasion.GetValue() + to.Agi.GetValue();
        if (from.IsShocked) finalEvasion -= 20; // 感电可减少对方闪避

        // 闪避判定
        if (UnityEngine.Random.Range(0, 100) <= finalEvasion)
        {
            to.Miss = true;
            return 0;
        }

        // 基础物理伤害公式
        var finalDamage = Mathf.RoundToInt(
            (from.Damage.GetValue() + from.Str.GetValue()) * from.attackMultiplier
            - to.Vit.GetValue()
        );

        // 如果进攻方处于冰冻状态，可做一些减伤示例
        if (from.IsChilled)
        {
            finalDamage -= Mathf.RoundToInt(from.Armor.GetValue() * 0.8f);
        }
        else
        {
            finalDamage -= to.Armor.GetValue();
        }

        // 暴击判定
        var finalCriticalChance = from.CritChance.GetValue() + from.Agi.GetValue();
        if (UnityEngine.Random.Range(0, 100) <= finalCriticalChance)
        {
            var finalCritPower = (CritPower.GetValue() + 100) * 0.01f;
            finalDamage = Mathf.RoundToInt(finalDamage * finalCritPower);
            triggerCriticalStrike = true;
        }

        finalDamage = Mathf.Clamp(finalDamage, 1, int.MaxValue);

        if (triggerCriticalStrike)
        {
            flashFX.CreatHitFX(to.transform, 1); // 暴击特效
        }
        else
        {
            flashFX.CreatHitFX(to.transform, 0); // 普通打击特效
        }

        return finalDamage;
    }

    private int CalculateMagicDamage(
        Damageable from,
        Damageable to,
        bool isFireDamage,
        bool isIceDamage,
        bool isShockDamage)
    {
        var fireDamageVal = Mathf.RoundToInt(from.FireDamage.GetValue() * from.attackMultiplier);
        var iceDamageVal = Mathf.RoundToInt(from.IceDamage.GetValue() * from.attackMultiplier);
        var lightingDamageVal = Mathf.RoundToInt(from.LightingDamage.GetValue() * from.attackMultiplier);

        // 总魔法伤害：火 + 冰 + 雷 + 智力加成
        var finalMagicalDamage = fireDamageVal + iceDamageVal + lightingDamageVal
            + from.Int.GetValue() * Mathf.RoundToInt(from.attackMultiplier);

        // 扣除对方的魔抗和一些智力减伤
        finalMagicalDamage -= to.MagicResistance.GetValue() + (to.Int.GetValue() * 3);
        finalMagicalDamage = Mathf.Clamp(finalMagicalDamage, 1, int.MaxValue);

        // 不同元素显示不同特效
        if (isFireDamage)
        {
            flashFX.CreatHitFX(to.transform, 2);
        }
        else if (isIceDamage)
        {
            flashFX.CreatHitFX(to.transform, 3);
        }
        else if (isShockDamage)
        {
            flashFX.CreatHitFX(to.transform, 4);
        }
        else
        {
            flashFX.CreatHitFX(to.transform, 0);
        }

        return finalMagicalDamage;
    }

    public void ApplyAilments(bool ignite, bool chill, bool shock)
    {
        var canApplyIgnite = !IsIgnited && !IsChilled && !IsShocked;
        var canApplyChill = !IsIgnited && !IsChilled && !IsShocked;
        var canApplyShock = !IsIgnited && !IsChilled;

        if (ignite && canApplyIgnite)
        {
            IsIgnited = ignite;
            ignitedTimer = igniteDuration;
            flashFX.AlimentsFxFor(flashFX.igniteColor, igniteDuration);
            flashFX.igniteFx.Play();
            Debug.Log("结算为燃烧状态");
        }
        if (chill && canApplyChill)
        {
            IsChilled = chill;
            chilledTimer = chillDuration;
            flashFX.AlimentsFxFor(flashFX.chillColor, chillDuration);
            character.SlowBy(0.5f, chillDuration);
            flashFX.chillFx.Play();
            Debug.Log("结算为冻结状态");
        }
        if (shock && canApplyShock)
        {
            if (!IsShocked)
            {
                IsShocked = shock;
                shockedTimer = shockDuration;
                flashFX.AlimentsFxFor(flashFX.shockColor, shockDuration);
                flashFX.shockFx.Play();
                Debug.Log("结算为感电状态");
            }
            else
            {
                // 示例：二次感电可对附近敌人再劈一道雷
                var collider = Physics2D.OverlapCircleAll(transform.position, 25);
                var closeDis = Mathf.Infinity;
                Transform closeEnemy = null;

                foreach (var hit in collider)
                {
                    if (!hit.CompareTag("Enemy") || hit.transform == transform) continue;
                    var disToEnemy = Vector2.Distance(transform.position, hit.transform.position);
                    if (disToEnemy >= closeDis) continue;
                    closeDis = disToEnemy;
                    closeEnemy = hit.transform;
                }

                if (!closeEnemy || CompareTag("Player")) closeEnemy = transform;

                var pos = transform.position + new Vector3(0, 1);
                var parent = FxManager.Instance.fx.transform;
                var strike = Instantiate(thunderStrikePrefab, pos, Quaternion.identity, parent);

                if (!strike.TryGetComponent(out ThunderStrikeController c)) return;
                if (!closeEnemy.TryGetComponent(out Damageable damageable)) return;
                c.Setup(damageable);
            }
        }
    }

    public void SetupIgniteDamage(int damage)
    {
        igniteDamage = damage;
    }

    public virtual void IncreaseHealthBy(int amount)
    {
        currentHp += amount;
        if (currentHp > MaxHp.GetValue())
            currentHp = MaxHp.GetValue();
    }

    public virtual void IncreaseStatBy(int modifier, float duration, Stats statsToModify)
    {
        StartCoroutine(StatModifier(modifier, duration, statsToModify));
        IEnumerator StatModifier(int modifier, float duration, Stats statsToModify)
        {
            statsToModify.AddModifier(modifier);
            yield return new WaitForSeconds(duration);
            statsToModify.RemoveModifier(modifier);
        }
    }

    protected abstract void Die();

    public void KillEntity() => Die();

    // --------------------- 下面是对克隆体伤害的逻辑，略有删减保持一致性 ---------------------
    public virtual void CloneTakeDamage(GameObject from, bool isMagic = false,
        bool canEffect = true, float _multiplier = 1f,
        bool isFireDamage = false,
        bool isIceDamage = false,
        bool isShockDamage = false)
    {
        if (isDead) return;
        if (currentHp <= 0)
        {
            isDead = true;
            Die();
            currentHp = 0;
        }
        if (isInvincible)
        {
            Debug.Log($"{gameObject.name}处于无敌状态，忽略伤害");
            return;
        }

        var damageFrom = from.GetComponent<Damageable>();
        var damage = isMagic
            ? CalculateMagicDamage(damageFrom, this, isFireDamage, isIceDamage, isShockDamage)
            : CalculateDamage(damageFrom, this);

        if (_multiplier > 0)
        {
            damage = Mathf.RoundToInt(damage * _multiplier);
        }

        currentHp -= damage;

        if (from.CompareTag("Player") && canEffect)
        {
            Inventory.Instance.GetEquipmentByType(EquipmentType.Weapon)?.ExecuteItemEffect(from, gameObject);
        }
        if (CompareTag("Player") && ((float)currentHp / MaxHp.GetValue()) < 0.3)
        {
            Inventory.Instance.GetEquipmentByType(EquipmentType.Armor)?.ExecuteItemEffect(from, gameObject);
        }

        if (isMagic)
        {
            var fireDamageVal = damageFrom.FireDamage.GetValue();
            var iceDamageVal = damageFrom.IceDamage.GetValue();
            var lightingDamageVal = damageFrom.LightingDamage.GetValue();

            if (Mathf.Max(fireDamageVal, iceDamageVal, lightingDamageVal) <= 0) return;

            bool canApplyIgnite = isFireDamage && fireDamageVal > 0;
            bool canApplyChill = isIceDamage && iceDamageVal > 0;
            bool canApplyShock = isShockDamage && lightingDamageVal > 0;

            if (canApplyIgnite)
            {
                SetupIgniteDamage(Mathf.RoundToInt(fireDamageVal * 0.2f));
            }

            ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
        }

        if (currentHp > 0)
        {
            if (damage != 0)
            {
                OnTakeDamage?.Invoke(from, gameObject);
                if (triggerCriticalStrike)
                {
                    AttackSense.Instance.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
                    triggerCriticalStrike = false;
                }
            }
            else
            {
                Debug.Log($"{gameObject.name} 回避了来自 {from.name} 的攻击");
            }
        }
        else
        {
            isDead = true;
            Die();
        }
    }

    // --------------------- StatsOfType 返回对应的 Stats ---------------------
    public Stats StatsOfType(StatType type) => type switch
    {
        StatType.Strength => Str,
        StatType.Agility => Agi,
        StatType.Intelegence => Int,
        StatType.Vitality => Vit,
        StatType.Damage => Damage,
        StatType.CritChance => CritChance,
        StatType.CritPower => CritPower,
        StatType.Health => MaxHp,
        StatType.Armor => Armor,
        StatType.Evasion => Evasion,
        StatType.MagicRes => MagicResistance,
        StatType.FireDamage => FireDamage,
        StatType.IceDamage => IceDamage,
        StatType.LightingDamage => LightingDamage,
        _ => null
    };

    // --------------------- 新增：返回“最终伤害”方法 ---------------------
    public int GetFinalDamageValue()
    {
        // 示例：最终物理伤害 = (基础Damage + 力量) * 攻击倍率
        var baseDamage = Damage.GetValue() + Str.GetValue();
        var finalValue = Mathf.RoundToInt(baseDamage * attackMultiplier);
        return finalValue;
    }

    // --------------------- 新增：返回“火焰伤害”最终数值 ---------------------
    public int GetFinalFireDamageValue()
    {
        // 示例：火焰伤害 * 攻击倍率
        // 如果想包含智力或其他加成，请自行修改公式
        int baseFire = FireDamage.GetValue();
        return Mathf.RoundToInt(baseFire * attackMultiplier);
    }

    // --------------------- 新增：返回“冰霜伤害”最终数值 ---------------------
    public int GetFinalIceDamageValue()
    {
        int baseIce = IceDamage.GetValue();
        return Mathf.RoundToInt(baseIce * attackMultiplier);
    }

    // --------------------- 新增：返回“雷电伤害”最终数值 ---------------------
    public int GetFinalLightningDamageValue()
    {
        int baseLightning = LightingDamage.GetValue();
        return Mathf.RoundToInt(baseLightning * attackMultiplier);
    }
}
