using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Defence3 : Skill
{
    [Header("Defense")]
    public bool defence__new;   // ���������������ʾ�����Ƿ����
    private SkillTreeSlot defence;       // SkillTreeSlot �ű�����

    private bool defense1 = false; // ��¼�Ƿ��Ѿ�����Ҽӹ�����
    private const int armorBonus = 5;    // Ҫ�ӵĻ�����ֵ

    protected override void Start()
    {
        base.Start();
        StartCoroutine(InitializeSkillButtons());
    }

    /// <summary>
    /// ÿ֡��⣬��������ѽ�������ִ�м��ܹ���
    /// </summary>
    protected override void Update()
    {
        if (defence__new)
        {
            SkillFunction();
        }
    }

    /// <summary>
    /// ����ļ��ܹ��ܣ���������� +5 ���ף�ֻ��һ�Σ�
    /// </summary>
    protected override void SkillFunction()
    {
        // �������û���������Ѿ��ӹ����ף��Ͳ���ִ��
        if (!defence__new || defense1) return;

        Damageable dmg = player.GetComponent<Damageable>();
        if (dmg != null)
        {
            dmg.Armor.AddModifier(armorBonus);

            Debug.Log($"��Ϊ������� {armorBonus} �㻤��");

            defense1 = true;
        }

    }

    /// <summary>
    /// ��ʼ�����ܰ�ť�������ؽ���״̬
    /// </summary>
    private IEnumerator InitializeSkillButtons()
    {
        // �ȴ� UI �Ͷ�Ӧ��ť��ʼ��
        while (UI.Instance == null || UI.Instance.defence3 == null)
        {
            yield return null;
        }

        // �� UI �ϵ� SkillTreeSlot
        defence = UI.Instance.defence3;

        if (defence != null)
        {
            // �����ť -> ��������
            defence.GetComponent<Button>().onClick.AddListener(UnlockAttackBoost);
            // �������ܱ�ȡ��
            defence.OnSkillCancelled += OnAttackBoostSkillCancelled;
        }


        // �ȴ��浵��������ʼ��
        while (SaveManager.instance == null || SaveManager.instance.CurrentGameData() == null)
        {
            yield return null;
        }

        // �Ӵ浵���ص�ǰ����״̬
        if (SaveManager.instance.CurrentGameData() != null)
        {
            defence.LoadData(SaveManager.instance.CurrentGameData());
        }


        // �ȴ�һ֡��ȷ���������
        yield return new WaitForEndOfFrame();
        CheckUnlock();
    }

    /// <summary>
    /// �����ܱ�ȡ��ʱ���Ƴ����׼ӳɣ����ñ��
    /// </summary>
    private void OnAttackBoostSkillCancelled()
    {
        defence__new = false;
        Damageable dmg = player.GetComponent<Damageable>();

        if (dmg != null)
        {
            dmg.Armor.RemoveModifier(armorBonus);

            Debug.Log($"���Ƴ� {armorBonus} �㻤�׼ӳ�");
        }
        defense1 = false;
    }

    /// <summary>
    /// ��⼼���Ƿ����
    /// </summary>
    protected override void CheckUnlock()
    {
        if (defence != null)
        {
            defence__new = defence.unlocked;
        }

    }

    /// <summary>
    /// �������ܣ������ťʱ���ã�
    /// </summary>
    private void UnlockAttackBoost()
    {
        if (defence.unlocked)
        {
            defence__new = true;
            // ��������ֱ�Ӽӻ��ף������� SkillFunction() ��ִ��
        }
    }
}
