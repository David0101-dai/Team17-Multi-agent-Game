using UnityEngine;

public class MagicManager : MonoBehaviour
{
    public static MagicManager Instance { get; private set; }

    public float maxMagic = 100f;
    public float currentMagic;
    public float regenRate = 3f; // 每秒回复的魔法值

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
        {
            Instance = this;
            currentMagic = maxMagic;
        }
    }

    private void Update()
    {
        RegenerateMagic();
    }

    private void RegenerateMagic()
    {
        // 使用 Time.deltaTime 确保回复效果与帧率无关
        currentMagic = Mathf.Clamp(currentMagic + regenRate * Time.deltaTime, 0, maxMagic);
    }

    // 尝试消耗一定量的魔法，如果足够则扣除并返回true，否则返回false
    public bool ConsumeMagic(float amount)
    {
        if (currentMagic >= amount)
        {
            currentMagic -= amount;
            return true;
        }
        return false;
    }
}
