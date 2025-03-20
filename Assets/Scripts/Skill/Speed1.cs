using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Speed1 : Skill
{
    [Header("Attack Speed Boost")]
    // �Ƿ�����˼���
    public bool attackSpeedBoost1Unlocked;

    // �������еĲ�λ��UI��
    private SkillTreeSlot attackSpeedSlot;

    

    protected override void SkillFunction()
    {
        // �������δ�������Ѿ�Ӧ�ù��ӳɣ�����ִ��
        if (!attackSpeedBoost1Unlocked) return;

        // ��ȡ Player �ű������������ϹҵĽű���
        Player p = player.GetComponent<Player>();
        if (p != null)
        {
            if (p.attackSpeed < 1.5f)
            {
                p.attackSpeed = 1.5f;
                Debug.Log("Attack Speed Boost: x1.5");
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
        if (attackSpeedBoost1Unlocked)
        {
            SkillFunction();
        }
    }

    // �����ܱ�ȡ��ʱ���Ƴ��ӳ�
    private void OnAttackSpeedSkillCancelled()
    {
        attackSpeedBoost1Unlocked = false;
        Player p = player.GetComponent<Player>();
        p.attackSpeed = 1f;
    }

    // ��鵱ǰ��λ�Ƿ��ѽ���
    protected override void CheckUnlock()
    {
        if (attackSpeedSlot != null)
        {
            attackSpeedBoost1Unlocked = attackSpeedSlot.unlocked;
        }
       
    }

    // �����ť������˼���
    private void UnlockAttackSpeedBoost()
    {
        if (attackSpeedSlot.unlocked)
        {
            attackSpeedBoost1Unlocked = true;
        }
    }
}
