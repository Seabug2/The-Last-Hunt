using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using DG.Tweening;

public class Rhythm_ChapterManager : MonoBehaviour
{
    public float Gamespeed = 1f;
    public int Maxcount = 0;
    public int Hitcount = 0;
    public int Misscount = 0;
    public float percent = 0;
    public bool BGMisPlaying, BGMisPausing;
    [SerializeField] private GameObject resultUI, introUI, Menu, NoteUI, BestScore;
    [SerializeField] private Animator Hunter_ani;

    [SerializeField] private Sprite[] judgeImages;
    [SerializeField] private Image judgeImageAppear;

    [Header("���")]
    [SerializeField] private Image ClearImage;
    [SerializeField] private Sprite ClearSP, FailSP;
    [SerializeField] private Text maxT, hitT, missT, scoreT, RecordT;
    [SerializeField] private GameObject NextButton, MainButton;

    [Header("ī�޶�")]
    [SerializeField] private CinemachineBrain brainCam;
    [SerializeField] private CinemachineVirtualCamera initVcam;
    [SerializeField] private CinemachineVirtualCamera mainVcam;
    [SerializeField] private CinemachineBlendListCamera mainBcam;
    [SerializeField] private CinemachineVirtualCamera resultVcam;



    // 0. �̱��� ����
    public static Rhythm_ChapterManager instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else { Destroy(gameObject); }
    }

    // �޽��� ��� �κ�
    Tween currentTween = null;
    [Header("�޽���")]
    // [SerializeField] private Image HeaderBoard;
    [SerializeField] private RectTransform message;
    [SerializeField] private Text text;
    public bool isNoMessage
    {
        get { return currentTween == null || !currentTween.IsActive(); }
    }
    public void ShowMessage(string _message, float _time = 1)
    {
        // ���� �ִϸ��̼��� ������ �ߴ�
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }

        text.text = _message;

        // �ִϸ��̼� ����
        message.sizeDelta = new Vector2(1920, 0); // ���� ũ��
        message.gameObject.SetActive(true);
        Sequence mySequence = DOTween.Sequence();

        // 0.5�� ���� ũ�⸦ Ű���
        mySequence.Append(message.DOSizeDelta(new Vector2(1920, 126), 1f).SetEase(Ease.InOutQuad));

        // _time ���� ���
        mySequence.AppendInterval(_time);

        // 0.5�� ���� ũ�⸦ �ٽ� ���̱�
        mySequence.Append(message.DOSizeDelta(new Vector2(1920, 0), 0.35f).SetEase(Ease.InOutQuad));

        // �ִϸ��̼��� ���� �� ��Ȱ��ȭ
        mySequence.OnComplete(() => message.gameObject.SetActive(false));

        // ���� �ִϸ��̼� ����
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
        yield return new WaitForSeconds(1.5f);
        mainVcam.Priority = initVcam.Priority + 1;
        yield return new WaitForSeconds(2.5f);
        PlaySFX("ChapterIntro");
        ShowMessage(text.text, 1.5f);
        yield return new WaitForSeconds(3f);
        Rhythm_SoundManager.instance.PlayBGM("BGM");
        mainBcam.Priority = mainVcam.Priority + 1;
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
        if (GameManager.instance.IsStoryMode) MainButton.SetActive(false);
        else NextButton.SetActive(false);

        resultVcam.Priority = mainBcam.Priority + 1;
        Menu.SetActive(false);
        NoteUI.SetActive(false);
        BGMisPlaying = false;
        percent = (100f * Maxcount + 60 * Hitcount) / (Maxcount + Hitcount + Misscount);
        resultUI.SetActive(true);

        maxT.text = Maxcount.ToString();
        hitT.text = Hitcount.ToString();
        missT.text = Misscount.ToString();
        scoreT.text = percent.ToString("0.00");

        // ����
        if (percent < 60)
        {
            PlaySFX("ChapterFail");
            ClearImage.sprite = FailSP;
            Hunter_ani.SetInteger("GameResult", -1);
            BestScore.SetActive(false);
        }
        // ����
        else
        {
            if (GameManager.instance.IsNewHighScore(1, percent)) 
            {
                RecordT.color = Color.yellow;
            }
            PlaySFX("ChapterClear");
            ClearImage.sprite = ClearSP;
            Hunter_ani.SetInteger("GameResult", 2);
            RecordT.text = GameManager.instance.userData.score[1].ToString("0.00");
        }
    }
}
