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
            Destroy(gameObject); // 防止重复实例化
            return;
        }

        Instance = this; // 初始化实例
        DontDestroyOnLoad(gameObject); // 跨场景保留
    }
    void Start()
    {
        ResetTimer(); // 初始化时开始计时
    }

    void Update()
    {
        elapsedTime = Time.time - startTime;
        //Debug.Log($"已过去的时间：{elapsedTime:F2} 秒");
    }

    public void ResetTimer()
    {
        startTime = Time.time; // 重置为当前时间
    }
    public float getTime()
    {
        return elapsedTime;
    }
}
