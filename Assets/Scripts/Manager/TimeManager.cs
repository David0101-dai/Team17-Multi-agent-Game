using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }
    private float startTime;
    public float elapsedTime;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // ��ֹ�ظ�ʵ����
            return;
        }

        Instance = this; // ��ʼ��ʵ��
        DontDestroyOnLoad(gameObject); // �糡������
    }
    void Start()
    {
        ResetTimer(); // ��ʼ��ʱ��ʼ��ʱ
    }

    void Update()
    {
        elapsedTime = Time.time - startTime;
        //Debug.Log($"�ѹ�ȥ��ʱ�䣺{elapsedTime:F2} ��");
    }

    public void ResetTimer()
    {
        startTime = Time.time; // ����Ϊ��ǰʱ��
    }
    public float getTime()
    {
        return elapsedTime;
    }
}
