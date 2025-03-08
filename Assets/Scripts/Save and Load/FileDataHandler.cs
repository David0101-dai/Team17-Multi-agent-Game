using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Newtonsoft.Json;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";
    private bool encryptData = false;
    private string codeW = "alexdev";

    public FileDataHandler(string _dataDirPath, string _dataFileName,bool _encryptData)
    {
        dataDirPath = _dataDirPath;
        dataFileName = _dataFileName;
        encryptData = _encryptData;
    }

    public void SaveData(GameData _data)
    {
        string dataPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(dataPath));
            // 在序列化前调用 OnBeforeSerialize
            _data.OnBeforeSerialize();
            // 使用 Newtonsoft.Json 序列化
            string dataToSave = JsonConvert.SerializeObject(_data, Formatting.Indented);
           
           if (encryptData)
            {
                dataToSave = EncryptDecrypt(dataToSave);
            }

            using (FileStream fs = new FileStream(dataPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(dataToSave);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error on trying to save data to file " + dataPath + "\n" + e.Message);
        }
    }

    public GameData LoadData()
    {
        string dataPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if (File.Exists(dataPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream fs = new FileStream(dataPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(fs))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if (encryptData)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }
                // 使用 Newtonsoft.Json 反序列化
                loadedData = JsonConvert.DeserializeObject<GameData>(dataToLoad);
                // 在反序列化后调用 OnAfterDeserialize
                loadedData.OnAfterDeserialize();
            }
            catch (Exception e)
            {
                Debug.Log("Error on trying to load data from file " + dataPath + "\n" + e.Message);
            }
        }
        return loadedData;
    }

    public void DeleteData()
    {
        string dataPath = Path.Combine(dataDirPath, dataFileName);
        if (File.Exists(dataPath))
        {
            try
            {
                File.Delete(dataPath);
            }
            catch (Exception e)
            {
                Debug.Log("Error on trying to delete data file " + dataPath + "\n" + e.Message);
            }
        }
    }

    private string EncryptDecrypt(string _data)
    {
        string result = "";
        for (int i = 0; i < _data.Length; i++)
        {
            result += (char)(_data[i] ^ codeW[i % codeW.Length]);
        }
        return result;
    }
}
