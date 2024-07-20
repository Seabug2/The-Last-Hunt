using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using static UnityEngine.Rendering.DebugUI;

public class VolumeSlider : MonoBehaviour
{
    public enum AudioGroup
    {
        Master,
        BGM,
        SFX
    }

    public AudioGroup m_Group;

    public AudioMixer audioMixer;
    Slider slider;

    private void Awake()
    {
        SliderSet();
    }

    void SliderSet()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = 0;
        slider.minValue = -80;
        float value;
        switch (m_Group)
        {
            case AudioGroup.Master:
                value = PlayerPrefs.GetFloat("AudioLevel_Master",0);
                audioMixer.SetFloat("Master", value);
                break;
            case AudioGroup.BGM:
                value = PlayerPrefs.GetFloat("AudioLevel_BGM", 0);
                audioMixer.SetFloat("BGM", value);
                break;
            case AudioGroup.SFX:
                value = PlayerPrefs.GetFloat("AudioLevel_SFX", 0);
                audioMixer.SetFloat("SFX", value);
                break;
            default:
                value = 0;
                break;
        }
        slider.value = value;
    }

    public void VolumeControl(float value)
    {
        value = Mathf.FloorToInt(value);

        switch (m_Group)
        {
            case AudioGroup.Master:
                audioMixer.SetFloat("Master", value);
                PlayerPrefs.SetFloat("AudioLevel_Master", value);
                break;
            case AudioGroup.BGM:
                audioMixer.SetFloat("BGM", value);
                PlayerPrefs.SetFloat("AudioLevel_BGM", value);
                break;
            case AudioGroup.SFX:
                audioMixer.SetFloat("SFX", value);
                PlayerPrefs.SetFloat("AudioLevel_SFX", value);
                break;
        }
    }

    //public void MasterControl(float value)
    //{
    //    audioMixer.SetFloat("Master", value);
    //}
    //public void BGMControl(float value)
    //{
    //    audioMixer.SetFloat("BGM", value);
    //}
    //public void SFXControl(float value)
    //{
    //    audioMixer.SetFloat("SFX", value);
    //}

    //public void VolumeControl(string group)
    //{
    //    audioMixer.SetFloat(group, slider.value);
    //}
}
