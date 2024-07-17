using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rhythm_ChapterManager : MonoBehaviour
{
    [SerializeField] private GameObject resultUI, introUI;
    [SerializeField] private Text maxT, hitT, missT, scoreT, RecordT;
    [SerializeField] private Slider scoreSlider;
    [SerializeField] private Image scoreBarFillImage, NextButton;
    [SerializeField] private Animator Hunter_ani;

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

    public int Maxcount = 0;
    public int Hitcount = 0;
    public int Misscount = 0;
    public bool BGMisPlaying;

    private void Start()
    {
        introUI.SetActive(true);
        StartCoroutine("Intro_co");
    }
    private IEnumerator Intro_co()
    {
        yield return introUI.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        Rhythm_SoundManager.instance.PlayBGM("BGM");
        BGMisPlaying = true;
    }

    public void CountAdd(int hit)
    {
        if (hit > 1) Maxcount++;
        else if (hit > 0) Hitcount++;
        else Misscount++;
    }

    public void ResultAppear()
    {
        BGMisPlaying = false;
        int percent = (100 * Maxcount + 70 * Hitcount) / (Maxcount + Hitcount + Misscount);
        resultUI.SetActive(true);

        maxT.text = Maxcount.ToString();
        hitT.text = Hitcount.ToString();
        missT.text = Misscount.ToString();
        scoreT.text = percent.ToString();
        scoreSlider.value = percent * 0.01f;

        if (percent < 60)
        {
            NextButton.color = new Color(255, 255, 255, 64);
            scoreBarFillImage.color = new Color(120, 0, 0);
            Hunter_ani.SetInteger("GameResult", -1);
            RecordT.text = "";
        }
        else if (percent > 99)
        {
            PlayerPrefs.SetInt("Ch2_BestScore", 100);
            RecordT.text = "Perfect!";
        }
        else
        {
            int BestScore = PlayerPrefs.GetInt("Ch2_BestScore");
            
            if (percent < 80)
            {
                scoreBarFillImage.color = new Color(120, 120, 0);
                Hunter_ani.SetInteger("GameResult", 1);
            }
            else
            {
                scoreBarFillImage.color = new Color(0, 120, 0);
                Hunter_ani.SetInteger("GameResult", 2);
            }
        }
    }
}
