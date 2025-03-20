using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Health2 : Skill
{
    [Header("Health Boost")]

    public bool healthBoostUnlocked;

    private SkillTreeSlot healthBoostSlot;

    private bool boostApplied = false;

    protected override void SkillFunction()
    {
        if (!healthBoostUnlocked || boostApplied) return;

        Damageable dmg = player.GetComponent<Damageable>();
        if (dmg != null)
        {

            dmg.MaxHp.AddModifier(100);

            dmg.currentHp += 100;

            boostApplied = true;
        }

    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(InitializeSkillButtons());
    }

    private IEnumerator InitializeSkillButtons()
    {

        while (UI.Instance == null || UI.Instance.health2 == null)
        {
            yield return null;
        }

        healthBoostSlot = UI.Instance.health2;

        if (healthBoostSlot != null)
        {
            healthBoostSlot.GetComponent<Button>().onClick.AddListener(UnlockHealthBoost);
            healthBoostSlot.OnSkillCancelled += OnHealthBoostSkillCancelled;
        }


        while (SaveManager.instance == null || SaveManager.instance.CurrentGameData() == null)
        {
            yield return null;
        }

        if (SaveManager.instance.CurrentGameData() != null)
        {
            healthBoostSlot.LoadData(SaveManager.instance.CurrentGameData());
        }


        yield return new WaitForEndOfFrame();
        CheckUnlock();
    }

    protected override void Update()
    {

        if (healthBoostUnlocked)
        {
            SkillFunction();
        }
    }


    private void OnHealthBoostSkillCancelled()
    {
        healthBoostUnlocked = false;
        if (boostApplied)
        {
            Damageable dmg = player.GetComponent<Damageable>();
            if (dmg != null)
            {
                dmg.MaxHp.RemoveModifier(100);

                dmg.currentHp = Mathf.Min(dmg.currentHp, dmg.MaxHp.GetValue());

            }
            boostApplied = false;
        }
    }

    protected override void CheckUnlock()
    {
        if (healthBoostSlot != null)
        {
            healthBoostUnlocked = healthBoostSlot.unlocked;
        }
    }

    private void UnlockHealthBoost()
    {
        if (healthBoostSlot.unlocked)
        {
            healthBoostUnlocked = true;
        }
    }
}