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
    /// Ÿ��Ʋ ȭ�鿡�� é�� �������� ������ �����ϸ� false
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
    /// �ְ� ���� �̻��� ����ϸ� �ְ� ������ ����մϴ�.
    /// </summary>
    /// <param name="chapterNumber">���� ������� 0, 1, 2, 3</param>
    /// <param name="newScore">���� ���� ���</param>
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