using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rhythm_ChapterManager : MonoBehaviour
{
    [SerializeField] private GameObject result_obj;
    [SerializeField] private Text maxT, hitT, missT, scoreT;
    [SerializeField] private Slider scoreSlider;
    [SerializeField] private Image scoreBarFillImage;
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
        result_obj.SetActive(true);

        maxT.text = Maxcount.ToString();
        hitT.text = Hitcount.ToString();
        missT.text = Misscount.ToString();
        scoreT.text = percent.ToString();
        scoreSlider.value = percent * 0.01f;

        if(percent < 60)
        {
            scoreBarFillImage.color = new Color(120, 0, 0);
            Hunter_ani.SetInteger("GameResult", -1);
        }
        else if(percent < 80)
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
