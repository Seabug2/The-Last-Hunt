using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using Cinemachine;

public class Run_Manager : MonoBehaviour
{
    public static Run_Manager instance;

    public bool IsGameOver { get; private set; }

    [Header("UI")]
    [SerializeField]
    Image fadeBoard;
    [SerializeField]
    RectTransform message;
    [SerializeField]
    GameObject nextButton;
    [SerializeField]
    GameObject homeButton;
    [SerializeField]
    Text currentScoreText;
    [SerializeField]
    Text bestScoreText;

    [Header("시네머신"), Space(10)]
    [SerializeField]
    CinemachineBrain brain;
    [SerializeField]
    CinemachineVirtualCamera IntroVCam;
    [SerializeField]
    CinemachineVirtualCamera InGameVCam;
    [SerializeField]
    CinemachineVirtualCamera EndingVCam;

    [Header("시스템"), Space(10)]
    [SerializeField]
    Timer timer;
    [SerializeField]
    float targetTime = 120f;

    [Header("이벤트"), Space(10)]
    public UnityEvent StartEvent;
    public UnityEvent EndEvent;
    public UnityEvent GameOverEvent;
    public UnityEvent GameClearEvent;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            Init();
            return;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void EventSetting()
    {
        //StartEvent.AddListener(() => { });
        EndEvent.AddListener(() =>
        {
            StartCoroutine(GameClose());
        });
        //GameOverEvent.AddListener(() => { });
        //GameClearEvent.AddListener(() => { });
    }

    private void Init()
    {
        IsGameOver = false;

        message.gameObject.SetActive(false);
        IntroVCam.Priority = 1;
        InGameVCam.Priority = 0;
        EndingVCam.Priority = 0;
        EventSetting();
    }

    IEnumerator Start()
    {
        // 페이드 인
        fadeBoard.color = Color.black;
        fadeBoard.DOFade(0, 3f).SetEase(Ease.InQuart).OnComplete(() => fadeBoard.gameObject.SetActive(false));
        yield return new WaitForSeconds(3.3f);

        // 메세지 출력
        message.sizeDelta = new Vector2(Screen.width, 0);
        message.gameObject.SetActive(true);
        message.DOSizeDelta(new Vector2(Screen.width, 150f), 1.8f).SetEase(Ease.OutCubic);
        yield return new WaitForSeconds(2f);

        float t = 3f;
        while (t > 0)
        {
            t -= Time.deltaTime;
            if (Input.anyKeyDown)
            {
                message.gameObject.SetActive(false);
                break;
            }
            yield return null;
        }

        if (message.gameObject.activeSelf)
        {
            message.DOSizeDelta(new Vector2(Screen.width, 0), 1f).SetEase(Ease.OutCubic).OnComplete(() => message.gameObject.SetActive(false));
            yield return new WaitForSeconds(1f);
        }

        InGameVCam.Priority = IntroVCam.Priority + 1;

        yield return new WaitUntil(()=>brain.ActiveVirtualCamera.Equals(InGameVCam));

        StartEvent?.Invoke();
    }

    IEnumerator GameClose()
    {
        float currentScore = (float)timer.time;
        if (currentScore >= targetTime)
        {
            print("2분 이상을 기록");

            nextButton.gameObject.SetActive(GameManager.instance.IsStoryMode);
            homeButton.gameObject.SetActive(!GameManager.instance.IsStoryMode);

            if (GameManager.instance.IsNewHighScore(2, currentScore))
            {
                print("기록 갱신!");
            }

            currentScoreText.text = Timer.ConvertTimeCode(currentScore);
            bestScoreText.text = Timer.ConvertTimeCode(GameManager.instance.userData.score[2]);

            GameClearEvent?.Invoke();
            yield break;
        }
        else
        {
            message.transform.GetComponentInChildren<Text>().text = "2분 만 버텨보자...!";
            // 메세지 출력
            message.sizeDelta = new Vector2(Screen.width, 0);
            message.gameObject.SetActive(true);
            message.DOSizeDelta(new Vector2(Screen.width, 150f), 1.8f).SetEase(Ease.OutCubic);
            yield return new WaitForSeconds(2f);

            float t = 3f;
            while (t > 0)
            {
                t -= Time.deltaTime;
                if (Input.anyKeyDown)
                {
                    message.gameObject.SetActive(false);
                    break;
                }
                yield return null;
            }

            if (message.gameObject.activeSelf)
            {
                message.DOSizeDelta(new Vector2(Screen.width, 0), 1f).SetEase(Ease.OutCubic).OnComplete(() => message.gameObject.SetActive(false));
                yield return new WaitForSeconds(1f);
            }

            print("게임 실패");
            GameOverEvent?.Invoke();
        }
    }

}
