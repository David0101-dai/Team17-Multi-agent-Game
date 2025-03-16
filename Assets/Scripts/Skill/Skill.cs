using UnityEngine;
using System.Collections;


public abstract class Skill : MonoBehaviour
{
    public float cooldown;
    protected Player player;
    [SerializeField] public float cooldownTimer;

        protected virtual void OnEnable()
    {
        StartCoroutine(DelayedCheckUnlock());
    }

    private IEnumerator DelayedCheckUnlock()
    {
        // 等待一帧，确保 SaveManager 加载数据后 SkillTreeSlot 的状态已更新
        yield return null;
        CheckUnlock();
    }

    protected virtual void Start()
    {
        player = PlayerManager.Instance.player.GetComponent<Player>();
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    protected virtual void CheckUnlock(){}
    public bool CanUseSkill()
    {
        if (cooldownTimer >= 0) return false;
        SkillFunction();
        // 使用玩家的冷却倍率来计算技能冷却
        cooldownTimer = cooldown * player.cooldownMultiplier;
        return true;
    }
    public bool DelayCanUseSkill()
    {
        if (cooldownTimer >= 0) return false;
        return true;
    }
    protected abstract void SkillFunction();
    protected virtual Transform FindClosestEnemy(Transform detectTransform, float radius)
    {
        var collider = Physics2D.OverlapCircleAll(detectTransform.position, radius);
        var closeDis = Mathf.Infinity;
        Transform closeEnemy = null;
        foreach (var hit in collider)
        {
            if (!hit.CompareTag("Enemy")) continue;
            var disToEnemy = Vector2.Distance(detectTransform.position, hit.transform.position);
            if (disToEnemy >= closeDis) continue;
            closeDis = disToEnemy;
            closeEnemy = hit.transform;
        }
        return closeEnemy;
    }
}
