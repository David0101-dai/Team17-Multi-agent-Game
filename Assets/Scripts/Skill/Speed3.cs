using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Speed3 : Skill
{
    [Header("Attack Speed Boost")]
    // �Ƿ�����˼���
    public bool attackSpeedBoost3Unlocked;

    // �������еĲ�λ��UI��
    private SkillTreeSlot attackSpeedSlot;

    

    protected override void SkillFunction()
    {
        // �������δ�������Ѿ�Ӧ�ù��ӳɣ�����ִ��
        if (!attackSpeedBoost3Unlocked) return;

        // ��ȡ Player �ű������������ϹҵĽű���
        Player p = player.GetComponent<Player>();
        if (p != null)
        {
            if (p.attackSpeed < 3f) { 
                p.attackSpeed = 3f;
                Debug.Log("Attack Speed Boost: x3");
            }
        }

    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(InitializeSkillButtons());
    }

    private IEnumerator InitializeSkillButtons()
    {
        // �ȴ� UI �ͼ��ܲ�λ����
        while (UI.Instance == null || UI.Instance.speed3 == null)
        {
            yield return null;
        }

        attackSpeedSlot = UI.Instance.speed3;

        // ����õ��˲�λ���Ͱ󶨰�ť����¼�
        if (attackSpeedSlot != null)
        {
            attackSpeedSlot.GetComponent<Button>().onClick.AddListener(UnlockAttackSpeedBoost);
            attackSpeedSlot.OnSkillCancelled += OnAttackSpeedSkillCancelled;
        }


        // �ȴ��浵���ݼ���
        while (SaveManager.instance == null || SaveManager.instance.CurrentGameData() == null)
        {
            yield return null;
        }

        if (SaveManager.instance.CurrentGameData() != null)
        {
            // ��ȡ�浵���ָ�����״̬
            attackSpeedSlot.LoadData(SaveManager.instance.CurrentGameData());
        }


        yield return new WaitForEndOfFrame();
        CheckUnlock();
    }

    protected override void Update()
    {
        // ��������ѽ���������ִ�м����߼�
        if (attackSpeedBoost3Unlocked)
        {
            SkillFunction();
        }
    }

    // �����ܱ�ȡ��ʱ���Ƴ��ӳ�
    private void OnAttackSpeedSkillCancelled()
    {
        attackSpeedBoost3Unlocked = false;
        Player p = player.GetComponent<Player>();
        p.attackSpeed = 2f;
   
    }

    // ��鵱ǰ��λ�Ƿ��ѽ���
    protected override void CheckUnlock()
    {
        if (attackSpeedSlot != null)
        {
            attackSpeedBoost3Unlocked = attackSpeedSlot.unlocked;
        }

    }

    // �����ť������˼���
    private void UnlockAttackSpeedBoost()
    {
        if (attackSpeedSlot.unlocked)
        {
            attackSpeedBoost3Unlocked = true;
        }
    }
}
