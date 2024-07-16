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
            Destroy(instance);
        }
        instance = this;
    }

    public const int tileSize = 3;

    /// <summary>
    /// 게임이 끝났는가?
    /// </summary>
    public bool IsGameOver { get; private set; }

    [SerializeField, Header("캐릭터"), Space(10)]
    GameObject hunter;
    [SerializeField]
    GameObject horse;
    public GameObject Hunter => hunter;
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
    }

    public void GameStart()
    {
        GameStartEvent?.Invoke();
    }
    public UnityEvent GameStartEvent;

    public UnityEvent GameClearEvent;
    public UnityEvent GameOverEvent;

    void Init()
    {
        FadeBoard.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
        brainCam = Camera.main.GetComponent<CinemachineBrain>();
    }

    IEnumerator StartEvent_co()
    {
        firstCam.Priority = 10;
        FadeBoard.color = Color.black;
        FadeBoard.gameObject.SetActive(true);
        Init();

        //페이드 인
        FadeBoard.DOFade(0, 1);

        yield return new WaitForSeconds(1);
        FadeBoard.gameObject.SetActive(false);

        uiManager.ShowMessage("말이 집까지 갈 수 있게 타일을 옮겨주세요!", 5f);

        bool isPassed = false;
        float t = Time.time;

        while (Time.time - t < 5)
        {
            if (Input.anyKeyDown)
            {
                isPassed = true;

                break;
            }

            yield return null;
        }

        if (!isPassed)
        {
            HomeViewCam.Priority = firstCam.Priority + 1;
            yield return new WaitForSeconds(13);
            traceVCam.Priority = HomeViewCam.Priority + 1;
        }

        else
            traceVCam.Priority = firstCam.Priority + 1;

        yield return null;
        yield return new WaitWhile(() => brainCam.IsBlending);

        yield return new WaitForSeconds(1f);

        GameStart();
    }

    void GameClear()
    {
        PuzzleGameClear();
    }

    IEnumerator PuzzleGameClear()
    {
        FadeBoard.color = Vector4.zero;
        FadeBoard.gameObject.SetActive(true);

        //페이드 아웃
        FadeBoard.DOFade(1, 3);

        yield return new WaitForSeconds(1f);
        // 다음 씬으로 수정해야함
        SceneManager.LoadScene(0);
    }

    [Header("UI"), Space(10)]
    public Puzzle_UI_Manager uiManager;
    [SerializeField, Header("페이드아웃")]
    Image FadeBoard;

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