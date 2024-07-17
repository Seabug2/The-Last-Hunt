using System.Collections;
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
    CinemachineVirtualCamera lookAtHunterVCam;
    [SerializeField]
    CinemachineVirtualCamera lookAtHorseVCam;
    CinemachineBrain brainCam;

    /// <summary>
    /// 본 게임이 실행되는 순간
    /// </summary>
    public UnityEvent GameStartEvent;

    /// <summary>
    /// (성공, 실패 상관없이)게임이 끝나는 순간
    /// </summary>
    public UnityEvent EndGame;
    // 플레이어 조작 중단
    // 말 이동 중단
    // BGM 중단
    // 타이머 정지

    public bool IsGameOver { get; private set; }

    void Init()
    {
        IsGameOver = false;
        EndGame.AddListener(() => IsGameOver = true);

        text = message.GetComponentInChildren<Text>();
        message.gameObject.SetActive(false);
        fadeBoard.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
        brainCam = Camera.main.GetComponent<CinemachineBrain>();
        
        HomeViewCam.m_Instructions[0].m_Hold = Mathf.Infinity;
    }

    IEnumerator Start()
    {
        firstCam.Priority = 10;
        yield return StartCoroutine(Fade_co(false, 3.5f));

        ShowMessage("말이 집까지 갈 수 있게 타일을 옮겨주세요!", 3f);
        yield return new WaitForSeconds(0.8f);

        float waitTime = Time.time;
        while (Time.time - waitTime < 2.2f)
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
            yield return new WaitForSeconds(9);
            traceVCam.Priority = HomeViewCam.Priority + 1;
        }

        yield return null;
        yield return new WaitWhile(() => brainCam.IsBlending);

        GameStartEvent.Invoke();
    }

    /// <summary>
    /// 말이 죽거나 떨어질 때 실행
    /// </summary>
    public IEnumerator GameOver_Horse_co()
    {
        lookAtHunterVCam.Priority = traceVCam.Priority + 1;
        yield return null;
        yield return new WaitWhile(() => brainCam.IsBlending);

        //플레이어 애니메이션 재생
        FindObjectOfType<Puzzle_Hunter>().Anim.SetTrigger("Failed");

        yield return new WaitForSeconds(1.2f);

        //메세지 출력
        ShowMessage("이런 멍청한 말 같으니...!", 3.5f);

        yield return new WaitForSeconds(3.5f);

        yield return StartCoroutine(Fade_co(true, 3f));

        //currentTween = null;

        //현재 활성화된 씬을 다시 실행
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public IEnumerator GameOver_Hunter_co()
    {
        yield return StartCoroutine(Fade_co(true, 3f));

        //메세지 출력
        ShowMessage("젠장... 다시 한 번 해보자...", 5f);
        yield return new WaitForSeconds(7);

        currentTween = null;

        //현재 활성화된 씬을 다시 실행
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator Fade_co(bool isFadeOut, float fadeTime = 1f)
    {
        fadeBoard.color = new Color(0, 0, 0, isFadeOut ? 0 : 1);
        fadeBoard.gameObject.SetActive(true);

        fadeBoard.DOFade(isFadeOut ? 1 : 0, fadeTime);
        yield return new WaitForSeconds(fadeTime);

        if (!isFadeOut)
            fadeBoard.gameObject.SetActive(false);

        yield break;
    }

    [SerializeField, Header("다음 씬"), Space(10)]
    string nextSceneName;

    public void GameClear()
    {
        StartCoroutine(GameClear_co());
    }

    IEnumerator GameClear_co()
    {
        IsGameOver = true;

        yield return StartCoroutine(Fade_co(false, 3f));

        ShowMessage("게임 클리어!", 5f);
        yield return new WaitForSeconds(5);

        SceneManager.LoadScene(nextSceneName);
    }


    #region 메시지 출력
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

        // 메세지 비활성화
        message.gameObject.SetActive(false);
    }
    #endregion
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