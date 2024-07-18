using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class Rhythm_SoundManager : MonoBehaviour
{
    // 0. �̱��� ����
    public static Rhythm_SoundManager instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }
        BGMPlayer = transform.GetChild(0).GetComponent<AudioSource>();
        SFXPlayer = transform.GetChild(1).GetComponents<AudioSource>();
    }

    // 1. ���� ����
    [Header("Audio Clip")]
    [SerializeField] private Sound[] BGM;
    [SerializeField] private Sound[] SFX;
    [Header("AudioSource")]
    [SerializeField] private AudioSource BGMPlayer;
    [SerializeField] private AudioSource[] SFXPlayer;

    // 2. �޼��� ����
    public void PlayBGM(string name)
    {
        foreach (Sound s in BGM)
        {
            if (s.name.Equals(name))
            {
                BGMPlayer.clip = s.clip;
                BGMPlayer.Play();
                return;
            }
        }
        Debug.Log($"PlayBGM {name} ����");
    }

    public bool BGMisPlaying()
    {
        return BGMPlayer.isPlaying;
    }

    public void StopBGM()
    {
        BGMPlayer.Stop();
    }

    public void PlaySFX(string name)
    {
        foreach (Sound s in SFX)
        {
            if (s.name.Equals(name))
            {
                for (int i = 0; i < SFXPlayer.Length; i++)
                {
                    if (!SFXPlayer[i].isPlaying)
                    {
                        SFXPlayer[i].clip = s.clip;
                        SFXPlayer[i].Play();
                        return;
                    }
                }
                Debug.Log("��� SFX�÷��̾ ��� ����");
                return;
            }
        }
        Debug.Log($"PlaySFX {name} ����");
    }
}
