using UnityEngine;

public class EnemyDamageable : Damageable
{
    private ItemDrop drop;
    private Enemy enemy;

    [Header("Level details")]
    [SerializeField] private EnemyLevel level;


    [Range(0f, 1f)]
    private float percentageModifier = .4f;


    protected override void Start()
    {
        ApplyLevelModifier();
        base.Start();
        drop = GetComponent<ItemDrop>();
        enemy = GetComponent<Enemy>();

    }

    private void ApplyLevelModifier()
    {
        Modify(Str);
        Modify(Agi);
        Modify(Int);
        Modify(Vit);

        Modify(Damage);
        Modify(CritChance);
        Modify(CritPower);

        Modify(MaxHp);
        Modify(Armor);
        Modify(Evasion);
        Modify(MagicResistance);

        Modify(FireDamage);
        Modify(IceDamage);
        Modify(LightingDamage);
    }

    private void Modify(Stats stat)
    {
        for (int i = 1; i < level.GetLevel(); i++)
        {
            float modifier = stat.GetValue() * percentageModifier;
            stat.AddModifier(Mathf.RoundToInt(modifier));

        }
    }
    protected override void Die()
    {
        if (!TryGetComponent(out Enemy enemy)) return;
        drop = GetComponent<ItemDrop>();
        drop.GenerateDrop();
        enemy.Die();
    }
}