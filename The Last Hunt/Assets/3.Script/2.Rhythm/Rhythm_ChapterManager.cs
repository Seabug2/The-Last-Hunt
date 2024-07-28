using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using DG.Tweening;

public class Rhythm_ChapterManager : MonoBehaviour
{
    public float GameSpeed = 1.0f;
    public int Wave = 1;
    public int Maxcount = 0;
    public int Hitcount = 0;
    public int Misscount = 0;
    public float percent = 0;
    private int notskip = 1;
    public bool MainBGMisPlaying, isPausing;
    Animator judgeAnimation;

    [SerializeField] private GameObject resultUI, introUI, Menu, NoteUI, BestScore, IntroBlackbg;
    [SerializeField] private Animator Hunter_ani, Lap_ani;
    [SerializeField] private Text SkipMessage, LapCount, NewLapCount;

    [SerializeField] private Sprite[] judgeImages;
    [SerializeField] private Image judgeImageAppear;

    [Header("결과")]
    [SerializeField] private Image ClearImage, NextButtonImage;
    [SerializeField] private Sprite ClearSP, FailSP;
    [SerializeField] private Text maxT, hitT, missT, scoreT, RecordT;
    [SerializeField] private GameObject NextButton, MainButton;

    [Header("카메라")]
    [SerializeField] private CinemachineBrain brainCam;
    [SerializeField] private CinemachineVirtualCamera initVcam;
    [SerializeField] private CinemachineVirtualCamera mainVcam;
    [SerializeField] private CinemachineBlendListCamera mainBcam;
    [SerializeField] private CinemachineVirtualCamera resultVcam;



    // 0. 싱글톤 적용
    public static Rhythm_ChapterManager instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else { Destroy(gameObject); }
        judgeAnimation = judgeImageAppear.GetComponent<Animator>();
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
        introUI.SetActive(true);
        yield return new WaitForSeconds(1.5f * notskip);
        mainVcam.Priority = initVcam.Priority + 1;
        yield return new WaitForSeconds(2.5f * notskip);
        if (notskip > 0) PlaySFX("ChapterIntro");
        ShowMessage(text.text, 1.5f);
        yield return new WaitForSeconds(3f * notskip);
        if (notskip > 0) StartBGM();
    }

    public void SkipIntro()
    {
        notskip = 0;
        Rhythm_SoundManager.instance.StopSFX();
        StartBGM();
    }

    private void StartBGM()
    {
        SkipMessage.text = "";
        IntroBlackbg.SetActive(false);
        introUI.SetActive(false);
        Hunter_ani.SetTrigger("StartBGM");
        Rhythm_SoundManager.instance.PlayBGM("BGM");
        mainBcam.Priority = mainVcam.Priority + 1;
        Menu.SetActive(true);
        NoteUI.SetActive(true);
        MainBGMisPlaying = true;
        isPausing = false;
    }
    public void JudgeResult(int hit)
    {
        judgeImageAppear.sprite = judgeImages[hit];
        switch(hit)
        {
            case 2:
                judgeImageAppear.color = Color.green;
                PlaySFX("MaxHit");
                Maxcount++;
                break;
            case 1:
                judgeImageAppear.color = Color.yellow;
                PlaySFX("Hit");
                Hitcount++;
                break;
            default:
                judgeImageAppear.color = Color.red;
                PlaySFX("Miss");
                Misscount++;
                break;
        }
        judgeAnimation.SetTrigger("IsJudged");
    }


    private void PlaySFX(string s)
    {
        Rhythm_SoundManager.instance.PlaySFX(s);
    }

    public void DebugButton()
    {
        Rhythm_SoundManager.instance.BGMPlayer.Stop();
        NextWave();
    }

    public void NextWave()
    {
        MainBGMisPlaying = false;
        if (Wave.Equals(3))
        {
            Invoke("ResultAppear", 1.5f);
        }
        else
        {
            Wave++;
            Rhythm_SoundManager.instance.PlayBGM("SpeedUp");
            Lap_ani.SetTrigger("LapChange");
            GameSpeed += 0.15f;
            introUI.SetActive(true);
            ShowMessage("스피드 업!", 1.5f);
            Invoke("BGMAgain", 4.5f / GameSpeed);
        }
    }

    private void BGMAgain()
    {
        MainBGMisPlaying = true;
        Rhythm_SoundManager.instance.BGMPlayer.pitch = GameSpeed;
        Rhythm_SoundManager.instance.PlayBGM("BGM");
        LapCount.text = Wave.ToString();
        NewLapCount.text = (Wave + 1).ToString();
    }

    public void ResultAppear()
    {
        if (GameManager.instance.IsStoryMode) MainButton.SetActive(false);
        else NextButton.SetActive(false);

        resultVcam.Priority = mainBcam.Priority + 1;
        Menu.SetActive(false);
        NoteUI.SetActive(false);
        MainBGMisPlaying = false;
        percent = (100f * Maxcount + 60 * Hitcount) / (Maxcount + Hitcount + Misscount);
        resultUI.SetActive(true);

        maxT.text = Maxcount.ToString();
        hitT.text = Hitcount.ToString();
        missT.text = Misscount.ToString();
        scoreT.text = percent.ToString("0.00");

        // 실패
        if (percent < 60)
        {
            PlaySFX("ChapterFail");
            ClearImage.sprite = FailSP;
            NextButton.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            NextButtonImage.color = new Color(1, 1, 1, 0.3f);
            NextButton.GetComponent<Button>().interactable = false;
            Hunter_ani.SetInteger("GameResult", -1);
            BestScore.SetActive(false);
        }
        // 성공
        else
        {
            GameManager.instance.currentGameScore[1] = percent;
            if (GameManager.instance.IsNewHighScore(1, percent))
            {
                RecordT.color = Color.yellow;
            }
            RecordT.text = GameManager.instance.userData.score[1].ToString("0.00");
            PlaySFX("ChapterClear");
            ClearImage.sprite = ClearSP;
            Hunter_ani.SetInteger("GameResult", 2);
        }
    }
}
