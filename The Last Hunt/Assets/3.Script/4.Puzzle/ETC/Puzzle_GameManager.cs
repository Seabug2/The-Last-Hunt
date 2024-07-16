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

        //���̵� ��
        FadeBoard.DOFade(0, 1);

        yield return new WaitForSeconds(1);
        FadeBoard.gameObject.SetActive(false);

        uiManager.ShowMessage("���� ������ �� �� �ְ� Ÿ���� �Ű��ּ���!", 5f);

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

        //���̵� �ƿ�
        FadeBoard.DOFade(1, 3);

        yield return new WaitForSeconds(1f);
        // ���� ������ �����ؾ���
        SceneManager.LoadScene(0);
    }

    [Header("UI"), Space(10)]
    public Puzzle_UI_Manager uiManager;
    [SerializeField, Header("���̵�ƿ�")]
    Image FadeBoard;

}

public class Puzzle_Score
{
    /// <summary>
    /// Ŭ������� �ɸ� �ð�
    /// </summary>
    double time = 0;
    /// <summary>
    /// Ŭ������� Ÿ���� �ű� Ƚ�� (Set Tile �� ������)
    /// </summary>
    int count = 0;

    public Puzzle_Score(float _time, int _count)
    {
        time = _time;
        count = _count;
    }
}