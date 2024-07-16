using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Cinemachine;
using DG.Tweening;


public class Puzzle_GameManager : MonoBehaviour
{
    public static Puzzle_GameManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        Init();
    }

    public const int tileSize = 3;

    [SerializeField, Header("사냥꾼")]
    GameObject hunter;
    public GameObject Hunter => hunter;
    [SerializeField, Header("말"), Space(10)]
    GameObject horse;
    public GameObject Horse => horse;

    [SerializeField, Header("타일 레이어"), Space(10)]
    LayerMask tileLayer;
    public LayerMask TileLayer => tileLayer;

    [SerializeField, Header("시네머신 카메라"), Space(10)]
    CinemachineVirtualCamera firstCam; //시작 카메라 위치
    [SerializeField]
    CinemachineBlendListCamera HomeViewCam; //시작 카메라 위치
    [SerializeField]
    CinemachineVirtualCamera traceVCam;
    [SerializeField]
    CinemachineVirtualCamera hunterVCam;
    CinemachineBrain brainCam;

    private void Start()
    {
        StartCoroutine(StartEvent_co());

        GameClearEvent.AddListener(() =>
        {
            LoadSceneAfterFadeOut(SceneManager.GetActiveScene().buildIndex);
        });

        GameOverEvent.AddListener(() =>
        {
            LoadSceneAfterFadeOut(SceneManager.GetActiveScene().buildIndex);
        });
    }

    public UnityEvent GameStartEvent;
    public UnityEvent GameClearEvent;
    public UnityEvent GameOverEvent;

    void Init()
    {
        text = message.GetComponentInChildren<Text>();
        message.gameObject.SetActive(false);
        fadeBoard.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
        brainCam = Camera.main.GetComponent<CinemachineBrain>();
    }

    IEnumerator StartEvent_co()
    {
        firstCam.Priority = 10;
        fadeBoard.color = Color.black;
        fadeBoard.gameObject.SetActive(true);

        //페이드 인
        fadeBoard.DOFade(0, 1);

        yield return new WaitForSeconds(1);
        fadeBoard.gameObject.SetActive(false);

        ShowMessage("말이 집까지 갈 수 있게 타일을 옮겨주세요!", 5f);

        yield return new WaitForSeconds(0.8f);

        float waitTime = Time.time;

        while (Time.time - waitTime < 4.2f)
        {
            if (Input.anyKeyDown)
            {
                MessageCut();
                break;
            }

            yield return null;
        }

        //중간에 메시지가 중단 되었다면 바로 사냥꾼을 추적하는 가상 카메라로 이동
        if (IsNoMessage)
            traceVCam.Priority = firstCam.Priority + 1;
        else
        {
            HomeViewCam.Priority = firstCam.Priority + 1;
            yield return new WaitForSeconds(13);
            traceVCam.Priority = HomeViewCam.Priority + 1;
        }


        yield return null;
        yield return new WaitWhile(() => brainCam.IsBlending);

        yield return new WaitForSeconds(1.8f);

        GameStartEvent.Invoke();
    }

    public void LoadSceneAfterFadeOut(int _sceneNum)
    {
        fadeBoard.DOFade(1, 3).OnComplete(() =>
        {
            // 이 블록은 DoTween 애니메이션이 끝난 후에 실행됩니다.
            SceneManager.LoadScene(_sceneNum);
        });
    }


    [SerializeField, Header("페이드아웃"), Space(10)]
    Image fadeBoard;
    [SerializeField, Header("메세지")]
    RectTransform message;
    Text text;

    public bool IsNoMessage
    {
        get { return currentTween == null || !currentTween.IsActive(); }
    }

    Tween currentTween = null;

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
        mySequence.Append(message.DOSizeDelta(new Vector2(1920, 126), 0.5f).SetEase(Ease.InOutQuad));

        // _time 동안 대기
        mySequence.AppendInterval(_time);

        // 0.5초 동안 크기를 다시 줄이기
        mySequence.Append(message.DOSizeDelta(new Vector2(1920, 0), 0.5f).SetEase(Ease.InOutQuad));

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

        // 메세지 비활성화
        message.gameObject.SetActive(false);
    }


}

public class Puzzle_Score
{
    /// <summary>
    /// 클리어까지 걸린 시간
    /// </summary>
    double time = 0;
    /// <summary>
    /// 클리어까지 타일을 옮긴 횟수 (Set Tile 할 때마다)
    /// </summary>
    int count = 0;

    public Puzzle_Score(float _time, int _count)
    {
        time = _time;
        count = _count;
    }
}