using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            isStoryMode = true;
            //path = Path.Combine(Application.persistentDataPath, jsonName);
            path = Path.Combine(Application.dataPath, "UserData.json");
            print(path);
            RoadJson();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    /// <summary>
    /// 타이틀 화면에서 챕터 선택으로 게임을 시작하면 false
    /// </summary>
    bool isStoryMode;
    public bool IsStoryMode => isStoryMode;

    string path;
    public UserDate userDate;

    void RoadJson()
    {
        string json = File.ReadAllText(path);
        userDate = JsonUtility.FromJson<UserDate>(json);
    }

    [ContextMenu("Save Json")]
    void SaveJson()
    {
        string file = JsonUtility.ToJson(userDate);
        File.WriteAllText(Application.persistentDataPath, file);
    }
}

[System.Serializable]
public class UserDate
{
    public bool[] IsCleared = { false, false, false, false};
    public float[] score = { 0f,0f,0f,0f};
}