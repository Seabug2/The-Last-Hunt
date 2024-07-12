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

        Init();
    }

    [SerializeField, Header("타일 레이어"), Space(10)]
    LayerMask tileLayer;

    public LayerMask TileLayer => tileLayer;

    GameObject hunter;
    public GameObject Hunter => hunter;

    GameObject horse;
    public GameObject Horse => horse;

    [SerializeField, Header("길 타일"), Space(10)]
    Puzzle_Road[] roadTiles;

    [SerializeField, Header("시네머신 카메라"), Space(10)]
    CinemachineVirtualCamera goalVCam;
    [SerializeField]
    CinemachineVirtualCamera traceVCam;


    private void Start()
    {
        StartCoroutine(StartEvent_co());
    }

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

        blackBoard.rectTransform.rect.Set(0, 0, Screen.width, Screen.width);

        roadTiles = FindObjectsOfType<Puzzle_Road>();
        hunter = FindObjectOfType<Puzzle_Hunter_Movement>().gameObject;
        hunter.transform.position = new Vector3(0, 0, -3);
        horse = FindObjectOfType<Puzzle_Horse_Movement>().gameObject;
        horse.transform.position = new Vector3(-1.5f, 0, 0);
    }

    IEnumerator StartEvent_co()
    {
        Init();

        //페이드 인
        yield return StartCoroutine(FadeIn_co());

        //메세지 출력
        //타일을 하나씩...
        List<Puzzle_Road> roads = new List<Puzzle_Road>(roadTiles);

        float t = 1f;

        //while (roads.Count > 0)
        //{
        //    Puzzle_Road road = roads[Random.Range(0, roads.Count)];
        //    roads.Remove(road);
        //    yield return new WaitForSeconds(t);
        //    t *= .9f;
        //}

        traceVCam.Priority = goalVCam.Priority + 1;
        // 플레이어 조작과 이동 활성화

        // 말 이동 시작
        horse.GetComponent<Puzzle_Horse_Movement>().MoveToNextTile();
        // 타이머 활성화
        // 기타 UI 활성화
    }

    IEnumerator GameOverEvent_co()
    {
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

        while (blackBoard.color.a > 0)
        {
            blackBoard.color -= new Color(0, 0, 0, Time.fixedDeltaTime * fadeTime);
            yield return new WaitForFixedUpdate();
        }

        blackBoard.gameObject.SetActive(false);
    }

    IEnumerator FadeOut_co()
    {
        blackBoard.gameObject.SetActive(true);
        blackBoard.color = new Color(0, 0, 0, 0);

        while (blackBoard.color.a < 1)
        {
            blackBoard.color += new Color(0, 0, 0, Time.fixedDeltaTime * fadeTime);
            yield return new WaitForFixedUpdate();
        }

        blackBoard.color = new Color(0, 0, 0, 1);
    }
}
