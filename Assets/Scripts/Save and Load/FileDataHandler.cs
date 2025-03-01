using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class FileDataHandler : MonoBehaviour
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string _dataDirPath, string _dataFileName)
    {
        dataDirPath = _dataDirPath;
        dataFileName = _dataFileName;
    }

    public void SaveData(GameData _data)
    {
        string dataPath = Path.Combine(dataDirPath, dataFileName);
        try{
           Directory.CreateDirectory(Path.GetDirectoryName(dataPath));

           string dataToSave = JsonUtility.ToJson(_data,true);

           using(FileStream fs = new FileStream(dataPath, FileMode.Create))
           {
               using(StreamWriter writer = new StreamWriter(fs))
               {
                   writer.Write(dataToSave);
               }
           }
        }
        catch(Exception e)
        {
            Debug.Log("Error on trying to save data to file " + dataPath + "\n" + e.Message);
        }
        string jsonData = JsonUtility.ToJson(_data);
        File.WriteAllText(dataPath, jsonData);
    }

    public GameData LoadData()
    {
        string dataPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if(File.Exists(dataPath))
        {
            try
            {
                string dataToLoad = "";
                using(FileStream fs = new FileStream(dataPath, FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(fs))
                    {
                        dataToLoad = reader.ReadToEnd();
                    } 
                }
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            
            }
            catch(Exception e)
            {
                Debug.Log("Error on trying to load data from file " + dataPath + "\n" + e.Message);
            }
        }
        return loadedData;
    }
}
