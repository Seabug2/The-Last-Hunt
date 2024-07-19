using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Rhythm_ChapterManager : MonoBehaviour
{
    [SerializeField] private GameObject resultUI, introUI;
    [SerializeField] private Text maxT, hitT, missT, scoreT, RecordT;
    [SerializeField] private Slider scoreSlider;
    [SerializeField] private Image scoreBarFillImage, NextButton;
    [SerializeField] private Animator Hunter_ani;

    // 0. 싱글톤 적용
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

    // 메시지 출력 부분
    Tween currentTween = null;
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
        mySequence.Append(message.DOSizeDelta(new Vector2(1920, 126), 0.35f).SetEase(Ease.InOutQuad));

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



    public int Maxcount = 0;
    public int Hitcount = 0;
    public int Misscount = 0;
    public int percent = 0;
    public bool BGMisPlaying;

    private IEnumerator Start()
    {
        ShowMessage(text.text);
        yield return new WaitForSeconds(1f);
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
        percent = (100 * Maxcount + 70 * Hitcount) / (Maxcount + Hitcount + Misscount);
        resultUI.SetActive(true);

        maxT.text = Maxcount.ToString();
        hitT.text = Hitcount.ToString();
        missT.text = Misscount.ToString();
        scoreT.text = percent.ToString();
        scoreSlider.value = percent * 0.01f;
        RecordT.rectTransform.anchoredPosition = new Vector3(125, -150, 0);
        RecordT.rectTransform.eulerAngles = new Vector3(0, 0, 15);

        // 실패
        if (percent < 60)
        {
            NextButton.color = new Color(1, 1, 1, 0.25f);
            scoreBarFillImage.color = new Color(120, 0, 0);
            Hunter_ani.SetInteger("GameResult", -1);
            RecordT.text = "";
        }
        // 성공
        else
        {
            NextButton.color = new Color(1, 1, 1, 1);
            int BestScore = PlayerPrefs.GetInt("Ch2_BestScore");
            if (BestScore < percent)
            {
                PlayerPrefs.SetInt("Ch2_BestScore", percent);
                RecordT.text = "New Record!";
            }
            else if (percent < 100)
            {
                RecordT.rectTransform.anchoredPosition = new Vector3(125, -175, 0);
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
