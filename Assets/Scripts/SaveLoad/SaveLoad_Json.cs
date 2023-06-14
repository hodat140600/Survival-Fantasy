using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveLoad_Json<T>
{
    public T data;
    private string _fileName;
    private string _filePath => Path.Combine(Application.persistentDataPath, _fileName) + ".json";
    private bool _isFileExists => File.Exists(_filePath);

    /// <summary>
    /// Read file and load type data to this class
    /// </summary>
    /// <returns>True if read file success, false if the file didn't exists</returns>
    public bool LoadData(string fileName)
    {
        _fileName = fileName;
        string playerData = PlayerPrefs.GetString(fileName);
        if (String.IsNullOrEmpty(playerData)) return false;
        data = JsonUtility.FromJson<T>(PlayerPrefs.GetString(fileName));
        
        return data != null;
        //if (_isFileExists)
        //{
        //    data = JsonUtility.FromJson<T>(File.ReadAllText(_filePath));
        //}

        //return _isFileExists;
    }

    public bool SaveData(string fileName)
    {
        _fileName = fileName;
        PlayerPrefs.SetString(fileName, JsonUtility.ToJson(this.data));
        //try
        //{
        //    var data = JsonUtility.ToJson(this.data);
        //    File.WriteAllText(_filePath, data);

        //    return true;
        //}
        //catch (System.Exception)
        //{
        //    return false;
        //}
        return true;
    }
}