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

    System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

    public const int tileSize = 3;

    [SerializeField, Header("타일 레이어"), Space(10)]
    LayerMask tileLayer;
    public LayerMask TileLayer => tileLayer;

    /// <summary>
    /// 게임 시작시 시네머신 카메라
    /// </summary>
    [SerializeField, Header("시네머신 카메라"), Space(10)]
    CinemachineBlendListCamera HomeViewCam;
    /// <summary>
    /// 본 게임이 실행되면 전환되는 시네머신 카메라
    /// follow는 사냥꾼이 기본
    /// 말이 죽는 이벤트라면 말로 변경
    /// </summary>
    [SerializeField]
    CinemachineVirtualCamera traceVCam;
    [SerializeField]
    CinemachineVirtualCamera lookAtHunterVCam;
    [SerializeField]
    CinemachineVirtualCamera lookAtHorseVCam;
    /// <summary>
    /// 메인 카메라
    /// </summary>
    CinemachineBrain brainCam;

    /// <summary>
    /// 본 게임이 실행되는 순간
    /// </summary>
    public UnityEvent GameStartEvent;

    /// <summary>
    /// (성공, 실패 상관없이)게임이 끝나는 순간
    /// </summary>
    public UnityEvent EndGame;

    /// <summary>
    /// 본 게임이 실행되는 순간
    /// </summary>
    public UnityEvent GameClearEvent;


    public bool IsGameOver { get; private set; }

    void Init()
    {
        IsGameOver = false;
        EndGame.AddListener(() => IsGameOver = true);

        ScreenSize = new Vector2(Screen.width, Screen.height);
        text = message.GetComponentInChildren<Text>();
        message.gameObject.SetActive(false);
        messageBoxHeight = message.sizeDelta.y;
        fadeBoard.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
        brainCam = Camera.main.GetComponent<CinemachineBrain>();

        HomeViewCam.m_Instructions[0].m_Hold = Mathf.Infinity;
    }

    IEnumerator Start()
    {
        yield return StartCoroutine(Fade_co(false, 3.5f));

        ShowMessage("타일을 옮겨 말이 집까지 갈 수 있게 만드세요", out float tt, 3f);
        yield return new WaitForSeconds(0.8f);

        float waitTime = Time.time;
        while (Time.time - waitTime < tt - .8f)
        {
            if (Input.anyKeyDown)
            {
                MessageCut();
                break;
            }

            yield return null;
        }

        if (IsNoMessage)
        {
            traceVCam.Priority = HomeViewCam.Priority + 1;
            yield return new WaitForSeconds(brainCam.m_DefaultBlend.BlendTime);
        }
        else
        {
            HomeViewCam.m_Instructions[0].m_Hold = 0;
            float blendTime = 0;
            for (int i = 0; i < HomeViewCam.m_Instructions.Length; i++)
            {
                blendTime += HomeViewCam.m_Instructions[i].m_Hold;
                if (i > 0)
                {
                    blendTime += HomeViewCam.m_Instructions[i].m_Blend.BlendTime;
                }
            }
            yield return new WaitForSeconds(blendTime);
            traceVCam.Priority = HomeViewCam.Priority + 1;
        }

        GameStartEvent.Invoke();
        stopwatch.Start();
    }

    /// <summary>
    /// 시네머신의 추적 대상을 말로 바꾼다
    /// </summary>
    public void VCamFollowHorse()
    {
        lookAtHorseVCam.Priority = 100;//brainCam.ActiveVirtualCamera.Priority + 1;
    }
    /// <summary>
    /// 사냥꾼을 더 가까이에서 봅니다.
    /// </summary>
    public void LookAtHunter()
    {
        //플레이어 시네 뷰
        lookAtHunterVCam.Priority = brainCam.ActiveVirtualCamera.Priority + 1;
    }

    public void GameOver_Horse(Puzzle_Horse horse)
    {
        StartCoroutine(GameOver_Horse_co(horse));
    }

    /// <summary>
    /// 말이 죽거나 떨어질 때 실행
    /// </summary>
    IEnumerator GameOver_Horse_co(Puzzle_Horse horse)
    {
        Destroy(lookAtHorseVCam.GetCinemachineComponent<CinemachineTransposer>());
        yield return new WaitUntil(() => brainCam.ActiveVirtualCamera.Equals(lookAtHorseVCam));
        yield return new WaitWhile(() => brainCam.IsBlending);

        //떨어진게 아니라면 폭발
        if (!horse.isFallen)
        {
            horse.Explosion();
        }
        else
        {
            horse.FallingEvent?.Invoke();
        }

        yield return new WaitForSeconds(2.5f);

        lookAtHunterVCam.Priority = brainCam.ActiveVirtualCamera.Priority + 1;
        yield return new WaitUntil(()=> brainCam.ActiveVirtualCamera.Equals(lookAtHunterVCam));
        yield return new WaitWhile(() => brainCam.IsBlending);

        //플레이어 애니메이션 재생
        FindObjectOfType<Puzzle_Hunter>().Anim.SetTrigger("Failed");

        yield return new WaitForSeconds(1.2f);

        //메세지 출력
        ShowMessage("이런 멍청한 말 같으니...!", out float tt, 3.5f);

        yield return new WaitForSeconds(1f);

        float waitTime = Time.time;
        while (Time.time - waitTime < tt - 1)
        {
            if (Input.anyKeyDown)
            {
                MessageCut();
                break;
            }
            yield return null;
        }

        yield return StartCoroutine(Fade_co(true, 3f));

        yield return null;
        //현재 활성화된 씬을 다시 실행
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void GameOver_Hunter(Puzzle_Hunter_TileAction hunter)
    {
        StartCoroutine(GameOver_Hunter_co(hunter));
    }

    IEnumerator GameOver_Hunter_co(Puzzle_Hunter_TileAction hunter)
    {
        if(hunter.isFallen)
        {
            hunter.FallingEvent?.Invoke();
        }
        else
        {
            hunter.GetComponent<Animator>().SetTrigger("Dead");
        }

        yield return new WaitForSeconds(.5f);

        yield return StartCoroutine(Fade_co(true, 3f));

        //메세지 출력
        ShowMessage("젠장... 다시 한 번 해보자...", out float tt, 3.5f);
        yield return new WaitForSeconds(1f);
        float waitTime = Time.time;
        while (Time.time - waitTime < tt - 1)
        {
            if (Input.anyKeyDown)
            {
                MessageCut();
                break;
            }
            yield return null;
        }

        //현재 활성화된 씬을 다시 실행
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator Fade_co(bool isFadeOut, float fadeTime = 1f)
    {
        fadeBoard.color = new Color(0, 0, 0, isFadeOut ? 0 : 1);
        fadeBoard.gameObject.SetActive(true);

        fadeBoard.DOFade(isFadeOut ? 1 : 0, fadeTime).SetEase(Ease.InQuad);
        yield return new WaitForSeconds(fadeTime);

        if (!isFadeOut)
            fadeBoard.gameObject.SetActive(false);

        yield break;
    }

    public Timer timer;
    public Text currentScore;
    public Text bestScore;
    public GameObject nextButton;
    public GameObject returnButton;
    public IEnumerator GameClear_co()
    {
        EndGame?.Invoke();
        float time = (float)timer.time;
        currentScore.text = Timer.ConvertTimeCode(time);
        print(time);

        if (GameManager.instance.userDate.score[3] > time || GameManager.instance.userDate.score[3] == 0)
        {
            GameManager.instance.userDate.score[3] = time;
            GameManager.instance.SaveJson();
            print("최고 점수 갱신!");
        }

        bestScore.text = Timer.ConvertTimeCode(GameManager.instance.userDate.score[3]);

        if (GameManager.instance.IsStoryMode)
        {
            nextButton.SetActive(true);
            returnButton.SetActive(false);
        }
        else
        {
            nextButton.SetActive(false);
            returnButton.SetActive(true);
        }

        traceVCam.Follow = FindObjectOfType<Puzzle_Horse>().transform;

        yield return null;
        yield return new WaitWhile(() => brainCam.IsBlending);

        ShowMessage("게임 클리어!", out float tt, 4f);
        yield return new WaitForSeconds(tt);

        lookAtHunterVCam.Priority = brainCam.ActiveVirtualCamera.Priority + 1;
        yield return new WaitUntil(()=>brainCam.ActiveVirtualCamera.Equals(lookAtHunterVCam));
        yield return new WaitWhile(() => brainCam.IsBlending);

        GameClearEvent?.Invoke();
    }

    #region 메시지 출력
    [SerializeField, Header("페이드아웃"), Space(10)]
    Image fadeBoard;
    [SerializeField, Header("메세지")]
    RectTransform message;
    Text text;

    public Vector2 ScreenSize { get; private set; }
    float messageBoxHeight;

    public bool IsNoMessage
    {
        get { return currentTween == null; }
    }

    Tween currentTween = null;

    public void ShowMessage(string _message, out float totalTime, float _time = 1)
    {
        totalTime = _time + .44f;
        // 기존 애니메이션이 있으면 중단
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }

        text.text = _message;

        // 애니메이션 설정
        message.sizeDelta = new Vector2(ScreenSize.x, 0); // 시작 크기
        message.gameObject.SetActive(true);
        Sequence mySequence = DOTween.Sequence();

        // 0.5초 동안 크기를 키우기
        mySequence.Append(message.DOSizeDelta(new Vector2(ScreenSize.x, messageBoxHeight), 0.22f).SetEase(Ease.InOutQuad));
        // _time 동안 대기
        mySequence.AppendInterval(_time);

        // 0.5초 동안 크기를 다시 줄이기
        mySequence.Append(message.DOSizeDelta(new Vector2(1920, 0), 0.22f).SetEase(Ease.InOutQuad));

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
            currentTween = null;
        }

        // 메세지 비활성화
        message.gameObject.SetActive(false);
    }
    #endregion
}