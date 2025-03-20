using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Attack1 : Skill
{
    [Header("Attack Boost")]
    public bool attackBoostUnlocked_1;
    private SkillTreeSlot attack1;

    
    protected override void SkillFunction()
    {

        if (!attackBoostUnlocked_1) return;
 
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
  
        while (UI.Instance == null || UI.Instance.attack1 == null)
        {
            yield return null;
        }

 
        attack1 = UI.Instance.attack1;

        if (attack1 != null)
        {
            
            attack1.GetComponent<Button>().onClick.AddListener(UnlockAttackBoost);
            
            attack1.OnSkillCancelled += OnAttackBoostSkillCancelled;
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
            attack1.LoadData(SaveManager.instance.CurrentGameData());
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

        if (attackBoostUnlocked_1)
        {
            SkillFunction();
        }
    }

    private void OnAttackBoostSkillCancelled()
    {
        attackBoostUnlocked_1 = false;
        Damageable dmg = player.GetComponent<Damageable>();
        dmg.attackMultiplier = 1f;

    }


    protected override void CheckUnlock()
    {
        if (attack1 != null)
        {
            attackBoostUnlocked_1 = attack1.unlocked;
        }
        else
        {
            Debug.LogWarning("AttackBoostSkill  attackBoostUnlockedButton111");
        }
    }


    private void UnlockAttackBoost()
    {
        if (attack1.unlocked)
        {
            attackBoostUnlocked_1 = true;
            

        }
    }
}