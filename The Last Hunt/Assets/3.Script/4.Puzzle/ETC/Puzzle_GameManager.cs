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
    CinemachineBlendListCamera goalVCam;
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

    IEnumerator StartEvent_co()
    {
        blackBoard.gameObject.SetActive(true);
        blackBoard.color = Color.black;

        Init();

        //페이드 인
        //yield return StartCoroutine(FadeIn_co());
        blackBoard.DOFade(0, fadeTime);
        yield return new WaitForSeconds(fadeTime);
        blackBoard.gameObject.SetActive(false);

        yield return new WaitForSeconds(7 - fadeTime);

        //메세지 출력

        //타일을 하나씩...
        List<Puzzle_Road> roads = new List<Puzzle_Road>(roadTiles);

        //float t = 1f;

        //while (roads.Count > 0)
        //{
        //    Puzzle_Road road = roads[Random.Range(0, roads.Count)];
        //    roads.Remove(road);
        //    yield return new WaitForSeconds(t);
        //    t *= .9f;
        //}

        traceVCam.Priority = goalVCam.Priority + 1;

        yield return null;
        yield return new WaitWhile(() => brainCam.IsBlending);

        yield return new WaitForSeconds(2f);

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

    public void ResetGame()
    {
        StartCoroutine(GameOverEvent_co());
    }

    [SerializeField]
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
}
