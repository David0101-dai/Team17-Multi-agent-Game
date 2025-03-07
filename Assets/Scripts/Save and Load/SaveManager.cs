using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    [SerializeField] private string fileName;
    [SerializeField] private bool encryptData;
    
    private GameData gameData;
    private List<ISaveManager> saveManagers;
    private FileDataHandler fileDataHandler;

    [ContextMenu("Delete Save Data")]
    public void DeleteSaveData()
    {
        // 确保 fileDataHandler 已经初始化
        if (fileDataHandler == null)
        {
            fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        }

        fileDataHandler.DeleteData();
        gameData = null;
        Debug.Log("Game data deleted.");
    }
private void Awake()
{
    // 确保 SaveManager 只有一个实例，并且不会在场景切换时销毁
    if (instance == null)
    {
        instance = this;
        transform.SetParent(null);  // 确保它不再是某个子物体，成为场景的根对象
        DontDestroyOnLoad(gameObject);  // 保证 SaveManager 不会在场景切换时销毁
    }
    else
    {
        Destroy(gameObject);  // 销毁重复的实例，确保只有一个实例
    }

    fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
    saveManagers = new List<ISaveManager>();
}

private IEnumerator Start()
{
    yield return null; // 延迟一帧，确保其他组件初始化完成
    // 确保文件数据处理器已经初始化
    if (fileDataHandler == null)
    {
        Debug.LogError("fileDataHandler initialization failed.");
        yield break;  // 如果初始化失败，则退出
    }

    // 立即加载游戏数据
    LoadGame();
    
}




    public void NewGame()
    {
        gameData = new GameData();
        SaveGame();
    }

public void LoadGame()
{
    gameData = fileDataHandler.LoadData();

    // 如果没有加载到数据，则创建新的游戏数据
    if (gameData == null)
    {
        UnityEngine.Debug.Log("Game data is null, creating new game data.");
        NewGame();
    }
    else
    {
        Debug.Log("Game data loaded successfully.");
    }

    // 加载游戏数据后，通知所有的 ISaveManager 实现类
    foreach (ISaveManager saveManager in saveManagers)
    {
        saveManager.LoadData(gameData);
    }
}


    public void SaveGame()
    {
        // 保存游戏数据
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }
        fileDataHandler.SaveData(gameData);
    }

        private void OnApplicationQuit()
        {
            // 应用退出时保存游戏数据
            SaveGame();
        }

    // 查找并注册场景中的所有实现 ISaveManager 接口的组件
    private List<ISaveManager> FindSaveManagers()
    {
        IEnumerable<ISaveManager> foundManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();
        return new List<ISaveManager>(foundManagers);
    }

    // 注册 SaveManager 以便其他组件可以监听数据加载
public void RegisterSaveManager(ISaveManager saveManager)
{
    if (saveManagers == null)
        saveManagers = new List<ISaveManager>();

    if (!saveManagers.Contains(saveManager))
    {
        saveManagers.Add(saveManager);

        // 如果数据已经加载，则立即将数据传递给新注册的组件
        if (gameData != null)
        {
            saveManager.LoadData(gameData);
        }
        else
        {
            Debug.Log("Game data is not loaded yet.");
        }
    }
}


    public GameData CurrentGameData()
    {
        return gameData; 
    }

    public bool HasSaveData()
    {
        if (fileDataHandler == null)
        {
            Debug.LogError("fileDataHandler is not initialized yet.");
            return false; // 还没有初始化时返回 false
        }

        var data = fileDataHandler.LoadData();
        return data != null;
    }




}
