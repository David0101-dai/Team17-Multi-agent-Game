using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

public abstract class Damageable : MonoBehaviour
{
    public int currentHp;

    [Header("Major Stats")]
    public Stats Str;  //力量， 可以提高伤害
    public Stats Agi;  //敏捷，敏捷可以提高闪避的概率
    public Stats Int;   //理智，可以提高魔法伤害
    public Stats Vit;   //防御， 可以减少收到的伤害

    [Header("offensive Stats")]
    public Stats Damage;  //攻击力
    public Stats CritChance;  //暴击率
    public Stats CritPower;  //暴击伤害
    private bool triggerCriticalStrike;

    [Header("Defensive Stats")]
    public Stats MaxHp;  //最大生命值
    public Stats Armor;  //装甲
    public Stats Evasion; //闪避
    public Stats MagicResistance; //抗魔

    [Header("Magic Stats")]
    public Stats FireDamage;
    public Stats IceDamage;
    public Stats LightingDamage;

    public bool IsIgnited; //持续伤害
    public bool IsChilled; //减少护甲
    public bool IsShocked; //减少闪避率

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

    public event Action<GameObject, GameObject> OnTakeDamage;
    private FlashFX flashFX;
    private Character character;

    protected virtual void Start()
    {
        currentHp = MaxHp.GetValue();
        CritPower.SetDefaultValue(150);
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

        if (ignitedTimer < 0)
        {
            IsIgnited = false;
        }
        if (chilledTimer < 0)
        {
            IsChilled = false;
        }
        if (shockedTimer < 0)
        {
            IsShocked = false;
        }
        if (IsIgnited && igniteDamageTimer < 0)
        {
            // 计算持续的火焰伤害
            currentHp -= igniteDamage;
            if (currentHp <= 0)
            {
                Die();
            }
            igniteDamageTimer = igniteDamageCooldown;  // 重置计时器
        }
    }

    public void MakeVulnerableFor(float _duration) => StartCoroutine(VulnerableCorutine(_duration));
    

    private IEnumerator VulnerableCorutine(float _duration)
    {
        isVulnerable = true;

        yield return new WaitForSeconds(_duration);

        isVulnerable = false;
    }

    public virtual void TakeDamage(GameObject from, bool isMagic = false, bool canEffect = true, bool isFromSwordSkill = false)
    {
        if (isDead) return;

        if (currentHp <= 0)
        {
            isDead = true;
            Die();
            currentHp = 0;  // Ensure the HP doesn't go below 0
        }

        // 如果当前对象是玩家，并且处于无敌状态，则忽略伤害
        var playerComponent = GetComponent<Player>();
        if (playerComponent != null && playerComponent.isInvincible)
        {
            Debug.Log($"{gameObject.name}处于无敌状态，忽略伤害");
            return;
        }

        var damageFrom = from.GetComponent<Damageable>();

        var damage = isMagic ? CalculateMagicDamage(damageFrom, this) : CalculateDamage(damageFrom, this);

        if (isFromSwordSkill)
        {
            damage = Mathf.RoundToInt(damage * 0.3f);
        }

        if (isVulnerable)
        {
            damage = Mathf.RoundToInt(damage * 2.0f);
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
            var fireDamage = damageFrom.FireDamage.GetValue();
            var iceDamage = damageFrom.IceDamage.GetValue();
            var lightingDamage = damageFrom.LightingDamage.GetValue();

            if (Mathf.Max(fireDamage, iceDamage, lightingDamage) <= 0) return;

            bool canApplyIgnite = fireDamage > iceDamage && fireDamage > lightingDamage;
            bool canApplyChill = iceDamage > lightingDamage && iceDamage > fireDamage;
            bool canApplyShock = lightingDamage > fireDamage && lightingDamage > iceDamage;

            while (!canApplyIgnite && !canApplyChill && !canApplyShock)
            {
                if (UnityEngine.Random.value < 0.3f && fireDamage > 0)
                {
                    canApplyIgnite = true;
                    break;
                }
                if (UnityEngine.Random.value < 0.45f && iceDamage > 0)
                {
                    canApplyChill = true;
                    break;
                }
                if (UnityEngine.Random.value < 0.7f && lightingDamage > 0)
                {
                    canApplyShock = true;
                    break;
                }
            }

            if (canApplyIgnite)
            {
                SetupIgniteDamage(Mathf.RoundToInt(fireDamage * 0.2f));
            }

            ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
        }

        if (currentHp > 0)
        {
            if (damage != 0)
            {
               // Debug.Log($"{gameObject.name} 受到了来自 {from.name} 的 {damage} 点伤害");
                OnTakeDamage?.Invoke(from, gameObject);     
                if(triggerCriticalStrike){
                    //AttackSense.Instance.HitPause(0.1f);
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

        if (from.IsShocked) finalEvasion += 20; //如果被雷劈可以减少地方的闪避率

        if (UnityEngine.Random.Range(0, 100) <= finalEvasion) return 0;

        var finalDamage = from.Damage.GetValue() + from.Str.GetValue() - to.Vit.GetValue();

        if (from.IsChilled)
        {
            finalDamage -= Mathf.RoundToInt(from.Armor.GetValue() * 0.8f);
        }
        else
        {
            finalDamage -= to.Armor.GetValue();
        }

        var finalCritical = from.CritChance.GetValue() + from.Agi.GetValue();
        if (UnityEngine.Random.Range(0, 100) <= finalCritical)
        {
            var finalCritPower = (CritPower.GetValue() + Str.GetValue()) * 0.01f;
            finalDamage = Mathf.RoundToInt(finalDamage * finalCritPower);
            //Debug.Log("触发了暴击");
            triggerCriticalStrike = true;
        }

        finalDamage = Mathf.Clamp(finalDamage, 1, int.MaxValue);
        return finalDamage;
    }

    private int CalculateMagicDamage(Damageable from, Damageable to)
    {
        var fireDamage = from.FireDamage.GetValue();
        var iceDamage = from.IceDamage.GetValue();
        var lightingDamage = from.LightingDamage.GetValue();

        var finalMagicalDamage = fireDamage + iceDamage + lightingDamage + from.Int.GetValue();


        finalMagicalDamage -= to.MagicResistance.GetValue() + (to.Int.GetValue() * 3);

        finalMagicalDamage = Mathf.Clamp(finalMagicalDamage, 1, int.MaxValue);

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
            Debug.Log("结算为燃烧状态");
        }
        if (chill && canApplyChill)
        {
            IsChilled = chill;
            chilledTimer = chillDuration;
            flashFX.AlimentsFxFor(flashFX.chillColor, chillDuration);
            character.SlowBy(0.5f, chillDuration);
            Debug.Log("结算为冻结状态");
        }
        if (shock && canApplyShock)
        {
            if (!IsShocked)
            {
                IsShocked = shock;
                shockedTimer = shockDuration;
                flashFX.AlimentsFxFor(flashFX.shockColor, shockDuration);
                Debug.Log("结算为感电状态");
            }
            else
            {
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

    public virtual void CloneTakeDamage(GameObject from, bool isMagic = false, bool canEffect = true, float _multiplier = 1f)
    {

        if (isDead) return;

        if (currentHp <= 0)
        {
            isDead = true;
            Die();
            currentHp = 0;  // Ensure the HP doesn't go below 0
        }


        // 如果当前对象是玩家，并且处于无敌状态，则忽略伤害
        var playerComponent = GetComponent<Player>();
        if (playerComponent != null && playerComponent.isInvincible)
        {
            Debug.Log($"{gameObject.name}处于无敌状态，忽略伤害");
            return;
        }

        var damageFrom = from.GetComponent<Damageable>();

        var damage = isMagic ? CalculateMagicDamage(damageFrom, this) : CalculateDamage(damageFrom, this);

        if (_multiplier > 0 )
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
            var fireDamage = damageFrom.FireDamage.GetValue();
            var iceDamage = damageFrom.IceDamage.GetValue();
            var lightingDamage = damageFrom.LightingDamage.GetValue();

            if (Mathf.Max(fireDamage, iceDamage, lightingDamage) <= 0) return;

            bool canApplyIgnite = fireDamage > iceDamage && fireDamage > lightingDamage;
            bool canApplyChill = iceDamage > lightingDamage && iceDamage > fireDamage;
            bool canApplyShock = lightingDamage > fireDamage && lightingDamage > iceDamage;

            while (!canApplyIgnite && !canApplyChill && !canApplyShock)
            {
                if (UnityEngine.Random.value < 0.3f && fireDamage > 0)
                {
                    canApplyIgnite = true;
                    break;
                }
                if (UnityEngine.Random.value < 0.45f && iceDamage > 0)
                {
                    canApplyChill = true;
                    break;
                }
                if (UnityEngine.Random.value < 0.7f && lightingDamage > 0)
                {
                    canApplyShock = true;
                    break;
                }
            }

            if (canApplyIgnite)
            {
                SetupIgniteDamage(Mathf.RoundToInt(fireDamage * 0.2f));
            }

            ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
        }

        if (currentHp > 0)
        {
            if (damage != 0)
            {
                // Debug.Log($"{gameObject.name} 受到了来自 {from.name} 的 {damage} 点伤害");
                OnTakeDamage?.Invoke(from, gameObject);
                AttackSense.Instance.HitPause(0.1f);
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
}
