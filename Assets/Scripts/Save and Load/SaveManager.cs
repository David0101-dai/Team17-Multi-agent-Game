using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

using System.IO;

public class SaveManager : MonoBehaviour
{
    public static event Action OnSaveDataLoaded;
    [SerializeField] private string fileName;
    public static SaveManager instance;
    private GameData gameData;
    private List<ISaveManager> saveManagers;
    private FileDataHandler fileDataHandler;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            saveManagers = new List<ISaveManager>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator Start()
    {
        yield return null; // 延迟一帧，确保其他组件初始化完成
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = fileDataHandler.LoadData();

        if (gameData == null)
        {
            Debug.Log("Game data is null, creating new game data.");
            NewGame();
        }

        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }

        OnSaveDataLoaded?.Invoke();
    }

    public void SaveGame()
    {
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }
        fileDataHandler.SaveData(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    // 查找场景中所有实现 ISaveManager 接口的组件
    private List<ISaveManager> FindSaveManagers()
    {
        IEnumerable<ISaveManager> foundManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();
        return new List<ISaveManager>(foundManagers);
    }

    public void RegisterSaveManager(ISaveManager saveManager)
    {
        if (saveManagers == null)
            saveManagers = new List<ISaveManager>();

        if (!saveManagers.Contains(saveManager))
        {
            saveManagers.Add(saveManager);
            // 如果游戏数据已经加载，则立即更新新注册的组件
            if (gameData != null)
            {
                saveManager.LoadData(gameData);
            }
        }
    }

        public GameData CurrentGameData
    {
        get { return gameData; }
    }

}
