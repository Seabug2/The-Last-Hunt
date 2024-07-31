using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run_HunterVoice : MonoBehaviour
{
    [SerializeField] AudioClip[] vioceClips;
    Dictionary<string, AudioClip[]> d_Audios = new Dictionary<string, AudioClip[]>();

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        d_Audios.Add("기합소리", vioceClips);
    }

    public void PlayAudio(string keyName)
    {
        AudioClip[] clips = d_Audios[keyName];
        if (clips.Length == 0) return;
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        audioSource.PlayOneShot(clip);
    }
}
