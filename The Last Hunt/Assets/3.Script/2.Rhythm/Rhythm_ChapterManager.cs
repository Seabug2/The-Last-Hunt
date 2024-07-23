using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Rhythm_ChapterManager : MonoBehaviour
{
    public float Gamespeed = 1f;
    public int Maxcount = 0;
    public int Hitcount = 0;
    public int Misscount = 0;
    public int percent = 0;
    public bool BGMisPlaying, BGMisPausing;
    [SerializeField] private GameObject resultUI, introUI, Menu, NoteUI;
    [SerializeField] private Animator Hunter_ani;

    [SerializeField] private Sprite[] judgeImages;
    [SerializeField] private Image judgeImageAppear;

    [Header("결과")]
    [SerializeField] private Slider scoreSlider;
    [SerializeField] private Text maxT, hitT, missT, scoreT, RecordT;
    [SerializeField] private Image scoreBarFillImage, NextButton;
    [SerializeField] private Sprite ClearSP, FailSP;
    [SerializeField] private Image ClearImage;


    // 0. 싱글톤 적용
    public static Rhythm_ChapterManager instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else { Destroy(gameObject); }
    }

    // 메시지 출력 부분
    Tween currentTween = null;
    [Header("메시지")]
    // [SerializeField] private Image HeaderBoard;
    [SerializeField] private RectTransform message;
    [SerializeField] private Text text;
    public bool isNoMessage
    {
        get { return currentTween == null || !currentTween.IsActive(); }
    }
    public void ShowMessage(string _message, float _time = 1)
    {
        // 기존 애니메이션이 있으면 중단
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }

        text.text = _message;

        // 애니메이션 설정
        message.sizeDelta = new Vector2(1920, 0); // 시작 크기
        message.gameObject.SetActive(true);
        Sequence mySequence = DOTween.Sequence();

        // 0.5초 동안 크기를 키우기
        mySequence.Append(message.DOSizeDelta(new Vector2(1920, 126), 1f).SetEase(Ease.InOutQuad));

        // _time 동안 대기
        mySequence.AppendInterval(_time);

        // 0.5초 동안 크기를 다시 줄이기
        mySequence.Append(message.DOSizeDelta(new Vector2(1920, 0), 0.35f).SetEase(Ease.InOutQuad));

        // 애니메이션이 끝난 후 비활성화
        mySequence.OnComplete(() => message.gameObject.SetActive(false));

        // 현재 애니메이션 저장
        currentTween = mySequence;
    }

    public void MessageCut()
    {
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }
        message.gameObject.SetActive(false);
    }




    private IEnumerator Start()
    {
        ShowMessage(text.text);
        yield return new WaitForSeconds(2f);
        Rhythm_SoundManager.instance.PlayBGM("BGM");
        yield return new WaitForSeconds(2f);
        Menu.SetActive(true);
        NoteUI.SetActive(true);
        BGMisPlaying = true;
        BGMisPausing = false;
    }

    public void JudgeResult(int hit)
    {
        judgeImageAppear.sprite = judgeImages[hit];
        judgeImageAppear.GetComponent<Animator>().SetTrigger("IsJudged");
        if (hit > 0)
        {
            if (hit > 1) 
            {
                judgeImageAppear.color = Color.green;
                PlaySFX("MaxHit");
                Maxcount++;
            }
            else
            {
                judgeImageAppear.color = Color.yellow;
                PlaySFX("Hit");
                Hitcount++;
            }
        }
        else
        {
            judgeImageAppear.color = Color.red;
            PlaySFX("Miss");
            Misscount++;
        }
    }


    private void PlaySFX(string s)
    {
        Rhythm_SoundManager.instance.PlaySFX(s);
    }

    public void ResultAppear()
    {
        Menu.SetActive(false);
        NoteUI.SetActive(false);
        BGMisPlaying = false;
        percent = (100 * Maxcount + 60 * Hitcount) / (Maxcount + Hitcount + Misscount);
        resultUI.SetActive(true);

        maxT.text = Maxcount.ToString();
        hitT.text = Hitcount.ToString();
        missT.text = Misscount.ToString();
        scoreT.text = percent.ToString();
        scoreSlider.value = percent * 0.01f;
        RecordT.rectTransform.anchoredPosition = new Vector3(125, -200, 0);
        RecordT.rectTransform.eulerAngles = new Vector3(0, 0, 15);
        NextButton.color = new Color(1, 1, 1, 0.25f);

        // 실패
        if (percent < 60)
        {
            ClearImage.sprite = FailSP;
            scoreBarFillImage.color = new Color(120, 0, 0);
            Hunter_ani.SetInteger("GameResult", -1);
            RecordT.text = "";
        }
        // 성공
        else
        {
            // GameManager.instance.userDate.IsCleared[1] = true;
            // GameManager.instance.userDate.SaveJson();

            ClearImage.sprite = ClearSP;
            // if(GameManager.instance.IsStoryMode)
            {
                NextButton.color = new Color(1, 1, 1, 1);
            }
            int BestScore = PlayerPrefs.GetInt("Ch2_BestScore");
            if (BestScore < percent)
            {
                PlayerPrefs.SetInt("Ch2_BestScore", percent);
                RecordT.text = "New Record!";
            }
            else if (percent < 100)
            {
                RecordT.rectTransform.anchoredPosition = new Vector3(125, -200, 0);
                RecordT.rectTransform.eulerAngles = new Vector3(0, 0, 0);
                RecordT.text = $"Best: {BestScore}" ;
            }

            // 60~79
            if (percent < 80)
            {
                scoreBarFillImage.color = new Color(120, 120, 0);
                Hunter_ani.SetInteger("GameResult", 1);
            }
            // 80~100
            else
            {
                scoreBarFillImage.color = new Color(0, 120, 0);
                Hunter_ani.SetInteger("GameResult", 2);
                // 100
                if (percent > 99)
                {
                    RecordT.text = "Perfect!";
                }
            }
        }
    }
}
