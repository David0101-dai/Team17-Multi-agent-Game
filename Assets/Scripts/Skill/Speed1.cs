using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Speed1 : Skill
{
    [Header("Attack Speed Boost")]
    // �Ƿ�����˼���
    public bool attackSpeedBoostUnlocked;

    // �������еĲ�λ��UI��
    private SkillTreeSlot attackSpeedSlot;

    // �Ƿ��Ѿ�Ӧ�ù��ӳɣ���ֹ�ظ���
    private bool boostApplied = false;

    protected override void SkillFunction()
    {
        // �������δ�������Ѿ�Ӧ�ù��ӳɣ�����ִ��
        if (!attackSpeedBoostUnlocked || boostApplied) return;

        // ��ȡ Player �ű������������ϹҵĽű���
        Player p = player.GetComponent<Player>();
        if (p != null)
        {
            // �����ٴ� 1.0 ��ߵ� 1.5���ɸ��������Ϊ�����ֵ��
            p.attackSpeed = 1.5f;
            Debug.Log("Attack Speed Boost: x1.5");
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
        // �ȴ� UI �ͼ��ܲ�λ����
        while (UI.Instance == null || UI.Instance.speed1 == null)
        {
            yield return null;
        }

        attackSpeedSlot = UI.Instance.speed1;

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
        if (attackSpeedBoostUnlocked)
        {
            SkillFunction();
        }
    }

    // �����ܱ�ȡ��ʱ���Ƴ��ӳ�
    private void OnAttackSpeedSkillCancelled()
    {
        attackSpeedBoostUnlocked = false;
        if (boostApplied)
        {
            Player p = player.GetComponent<Player>();
            if (p != null)
            {
                // �ָ�����
                p.attackSpeed = 1f;
                Debug.Log("Attack Speed Boost Cancelled: revert to normal");
            }
            boostApplied = false;
        }
    }

    // ��鵱ǰ��λ�Ƿ��ѽ���
    protected override void CheckUnlock()
    {
        if (attackSpeedSlot != null)
        {
            attackSpeedBoostUnlocked = attackSpeedSlot.unlocked;
        }
       
    }

    // �����ť������˼���
    private void UnlockAttackSpeedBoost()
    {
        if (attackSpeedSlot.unlocked)
        {
            attackSpeedBoostUnlocked = true;
        }
    }
}
