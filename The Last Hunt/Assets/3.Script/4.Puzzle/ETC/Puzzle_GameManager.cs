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
    /// ������ �����°�?
    /// </summary>
    public bool IsGameOver { get; private set; }

    [SerializeField, Header("�÷��̾�"), Space(10)]
    GameObject hunter;
    public GameObject Hunter => hunter;

    [SerializeField, Header("��"), Space(10)]
    GameObject horse;
    public GameObject Horse => horse;

    [SerializeField, Header("Ÿ�� ���̾�"), Space(10)]
    LayerMask tileLayer;
    public LayerMask TileLayer => tileLayer;

    [SerializeField, Header("�� Ÿ��"), Space(10)]
    Puzzle_Road[] roadTiles;

    [SerializeField, Header("�ó׸ӽ� ī�޶�"), Space(10)]
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
            print("���� Ŭ����!");
        });
        GameOverEvent.AddListener(() =>
        {
            print("���� ����...");

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

        //���̵� ��
        //yield return StartCoroutine(FadeIn_co());
        blackBoard.DOFade(0, fadeTime);
        yield return new WaitForSeconds(fadeTime);
        blackBoard.gameObject.SetActive(false);

        yield return new WaitForSeconds(7 - fadeTime);

        //�޼��� ���

        //Ÿ���� �ϳ���...
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

        // �÷��̾� ���۰� �̵� Ȱ��ȭ
        hunter.GetComponent<Puzzle_Hunter_Input>().enabled = true;
        hunter.GetComponent<Puzzle_Hunter_Movement>().enabled = true;
        // �� �̵� ����
        horse.GetComponent<Puzzle_Horse>().MoveToNextTile();

        // Ÿ�̸� Ȱ��ȭ
        // ��Ÿ UI Ȱ��ȭ
    }

    IEnumerator GameOverEvent_co()
    {
        yield return new WaitForSeconds(1f);

        //�÷��̾ ȭ���� ����� �����ָ�...?

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
