using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Attack2 : Skill
{
    [Header("Attack Boost")]
    public bool attackBoostUnlocked;
    private SkillTreeSlot attack2;


    protected override void SkillFunction()
    {

        if (!attackBoostUnlocked) return;

        Damageable dmg = player.GetComponent<Damageable>();
        if (dmg != null)
        {

            if (dmg.attackMultiplier < 1.5f)
            {
                dmg.attackMultiplier = 1.5f;
                Debug.Log("AttackBoostSkill111");
            }
        }
        else
        {
            Debug.LogWarning("AttackBoostSkill111Damageable      ");
        }
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(InitializeSkillButtons());
    }



    private IEnumerator InitializeSkillButtons()
    {

        while (UI.Instance == null || UI.Instance.attack2 == null)
        {
            yield return null;
        }


        attack2 = UI.Instance.attack2;

        if (attack2 != null)
        {

            attack2.GetComponent<Button>().onClick.AddListener(UnlockAttackBoost);

            attack2.OnSkillCancelled += OnAttackBoostSkillCancelled;
        }
        else
        {
            Debug.LogError("AttackBoostSkill  attackBoostUnlockedButto1111");
        }


        while (SaveManager.instance == null || SaveManager.instance.CurrentGameData() == null)
        {
            yield return null;
        }


        if (SaveManager.instance.CurrentGameData() != null)
        {
            attack2.LoadData(SaveManager.instance.CurrentGameData());
        }
        else
        {
            Debug.LogError("AttackBoostSkill  CurrentGameData 111 ");
        }


        yield return new WaitForEndOfFrame();
        CheckUnlock();
    }



    protected override void Update()
    {

        if (attackBoostUnlocked)
        {
            SkillFunction();
        }
    }

    private void OnAttackBoostSkillCancelled()
    {
        attackBoostUnlocked = false;
        Damageable dmg = player.GetComponent<Damageable>();
        dmg.attackMultiplier = 1.25f;

    }


    protected override void CheckUnlock()
    {
        if (attack2 != null)
        {
            attackBoostUnlocked = attack2.unlocked;
        }
        else
        {
            Debug.LogWarning("AttackBoostSkill  attackBoostUnlockedButton111");
        }
    }


    private void UnlockAttackBoost()
    {
        if (attack2.unlocked)
        {
            attackBoostUnlocked = true;


        }
    }
}