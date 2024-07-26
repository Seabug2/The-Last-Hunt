using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

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
        slider = GetComponent<Slider>();
    }

    private void Start()
    {
        SliderSet();
    }
    void SliderSet()
    {
        float value = 0;
        switch (m_Group)
        {
            case AudioGroup.Master:
                value = PlayerPrefs.GetFloat("AudioLevel_Master");
                break;
            case AudioGroup.BGM:
                value = PlayerPrefs.GetFloat("AudioLevel_BGM");
                break;
            case AudioGroup.SFX:
                value = PlayerPrefs.GetFloat("AudioLevel_SFX");
                break;
        }
        slider.value = value;
    }

    public void VolumeControl(float value)
    {
        if (value <= -20)
        {
            value = -80f;
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
