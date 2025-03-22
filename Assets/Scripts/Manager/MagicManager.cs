using UnityEngine;

public class MagicManager : MonoBehaviour
{
    public static MagicManager Instance { get; private set; }

    public float maxMagic = 100f;
    public float currentMagic;
    public float regenRate = 4f; // ÿ��ظ���ħ��ֵ

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
        // ʹ�� Time.deltaTime ȷ���ظ�Ч����֡���޹�
        currentMagic = Mathf.Clamp(currentMagic + regenRate * Time.deltaTime, 0, maxMagic);
    }

    // ��������һ������ħ��������㹻��۳�������true�����򷵻�false
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
