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
    }

    public readonly int tileSize = 3;

    /// <summary>
    /// ������ �����°�?
    /// </summary>
    public bool IsGameOver { get; private set; }

    [SerializeField, Header("ĳ����"), Space(10)]
    GameObject hunter;
    [SerializeField]
    GameObject horse;
    public GameObject Hunter => hunter;
    public GameObject Horse => horse;

    [SerializeField, Header("Ÿ�� ���̾�"), Space(10)]
    LayerMask tileLayer;
    public LayerMask TileLayer => tileLayer;

    [SerializeField, Header("�� Ÿ��"), Space(10)]
    Puzzle_Road[] roadTiles;

    [SerializeField, Header("�ó׸ӽ� ī�޶�"), Space(10)]
    CinemachineVirtualCamera firstCam; //���� ī�޶� ��ġ
    [SerializeField]
    CinemachineBlendListCamera HomeViewCam; //���� ī�޶� ��ġ
    [SerializeField]
    CinemachineVirtualCamera traceVCam;
    [SerializeField]
    CinemachineVirtualCamera hunterVCam;
    CinemachineBrain brainCam;

    private void Start()
    {
        StartCoroutine(StartEvent_co());
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
        });

        //Screen.SetResolution(Mathf.RoundToInt(1920 * .5f), Mathf.RoundToInt(1080 * .5f), FullScreenMode.Windowed);
        FadeBoard.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);

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
        FadeBoard.color = Color.black;
        FadeBoard.gameObject.SetActive(true);

        Init();

        GetComponent<Puzzle_TileSpawner>().LevelSetUp();

        //���̵� ��
        //yield return StartCoroutine(FadeIn_co());
        FadeBoard.DOFade(0, 1);
        yield return new WaitForSeconds(1);
        FadeBoard.gameObject.SetActive(false);

        ShowMessage("���� ������ �� �� �ְ� Ÿ���� �Ű��ּ���!", 5f);

        yield return new WaitForSeconds(5);

        HomeViewCam.Priority = firstCam.Priority + 1;

        yield return new WaitForSeconds(13);

        traceVCam.Priority = HomeViewCam.Priority + 1;

        yield return null;
        yield return new WaitWhile(() => brainCam.IsBlending);

        yield return new WaitForSeconds(1f);

        // �÷��̾� ���۰� �̵� Ȱ��ȭ
        hunter.GetComponent<Puzzle_Hunter_Input>().enabled = true;
        hunter.GetComponent<Puzzle_Hunter_Movement>().enabled = true;
        // �� �̵� ����
        horse.GetComponent<Puzzle_Horse>().MoveToNextTile();

        // Ÿ�̸� Ȱ��ȭ
        // ��Ÿ UI Ȱ��ȭ
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
        //2�� ��ġ
        for (int i = 1; i <= 2; i++)
        {
            Instantiate(deadTile, new Vector3(_i + i * 3, 0, Mathf.RoundToInt(Random.Range(-9f, 9f) / 3 * 3)), Quaternion.identity);
        }
    }

    public void SetObstacleNItem(int _i)
    {
        int caes = Random.Range(0, obstacle.Length);


        //������ ����
        //GameObject tile = Instantiate(deadTile, new Vector3(_i + 1, 0, RandomVerticalPosition()), Quaternion.identity);

        ////��ֹ� ����
        //tile = Instantiate(deadTile, new Vector3(_i + 1, 0, RandomVerticalPosition()), Quaternion.identity);

    }

    [SerializeField, Header("UI"), Space(10)]
    Image FadeBoard;

    [SerializeField]
    Text message;
    [SerializeField]
    RectTransform textBack;

    Tween currentTween = null;
    public void ShowMessage(string _message, float _time = 1)
    {
        // ���� �ִϸ��̼��� ������ �ߴ�
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }

        message.text = _message;

        // �ִϸ��̼� ����
        textBack.sizeDelta = new Vector2(1920, 0); // ���� ũ��
        textBack.gameObject.SetActive(true);
        Sequence mySequence = DOTween.Sequence();

        // 0.5�� ���� ũ�⸦ Ű���
        mySequence.Append(textBack.DOSizeDelta(new Vector2(1920, 126), 0.5f).SetEase(Ease.InOutQuad));

        // _time ���� ���
        mySequence.AppendInterval(_time);

        // 0.5�� ���� ũ�⸦ �ٽ� ���̱�
        mySequence.Append(textBack.DOSizeDelta(new Vector2(1920, 0), 0.5f).SetEase(Ease.InOutQuad));

        // �ִϸ��̼��� ���� �� ��Ȱ��ȭ
        mySequence.OnComplete(() => textBack.gameObject.SetActive(false));

        // ���� �ִϸ��̼� ����
        currentTween = mySequence;
    }
}

