using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    public float moveSpeed = 1f;  // 控制移动速度
    public float moveDistance = 0.5f;  // 控制上下移动的幅度
    private Vector3 initialPosition;  // 初始位置

    public float destructionRadius = 5f;  // 控制销毁范围

    public GameObject BOSS;  // 这里是我们分配的BOSS物体

    private void Start()
    {
        initialPosition = transform.position;  // 记录门的初始位置
        EnsureUIInitialized();

        // 初始时判断BOSS是否已经死亡
        if (BOSS == null)
        {
            SetDoorActive(true);  // 如果BOSS已经死了，激活门
        }
        else
        {
            SetDoorActive(false);  // 如果BOSS还未死，禁用门
        }
    }

    private void Update()
    {
        // 每帧检查BOSS的状态
        if (BOSS == null)
        {
            SetDoorActive(true);  // 如果BOSS死亡，激活门
        }

        // 让门上下轻微地循环移动
        float newY = Mathf.Sin(Time.time * moveSpeed) * moveDistance;  // 通过正弦函数计算新的Y坐标
        transform.position = new Vector3(initialPosition.x, initialPosition.y + newY, initialPosition.z);
    }

    private void SetDoorActive(bool isActive)
    {
        // 设置门的激活状态
        gameObject.SetActive(isActive);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the door");
            StartCoroutine(InitializeUI());
            
            // 触碰时生成碰撞盒销毁敌人和Spike
            DestroyEnemiesAndSpikes();

            AudioManager.instance.PlayBGM(2); //播放赛博朋克BGM
        }
    }

    private void DestroyEnemiesAndSpikes()
    {
        // 使用OverlapCircle获取在一定半径内的所有物体
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, destructionRadius);

        foreach (var hit in hitEnemies)
        {
            if (hit.CompareTag("Enemy") || hit.CompareTag("Spike"))
            {
                // 销毁标签为 "Enemy" 或 "Spike" 的物体
                Destroy(hit.gameObject);
            }
        }
    }

    private void EnsureUIInitialized()
    {
        if (UI.Instance != null)
        {
            var ui = UI.Instance;  // 获取UI单例
            if (ui != null)
            {
                ui.gameObject.SetActive(true);  // 激活UI
                var fadeScreen = ui.getFadeScreen();
                if (fadeScreen != null)
                {
                    fadeScreen.gameObject.SetActive(true);  // 激活FadeScreen
                }
            }
        }
    }

    private IEnumerator InitializeUI()
    {
        // 等待UI单例初始化
        yield return new WaitUntil(() => UI.Instance != null);

        var ui = UI.Instance;  // 使用UI单例
        float waitTime = 0f;

        // 等待fadeScreen准备好（最多等待3秒）
        while (ui.getFadeScreen() == null && waitTime < 3f)
        {
            yield return new WaitForSeconds(0.1f);
            waitTime += 0.1f;
        }

        // 如果fadeScreen已经准备好，启动结束画面
        if (ui.getFadeScreen() != null)
        {
            ui.SwitchOnVictoryScreen();
        }
        else
        {
            Debug.LogWarning("FadeScreen is not initialized properly.");
        }
    }
}
