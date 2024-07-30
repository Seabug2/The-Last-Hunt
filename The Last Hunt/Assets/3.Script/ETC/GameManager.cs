using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    /// <summary>
    /// 여기에 클리어한 점수를 기록합니다.
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
    /// 타이틀 화면에서 챕터 선택으로 게임을 시작하면 false
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
    /// <param name="isHigher">true = 높은 기록을 저장 / false = 낮은 기록을 저장</param>
    /// <returns></returns>
    public bool IsNewHighScore(int chapterNumber, float newScore, bool isHigher = true)
    {
        //이 bool 함수를 호출한 시점에서 게임을 클리어 한 것으로 간주
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