using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour,ISaveManager
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private CheckPoint[] checkPoints;
    [SerializeField] private string closetCheckPointId;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);  // 销毁重复的实例，确保只有一个实例
        }
        // 如果 SaveManager 尚未创建，则等待一帧后再注册
        if (SaveManager.instance != null && SaveManager.instance.CurrentGameData() != null)
        {
            SaveManager.instance.RegisterSaveManager(this);
            Debug.Log("Inventory registered in SaveManager (Awake)");
        }
        else
        {
            // 如果 SaveManager 在此时还没初始化，使用协程等待
            StartCoroutine(RegisterWhenReady());
        }
    }

    private IEnumerator RegisterWhenReady()
    {   // 等待 SaveManager 初始化完成
        while (SaveManager.instance == null || SaveManager.instance.CurrentGameData() == null)
        {
            yield return null;  // 每帧检查，直到 SaveManager 和 gameData 完全加载
        }// 当 SaveManager 准备好时，注册
        SaveManager.instance.RegisterSaveManager(this);
        //Debug.Log("Inventory registered in SaveManager (Coroutine)");
    }

    public void ReStartGame(){
        SaveManager.instance.SaveGame();
        Scene  scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void ReturnHome(){
        SaveManager.instance.SaveGame();
        SceneManager.LoadScene("MainMenu");
    }

    void Start()
    {
        AudioManager.instance.PlayBGM(0);
    }
    public void SaveData(ref GameData _data)
    {
        _data.checkPointKeys.Clear(); // 清空旧数据
        _data.checkPointValues.Clear();
        if(_data.closetCheckPointId == null){
        _data.closetCheckPointId = FindClosestCheckPoint().checkPointId;
        }
        
        foreach (CheckPoint checkPoint in checkPoints)
        {
            _data.checkPointKeys.Add(checkPoint.checkPointId);  // 添加检查点ID
            _data.checkPointValues.Add(checkPoint.activated);   // 添加激活状态
        }
    }
    public void LoadData(GameData _data)
    {
        Debug.Log("存档中的最近点" + _data.closetCheckPointId);
        // 等待所有检查点的 Animator 初始化完成
        StartCoroutine(WaitForAnimatorsThenActivate(_data));
    }

    private IEnumerator WaitForAnimatorsThenActivate(GameData _data)
    {
        // 等待直到所有检查点的 Animator 都初始化完成
        bool allInitialized = false;
        while (!allInitialized)
        {
            allInitialized = true;
            foreach (CheckPoint checkPoint in checkPoints)
            {
                if (checkPoint.anim == null)
                {
                    allInitialized = false;
                    break;
                }
            }

            if (!allInitialized)
                yield return null;  // 每帧检查一次，直到所有 Animator 都初始化完成
        }
        ActivateCheckPoints(_data);
    }

    private void PlacePlayerAtClosestCheckpoint()
    {
        foreach (CheckPoint checkPoint in checkPoints)
        {
            if (checkPoint != null && checkPoint.checkPointId == closetCheckPointId) // 添加null检查
            {
                PlayerManager.Instance.player.transform.position = checkPoint.transform.position;
                break;  // 找到正确的检查点后就可以跳出循环
            }
        }
    }
    private CheckPoint FindClosestCheckPoint(){
    float closestDistance = Mathf.Infinity;
    CheckPoint closetCheckPoint = null;

    foreach(var checkPoint in checkPoints){
        if (checkPoint == null) continue;  // 防止访问被销毁的对象
        float distanceToCheckPoint = Vector2.Distance(PlayerManager.Instance.player.transform.position, checkPoint.transform.position);
        if(distanceToCheckPoint < closestDistance && checkPoint.activated == true){
            closestDistance = distanceToCheckPoint;
            closetCheckPoint = checkPoint;
        }
    }
    return closetCheckPoint;
    }
    private void ActivateCheckPoints(GameData _data)
    {
        // 激活存档中的所有检查点
        for (int i = 0; i < _data.checkPointKeys.Count; i++)
        {
            string checkPointId = _data.checkPointKeys[i];
            bool isActivated = _data.checkPointValues[i];

            foreach (CheckPoint checkPoint in checkPoints)
            {
                if (checkPoint == null) continue;  // 防止访问已销毁的检查点
                if (checkPoint.checkPointId == checkPointId && isActivated)
                {
                    checkPoint.ActivateCheckPoint();  // 激活检查点
                }
            }
        }

        // 确保只在这里设置 closetCheckPointId
        closetCheckPointId = _data.closetCheckPointId;
        Debug.Log("加载出来的最近点: " + closetCheckPointId);  // 确认加载时使用的是正确的检查点ID

        PlacePlayerAtClosestCheckpoint();  // 将玩家传送到存档中的最近检查点
    }
}
