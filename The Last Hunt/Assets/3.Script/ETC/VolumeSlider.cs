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
        slider.minValue = -20;
        float value;
        switch (m_Group)
        {
            case AudioGroup.Master:
                value = PlayerPrefs.GetFloat("AudioLevel_Master");
                audioMixer.SetFloat("Master", value);
                break;
            case AudioGroup.BGM:
                value = PlayerPrefs.GetFloat("AudioLevel_BGM");
                audioMixer.SetFloat("BGM", value);
                break;
            case AudioGroup.SFX:
                value = PlayerPrefs.GetFloat("AudioLevel_SFX");
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
        
        if (value <= -20)
        {
            value = -80;
        }

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
}
