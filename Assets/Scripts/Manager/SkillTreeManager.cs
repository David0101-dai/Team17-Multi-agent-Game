using System.Collections.Generic;
using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{
    public static SkillTreeManager Instance;


    private List<SkillTreeSlot> allSkills = new List<SkillTreeSlot>();



    private void Awake()
    {

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        allSkills.AddRange(FindObjectsOfType<SkillTreeSlot>());
    }

    public bool HasUnlockedDescendants(SkillTreeSlot skill)
    {
        foreach (var candidate in allSkills)
        {
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
                    if (candidate.unlocked)
                    {
                        return true;
                    }
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

