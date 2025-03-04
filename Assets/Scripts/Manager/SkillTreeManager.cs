using System.Collections.Generic;
using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{
    public static SkillTreeManager Instance;

    // 存储所有技能
    private List<SkillTreeSlot> allSkills = new List<SkillTreeSlot>();



    private void Awake()
    {
        // 简易单例
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // 把场景中所有 SkillTreeSlot 都加入列表
        // （或者你可以在 SkillTreeSlot 的 Awake 里手动向这里注册）
        allSkills.AddRange(FindObjectsOfType<SkillTreeSlot>());
    }

    /// <summary>
    /// 判断某个技能是否有“已解锁的后续技能”依赖它（包括多级后续）。
    /// 如果找到任何子孙技能处于 unlocked 状态，就返回 true。
    /// </summary>
    public bool HasUnlockedDescendants(SkillTreeSlot skill)
    {
        foreach (var candidate in allSkills)
        {
            // 如果 candidate 的前置列表里包含当前 skill
            // 那么 candidate 就是当前 skill 的直接“子技能”
            if (candidate.shouldBeUnlocked != null && candidate.shouldBeUnlocked.Length > 0)
            {
                // candidate.shouldBeUnlocked 是一个 SkillTreeSlot[]，判断里头是否包含 skill
                bool isChild = false;
                foreach (var prerequisite in candidate.shouldBeUnlocked)
                {
                    if (prerequisite == skill)
                    {
                        isChild = true;
                        break;
                    }
                }

                // 如果 candidate 是当前 skill 的直接子技能
                if (isChild)
                {
                    // 如果这个子技能已经解锁，就说明当前 skill 有“已解锁的后续技能”
                    if (candidate.unlocked)
                    {
                        return true;
                    }
                    // 如果这个子技能没解锁，但它下面可能还有后续技能
                    // 递归检测 candidate 的后续技能是否有解锁的
                    if (HasUnlockedDescendants(candidate))
                    {
                        return true;
                    }
                }
            }
        }
        // 没找到任何已解锁的后续技能
        return false;
    }
}

