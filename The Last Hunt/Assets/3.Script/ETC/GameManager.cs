using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            isStoryMode = true;

            path = Path.Combine(Application.dataPath, "UserData.json");
            LoadJson();
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
    public UserData userDate;

    void LoadJson()
    {
        if (!File.Exists(path))
        {
            userDate = new UserData();
            return;
        }
        string json = File.ReadAllText(path);
        userDate = JsonUtility.FromJson<UserData>(json);
    }

    /// <summary>
    /// 최고 점수 이상을 기록하면 최고 점수로 기록합니다.
    /// </summary>
    /// <param name="chapterNumber">게임 순서대로 0, 1, 2, 3</param>
    /// <param name="newScore">현재 게임 결과</param>
    /// <returns></returns>
    public bool IsNewHighScore(int chapterNumber, float newScore)
    {
        if (userDate.score[chapterNumber] <= newScore)
        {
            userDate.score[chapterNumber] = newScore;
            SaveJson();
            return true;
        }
        return false;
    }

    void SaveJson()
    {
        string file = JsonUtility.ToJson(userDate, true);
        File.WriteAllText(path, file);
    }
}

[System.Serializable]
public class UserData
{
    public bool[] IsCleared = { false, false, false, false };
    public float[] score = { 0f, 0f, 0f, 0f };
}