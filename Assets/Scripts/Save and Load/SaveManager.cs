using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class SaveManager : MonoBehaviour
{
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
            //Load();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        saveManagers = FindSaveManagers();
        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
        //Save();
    }

    public void LoadGame()
    {
        gameData = fileDataHandler.LoadData();

        if(this.gameData == null)
        {
            Debug.Log("Game data is null");
            NewGame();
        }

        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }

        Debug.Log("Loaded currency: " + gameData.currency);
    }

    public void SaveGame()
    {
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData); // 确保这里正确填充了 gameData
        }

        fileDataHandler.SaveData(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISaveManager> FindSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();
        
        return new List<ISaveManager>(saveManagers);
    }
}
