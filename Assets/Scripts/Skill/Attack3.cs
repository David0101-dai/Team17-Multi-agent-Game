using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Attack3 : Skill
{
    [Header("Attack Boost")]
    public bool attackBoostUnlocked;
    private SkillTreeSlot attack3;


    protected override void SkillFunction()
    {

        if (!attackBoostUnlocked) return;

        Damageable dmg = player.GetComponent<Damageable>();
        if (dmg != null)
        {

            if (dmg.attackMultiplier < 2f)
            {
                dmg.attackMultiplier = 2f;
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

        while (UI.Instance == null || UI.Instance.attack3 == null)
        {
            yield return null;
        }


        attack3 = UI.Instance.attack3;

        if (attack3 != null)
        {

            attack3.GetComponent<Button>().onClick.AddListener(UnlockAttackBoost);

            attack3.OnSkillCancelled += OnAttackBoostSkillCancelled;
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
            attack3.LoadData(SaveManager.instance.CurrentGameData());
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
        dmg.attackMultiplier = 1.5f;

    }


    protected override void CheckUnlock()
    {
        if (attack3 != null)
        {
            attackBoostUnlocked = attack3.unlocked;
        }
        else
        {
            Debug.LogWarning("AttackBoostSkill  attackBoostUnlockedButton111");
        }
    }


    private void UnlockAttackBoost()
    {
        if (attack3.unlocked)
        {
            attackBoostUnlocked = true;


        }
    }
}