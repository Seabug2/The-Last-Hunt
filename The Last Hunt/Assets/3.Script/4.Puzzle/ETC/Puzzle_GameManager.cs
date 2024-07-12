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

    [SerializeField, Header("Ÿ�� ���̾�"), Space(10)]
    LayerMask tileLayer;

    public LayerMask TileLayer => tileLayer;

    GameObject hunter;
    public GameObject Hunter => hunter;

    GameObject horse;
    public GameObject Horse => horse;

    [SerializeField, Header("�� Ÿ��"), Space(10)]
    Puzzle_Road[] roadTiles;

    [SerializeField, Header("�ó׸ӽ� ī�޶�"), Space(10)]
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
            print("���� Ŭ����!");
        });
        GameOverEvent.AddListener(() =>
        {
            print("���� ����...");

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

        //���̵� ��
        yield return StartCoroutine(FadeIn_co());

        //�޼��� ���
        //Ÿ���� �ϳ���...
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
        // �÷��̾� ���۰� �̵� Ȱ��ȭ

        // �� �̵� ����
        horse.GetComponent<Puzzle_Horse_Movement>().MoveToNextTile();
        // Ÿ�̸� Ȱ��ȭ
        // ��Ÿ UI Ȱ��ȭ
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
