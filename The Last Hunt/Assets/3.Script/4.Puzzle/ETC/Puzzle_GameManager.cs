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
            Destroy(gameObject);
            return;
        }
        instance = this;

        Init();
    }

    public const int tileSize = 3;

    [SerializeField, Header("��ɲ�")]
    GameObject hunter;
    public GameObject Hunter => hunter;
    [SerializeField, Header("��"), Space(10)]
    GameObject horse;
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

        GameClearEvent.AddListener(() =>
        {
            LoadSceneAfterFadeOut(SceneManager.GetActiveScene().buildIndex);
        });

        GameOverEvent.AddListener(() =>
        {
            LoadSceneAfterFadeOut(SceneManager.GetActiveScene().buildIndex);
        });
    }

    public UnityEvent GameStartEvent;
    public UnityEvent GameClearEvent;
    public UnityEvent GameOverEvent;

    void Init()
    {
        text = message.GetComponentInChildren<Text>();
        message.gameObject.SetActive(false);
        fadeBoard.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
        brainCam = Camera.main.GetComponent<CinemachineBrain>();
    }

    IEnumerator StartEvent_co()
    {
        firstCam.Priority = 10;
        fadeBoard.color = Color.black;
        fadeBoard.gameObject.SetActive(true);

        //���̵� ��
        fadeBoard.DOFade(0, 1);

        yield return new WaitForSeconds(1);
        fadeBoard.gameObject.SetActive(false);

        ShowMessage("���� ������ �� �� �ְ� Ÿ���� �Ű��ּ���!", 5f);

        yield return new WaitForSeconds(0.8f);

        float waitTime = Time.time;

        while (Time.time - waitTime < 4.2f)
        {
            if (Input.anyKeyDown)
            {
                MessageCut();
                break;
            }

            yield return null;
        }

        //�߰��� �޽����� �ߴ� �Ǿ��ٸ� �ٷ� ��ɲ��� �����ϴ� ���� ī�޶�� �̵�
        if (IsNoMessage)
            traceVCam.Priority = firstCam.Priority + 1;
        else
        {
            HomeViewCam.Priority = firstCam.Priority + 1;
            yield return new WaitForSeconds(13);
            traceVCam.Priority = HomeViewCam.Priority + 1;
        }


        yield return null;
        yield return new WaitWhile(() => brainCam.IsBlending);

        yield return new WaitForSeconds(1.8f);

        GameStartEvent.Invoke();
    }

    public void LoadSceneAfterFadeOut(int _sceneNum)
    {
        fadeBoard.DOFade(1, 3).OnComplete(() =>
        {
            // �� ����� DoTween �ִϸ��̼��� ���� �Ŀ� ����˴ϴ�.
            SceneManager.LoadScene(_sceneNum);
        });
    }


    [SerializeField, Header("���̵�ƿ�"), Space(10)]
    Image fadeBoard;
    [SerializeField, Header("�޼���")]
    RectTransform message;
    Text text;

    public bool IsNoMessage
    {
        get { return currentTween == null || !currentTween.IsActive(); }
    }

    Tween currentTween = null;

    public void ShowMessage(string _message, float _time = 1)
    {
        // ���� �ִϸ��̼��� ������ �ߴ�
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }

        text.text = _message;

        // �ִϸ��̼� ����
        message.sizeDelta = new Vector2(1920, 0); // ���� ũ��
        message.gameObject.SetActive(true);
        Sequence mySequence = DOTween.Sequence();

        // 0.5�� ���� ũ�⸦ Ű���
        mySequence.Append(message.DOSizeDelta(new Vector2(1920, 126), 0.5f).SetEase(Ease.InOutQuad));

        // _time ���� ���
        mySequence.AppendInterval(_time);

        // 0.5�� ���� ũ�⸦ �ٽ� ���̱�
        mySequence.Append(message.DOSizeDelta(new Vector2(1920, 0), 0.5f).SetEase(Ease.InOutQuad));

        // �ִϸ��̼��� ���� �� ��Ȱ��ȭ
        mySequence.OnComplete(() => message.gameObject.SetActive(false));

        // ���� �ִϸ��̼� ����
        currentTween = mySequence;
    }

    public void MessageCut()
    {
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }

        // �޼��� ��Ȱ��ȭ
        message.gameObject.SetActive(false);
    }


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