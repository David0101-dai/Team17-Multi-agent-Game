using System.Collections.Generic;
using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{
    public static SkillTreeManager Instance;

    // �洢���м���
    private List<SkillTreeSlot> allSkills = new List<SkillTreeSlot>();



    private void Awake()
    {
        // ���׵���
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // �ѳ��������� SkillTreeSlot �������б�
        // ������������� SkillTreeSlot �� Awake ���ֶ�������ע�ᣩ
        allSkills.AddRange(FindObjectsOfType<SkillTreeSlot>());
    }

    /// <summary>
    /// �ж�ĳ�������Ƿ��С��ѽ����ĺ������ܡ��������������༶��������
    /// ����ҵ��κ����＼�ܴ��� unlocked ״̬���ͷ��� true��
    /// </summary>
    public bool HasUnlockedDescendants(SkillTreeSlot skill)
    {
        foreach (var candidate in allSkills)
        {
            // ��� candidate ��ǰ���б��������ǰ skill
            // ��ô candidate ���ǵ�ǰ skill ��ֱ�ӡ��Ӽ��ܡ�
            if (candidate.shouldBeUnlocked != null && candidate.shouldBeUnlocked.Length > 0)
            {
                // candidate.shouldBeUnlocked ��һ�� SkillTreeSlot[]���ж���ͷ�Ƿ���� skill
                bool isChild = false;
                foreach (var prerequisite in candidate.shouldBeUnlocked)
                {
                    if (prerequisite == skill)
                    {
                        isChild = true;
                        break;
                    }
                }

                // ��� candidate �ǵ�ǰ skill ��ֱ���Ӽ���
                if (isChild)
                {
                    // �������Ӽ����Ѿ���������˵����ǰ skill �С��ѽ����ĺ������ܡ�
                    if (candidate.unlocked)
                    {
                        return true;
                    }
                    // �������Ӽ���û����������������ܻ��к�������
                    // �ݹ��� candidate �ĺ��������Ƿ��н�����
                    if (HasUnlockedDescendants(candidate))
                    {
                        return true;
                    }
                }
            }
        }
        // û�ҵ��κ��ѽ����ĺ�������
        return false;
    }
}

