using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rhythm_ChapterManager : MonoBehaviour
{
    [SerializeField] private GameObject result_obj;
    [SerializeField] private Text hitT, missT, scoreT;
    [SerializeField] private Slider scoreSlider;
    // 0. ½Ì±ÛÅæ Àû¿ë
    public static Rhythm_ChapterManager instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }
    }

    public int Hitcount = 0;
    public int Misscount = 0;
    public bool BGMisPlaying;

    private void Start()
    {
        Rhythm_SoundManager.instance.PlayBGM("BGM");
        BGMisPlaying = true;
    }

    public void CountAdd(bool isHit)
    {
        if (isHit) Hitcount++;
        else Misscount++;
    }

    public void ResultAppear()
    {
        int percent = 100 * Hitcount / (Hitcount + Misscount);
        result_obj.SetActive(true);
        hitT.text = Hitcount.ToString();
        missT.text = Misscount.ToString();
        scoreT.text = percent.ToString();
        scoreSlider.value = percent * 0.01f;
    }
}
