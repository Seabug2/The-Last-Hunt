using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    /// <summary>
    /// ���⿡ Ŭ������ ������ ����մϴ�.
    /// </summary>
    [HideInInspector] public string[] currentGameScore = new string[4];
    public AudioMixer audioMixer;

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

    private void Start()
    {
        MixerSetting();
    }

    void MixerSetting()
    {
        audioMixer.SetFloat("Master", PlayerPrefs.GetFloat("AudioLevel_Master", 0));
        audioMixer.SetFloat("BGM", PlayerPrefs.GetFloat("AudioLevel_BGM", 0));
        audioMixer.SetFloat("SFX", PlayerPrefs.GetFloat("AudioLevel_SFX", 0));
    }

    /// <summary>
    /// Ÿ��Ʋ ȭ�鿡�� é�� �������� ������ �����ϸ� false
    /// </summary>
    bool isStoryMode;
    public bool IsStoryMode => isStoryMode;

    public void SetPlayingMode(bool on)
    {
        isStoryMode = on;
    }

    string path;
    public UserData userData;

    void LoadJson()
    {
        if (!File.Exists(path))
        {
            userData = new UserData();
            return;
        }
        string json = File.ReadAllText(path);
        userData = JsonUtility.FromJson<UserData>(json);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="chapterNumber"></param>
    /// <param name="newScore"></param>
    /// <param name="isHigher">true = ���� ����� ���� / false = ���� ����� ����</param>
    /// <returns></returns>
    public bool IsNewHighScore(int chapterNumber, float newScore, bool isHigher = true)
    {
        //�� bool �Լ��� ȣ���� �������� ������ Ŭ���� �� ������ ����
        userData.IsCleared[chapterNumber] = true;

        if (isHigher)
        {
            if (userData.score[chapterNumber] <= newScore)
            {
                userData.score[chapterNumber] = newScore;
                SaveJson();
                return true;
            }
        }
        else
        {
            if (userData.score[chapterNumber] <= 0 || userData.score[chapterNumber] >= newScore)
            {
                userData.score[chapterNumber] = newScore;
                SaveJson();
                return true;
            }
        }

        return false;
    }

    public void SaveJson()
    {
        string file = JsonUtility.ToJson(userData, true);
        File.WriteAllText(path, file);
    }
}

[System.Serializable]
public class UserData
{
    public bool[] IsCleared = { false, false, false, false };
    public float[] score = { 0f, 0f, 0f, 0f };
}