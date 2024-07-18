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

    [SerializeField, Header("Ÿ�� ���̾�"), Space(10)]
    LayerMask tileLayer;
    public LayerMask TileLayer => tileLayer;

    /// <summary>
    /// ���� ���۽� �ó׸ӽ� ī�޶�
    /// </summary>
    [SerializeField, Header("�ó׸ӽ� ī�޶�"), Space(10)]
    CinemachineBlendListCamera HomeViewCam;
    /// <summary>
    /// �� ������ ����Ǹ� ��ȯ�Ǵ� �ó׸ӽ� ī�޶�
    /// follow�� ��ɲ��� �⺻
    /// ���� �״� �̺�Ʈ��� ���� ����
    /// </summary>
    [SerializeField]
    CinemachineVirtualCamera traceVCam;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    CinemachineVirtualCamera lookAtHunterVCam;
    /// <summary>
    /// ���� ī�޶�
    /// </summary>
    CinemachineBrain brainCam;

    /// <summary>
    /// �� ������ ����Ǵ� ����
    /// </summary>
    public UnityEvent GameStartEvent;

    /// <summary>
    /// (����, ���� �������)������ ������ ����
    /// </summary>
    public UnityEvent EndGame;

    /// <summary>
    /// �� ������ ����Ǵ� ����
    /// </summary>
    public UnityEvent GameClearEvent;


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
        yield return StartCoroutine(Fade_co(false, 3.5f));

        ShowMessage("Ÿ���� �Ű� ���� ������ �� �� �ְ� ���弼��", 3f);
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

        HomeViewCam.m_Instructions[0].m_Hold = 0;

        //�߰��� �޽����� �ߴ� �Ǿ��ٸ� �ٷ� ��ɲ��� �����ϴ� ���� ī�޶�� �̵�
        if (IsNoMessage)
            traceVCam.Priority = HomeViewCam.Priority + 1;
        else
        {
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

        yield return null;
        yield return new WaitWhile(() => brainCam.IsBlending);
        GameStartEvent.Invoke();
    }

    /// <summary>
    /// �ó׸ӽ��� ���� ����� ���� �ٲ۴�
    /// </summary>
    public void VCamFollowHorse()
    {
        traceVCam.Follow = FindObjectOfType<Puzzle_Horse>().transform;
    }
    /// <summary>
    /// ��ɲ��� �� �����̿��� ���ϴ�.
    /// </summary>
    public void LookAtHunter()
    {
        //�÷��̾� �ó� ��
        lookAtHunterVCam.Priority = brainCam.ActiveVirtualCamera.Priority + 1;
    }

    /// <summary>
    /// ���� �װų� ������ �� ����
    /// </summary>
    public IEnumerator GameOver_Horse_co()
    {
        yield return new WaitForSeconds(1f);
        Destroy(traceVCam.GetCinemachineComponent<CinemachineTransposer>());
        yield return new WaitForSeconds(1.3f);

        lookAtHunterVCam.Priority = brainCam.ActiveVirtualCamera.Priority + 1;
        yield return null;
        yield return new WaitWhile(() => brainCam.IsBlending);

        //�÷��̾� �ִϸ��̼� ���
        FindObjectOfType<Puzzle_Hunter>().Anim.SetTrigger("Failed");

        yield return new WaitForSeconds(1.2f);

        //�޼��� ���
        ShowMessage("�̷� ��û�� �� ������...!", 3.5f);

        yield return new WaitForSeconds(3.5f);
        yield return StartCoroutine(Fade_co(true, 3f));

        //���� Ȱ��ȭ�� ���� �ٽ� ����
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public IEnumerator GameOver_Hunter_co()
    {
        yield return new WaitForSeconds(2.3f);
        yield return StartCoroutine(Fade_co(true, 3f));

        //�޼��� ���
        ShowMessage("����... �ٽ� �� �� �غ���...", 5f);
        yield return new WaitForSeconds(6);

        currentTween = null;

        //���� Ȱ��ȭ�� ���� �ٽ� ����
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

    [SerializeField, Header("���� ��"), Space(10)]
    string nextSceneName;

    public IEnumerator GameClear_co()
    {
        EndGame?.Invoke();
        VCamFollowHorse();

        ShowMessage("���� Ŭ����!", 5f);
        yield return new WaitForSeconds(5);

        LookAtHunter();
        GameClearEvent?.Invoke();
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(Fade_co(true, 3f));

        SceneManager.LoadScene(0);
        //SceneManager.LoadScene(nextSceneName);
    }


    #region �޽��� ���
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
        mySequence.Append(message.DOSizeDelta(new Vector2(1920, 126), 0.35f).SetEase(Ease.InOutQuad));

        // _time ���� ���
        mySequence.AppendInterval(_time);

        // 0.5�� ���� ũ�⸦ �ٽ� ���̱�
        mySequence.Append(message.DOSizeDelta(new Vector2(1920, 0), 0.35f).SetEase(Ease.InOutQuad));

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
    #endregion
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