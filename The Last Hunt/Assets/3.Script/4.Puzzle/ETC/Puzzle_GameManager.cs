using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
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

        //if (instance == null)
        //{
        //}
        //else
        //{
        //    Destroy(this.gameObject);
        //    return;
        //}
    }

    public const float tileSize = 3;

    /// <summary>
    /// 게임이 끝났는가?
    /// </summary>
    public bool IsGameOver { get; private set; }

    [SerializeField, Header("플레이어"), Space(10)]
    GameObject hunter;
    public GameObject Hunter => hunter;

    [SerializeField, Header("말"), Space(10)]
    GameObject horse;
    public GameObject Horse => horse;

    [SerializeField, Header("타일 레이어"), Space(10)]
    LayerMask tileLayer;
    public LayerMask TileLayer => tileLayer;

    [SerializeField, Header("길 타일"), Space(10)]
    Puzzle_Road[] roadTiles;

    [SerializeField, Header("시네머신 카메라"), Space(10)]
    CinemachineVirtualCamera firstCam; //시작 카메라 위치
    [SerializeField]
    CinemachineBlendListCamera HomeViewCam; //시작 카메라 위치
    [SerializeField]
    CinemachineVirtualCamera traceVCam;
    CinemachineBrain brainCam;

    private void Start()
    {
        StartCoroutine(StartEvent_co());
    }

    public void GameStart()
    {
        GameStartEvent?.Invoke();
    }

    public void GameClear()
    {

    }

    public void GameOver()
    {

    }

    public UnityEvent GameStartEvent;
    public UnityEvent GameClearEvent;
    public UnityEvent GameOverEvent;

    void Init()
    {
        GameClearEvent.AddListener(() =>
        {
            print("게임 클리어!");
        });
        GameOverEvent.AddListener(() =>
        {
            print("게임 오버...");

            StartCoroutine(GameOverEvent_co());
        });

        Screen.SetResolution(Mathf.RoundToInt(1920 * .5f), Mathf.RoundToInt(1080 * .5f), FullScreenMode.Windowed);
        blackBoard.rectTransform.rect.Set(0, 0, Screen.width, Screen.width);

        roadTiles = FindObjectsOfType<Puzzle_Road>();
        hunter.GetComponent<Puzzle_Hunter_Input>().enabled = false;
        hunter.GetComponent<Puzzle_Hunter_Movement>().enabled = false;
        hunter.transform.position = new Vector3(0, 0, -3);
        horse.transform.position = new Vector3(-1.5f, 0, 0);

        brainCam = Camera.main.GetComponent<CinemachineBrain>();
    }
    [SerializeField]
    Transform homePosition;

    IEnumerator StartEvent_co()
    {
        firstCam.Priority = 10;
        blackBoard.color = Color.black;
        blackBoard.gameObject.SetActive(true);

        Init();

        int i = 12;
        //타일 설치하기
        while (i < Mathf.RoundToInt(homePosition.position.x)-2)
        {
            SetDeadTile(i);
            //if (Random.Range(0, 2) == 0) SetDeadTile(i);
            //else SetObstacleNItem(i);
            i = +2;
        }

        //페이드 인
        //yield return StartCoroutine(FadeIn_co());
        blackBoard.DOFade(0, fadeTime);
        yield return new WaitForSeconds(fadeTime);
        blackBoard.gameObject.SetActive(false);

        ShowMessage("말이 집까지 갈 수 있게 타일을 옮겨주세요!", 5f);
        
        yield return new WaitForSeconds(5);

        HomeViewCam.Priority = firstCam.Priority + 1;

        yield return new WaitForSeconds(15);

        traceVCam.Priority = HomeViewCam.Priority + 1;

        yield return null;
        yield return new WaitWhile(() => brainCam.IsBlending);

        yield return new WaitForSeconds(1f);

        // 플레이어 조작과 이동 활성화
        hunter.GetComponent<Puzzle_Hunter_Input>().enabled = true;
        hunter.GetComponent<Puzzle_Hunter_Movement>().enabled = true;
        // 말 이동 시작
        horse.GetComponent<Puzzle_Horse>().MoveToNextTile();

        // 타이머 활성화
        // 기타 UI 활성화
    }

    IEnumerator GameOverEvent_co()
    {
        yield return new WaitForSeconds(1f);

        //플레이어가 화나는 모습을 보여주며...?

        yield return StartCoroutine(FadeOut_co());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    [SerializeField]
    GameObject deadTile;
    [SerializeField]
    GameObject basicTile;
    [SerializeField]
    GameObject[] item;
    [SerializeField]
    GameObject[] obstacle;

    public void SetDeadTile(int _i)
    {
        //2개 설치
        for (int i = 1; i <= 2; i++)
        {
            Instantiate(deadTile, new Vector3(_i + i, 0, Mathf.RoundToInt(Random.Range(-9f, 9f) / 3 * 3)), Quaternion.identity);
        }
    }

    public void SetObstacleNItem(int _i)
    {
        int caes = Random.Range(0, obstacle.Length);


        //아이템 생성
        //GameObject tile = Instantiate(deadTile, new Vector3(_i + 1, 0, RandomVerticalPosition()), Quaternion.identity);

        ////장애물 생성
        //tile = Instantiate(deadTile, new Vector3(_i + 1, 0, RandomVerticalPosition()), Quaternion.identity);

    }

    public void ResetGame()
    {
        StartCoroutine(GameOverEvent_co());
    }

    [SerializeField,Header("UI"), Space(10)]
    Image blackBoard;
    [SerializeField]
    float fadeTime = 1;
    IEnumerator FadeIn_co()
    {
        blackBoard.gameObject.SetActive(true);
        blackBoard.color = new Color(0, 0, 0, 1);

        float t = fadeTime;

        while (t > 0)
        {
            float a = Mathf.LerpUnclamped(0, 1, t);
            blackBoard.color = new Color(0, 0, 0, a);

            yield return new WaitForFixedUpdate();

            t -= Time.fixedDeltaTime;
        }

        blackBoard.gameObject.SetActive(false);
    }

    IEnumerator FadeOut_co()
    {
        blackBoard.gameObject.SetActive(true);
        blackBoard.color = new Color(0, 0, 0, 0);

        float t = 0;

        while (t < fadeTime)
        {
            float a = Mathf.LerpUnclamped(0, 1, t);
            blackBoard.color = new Color(0, 0, 0, a);

            yield return new WaitForFixedUpdate();

            t += Time.fixedDeltaTime;
        }

        blackBoard.color = new Color(0, 0, 0, 1);
    }


    [SerializeField]
    Text message;
    [SerializeField]
    RectTransform textBack;

    private Tween currentTween;

    public void ShowMessage(string _message, float _time = 1)
    {
        // 기존 애니메이션이 있으면 중단
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }

        message.text = _message;

        // 애니메이션 설정
        textBack.sizeDelta = new Vector2(1920, 0); // 시작 크기
        textBack.gameObject.SetActive(true);
        Sequence mySequence = DOTween.Sequence();

        // 0.5초 동안 크기를 키우기
        mySequence.Append(textBack.DOSizeDelta(new Vector2(1920, 126), 0.5f).SetEase(Ease.InOutQuad));

        // 1초 대기
        mySequence.AppendInterval(_time);

        // 0.5초 동안 크기를 다시 줄이기
        mySequence.Append(textBack.DOSizeDelta(new Vector2(1920, 0), 0.5f).SetEase(Ease.InOutQuad));

        textBack.gameObject.SetActive(false);

        // 현재 애니메이션 저장
        currentTween = mySequence;
    }
}
