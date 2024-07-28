using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UIController_Shoot : MonoBehaviour
{
    [SerializeField] private Text DeerKill_Text;
    [SerializeField] private Text ReindeerKill_Text;
    [SerializeField] private Text BoarKill_Text;
    [SerializeField] private Text WolfKill_Text;
    [SerializeField] private Text TotalKill_Text;

    [SerializeField] private RectTransform Message;
    [SerializeField] private Text Header_Text;

    [SerializeField] private GameObject GameClear;
    [SerializeField] private Text GameClearScore_Text;
    [SerializeField] private GameObject NextChapter;
    [SerializeField] private GameObject GameOver;
    [SerializeField] private Text GameOverScore_Text;

    [SerializeField] private Slider BowCharge;
    [SerializeField] private PlayerController_Shoot player;

    [SerializeField] private GameObject wind;
    [SerializeField] private Text windText;

    [SerializeField] private Image fade;

    public bool isNoMessage
    {
        get
        {
            return currentTween == null || !currentTween.IsActive();
        }
    }

    Tween currentTween = null;

    private int DeerKill_Score;
    private int ReindeerKill_Score;
    private int BoarKill_Score;
    private int WolfKill_Score;
    public int TotalKill_Score; // Change to Float?

    private void Awake()
    {
        Time.timeScale = 0;
        StartCoroutine(Fade_co(true));
        DeerKill_Score = 0;
        ReindeerKill_Score = 0;
        BoarKill_Score = 0;
        WolfKill_Score = 0;
        TotalKill_Score = 0;
        GameClear.SetActive(false);
        GameOver.SetActive(false);
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1.5f);
        ShowMessage(Header_Text.text);
        Time.timeScale = 1;
    }

    private void Update()
    {
        BowCharge.maxValue = 5f;
        if (player.isDraw)
        {
            BowCharge.value = Time.time - player.drawTime;
        }
        else
        {
            BowCharge.value = 0;
        }

        float windDelta = Vector3.SignedAngle(player.transform.forward, Wind_Shoot.windDir, player.transform.up);
        Quaternion windSpin = Quaternion.Euler(0, 0, -windDelta);
        wind.transform.rotation = windSpin;
        windText.text = string.Format("{0:#.0} m/s", Wind_Shoot.windStr * 200);
    }

    private IEnumerator Fade_co(bool isFadeIn)
    {
        fade.gameObject.SetActive(true);
        if (isFadeIn)
        {
            for (float i = 2; i >= 0; i -= Time.deltaTime)
            {
                fade.color = new Color(0, 0, 0, i);
                yield return null;
            }
        }
        else
        {
            Debug.Log("FadeOut");
            Time.timeScale = 1;
            for (float i = 0; i <= 2; i += Time.deltaTime)
            {
                fade.color = new Color(0, 0, 0, i);
                print(Time.deltaTime);
                print(i);
                yield return null;
            }

        }
        fade.gameObject.SetActive(false);
    }

    public void ShowMessage(string message, float time = 1)
    {
        // Abort if prior animation exists
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }
        Header_Text.text = message;

        // Set animation
        Message.sizeDelta = new Vector2(1920, 0);
        Message.gameObject.SetActive(true);
        Sequence headerSequence = DOTween.Sequence();

        // Expand for 0.4 seconds
        headerSequence.Append(Message.DOSizeDelta(new Vector2(1920, 126), 0.4f).SetEase(Ease.InOutQuad));
        // Hold for {time}
        headerSequence.AppendInterval(time);
        // Reduce for 0.4 seconds
        headerSequence.Append(Message.DOSizeDelta(new Vector2(1920, 0), 0.4f).SetEase(Ease.InOutQuad));
        // Deactivate after animation
        headerSequence.OnComplete(() => Message.gameObject.SetActive(false));
        // Save animation
        currentTween = headerSequence;
    }

    public void MessageCut()
    {
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }
        Message.gameObject.SetActive(false);
    }

    public void AddKill(string species, int score)
    {
        switch (species)
        {
            case "Deer":
                DeerKill_Score++;
                TotalKill_Score += score;
                DeerKill_Text.text = string.Format("Deer : {0}", DeerKill_Score);
                break;
            case "Reindeer":
                ReindeerKill_Score++;
                TotalKill_Score += score;
                ReindeerKill_Text.text = string.Format("Reindeer : {0}", ReindeerKill_Score);
                break;
            case "Boar":
                BoarKill_Score++;
                TotalKill_Score += score;
                BoarKill_Text.text = string.Format("Boar : {0}", BoarKill_Score);
                break;
            case "Wolf":
                WolfKill_Score++;
                TotalKill_Score += score;
                WolfKill_Text.text = string.Format("Wolf : {0}", WolfKill_Score);
                break;
        }
        TotalKill_Text.text = string.Format("Total : {0:#,##0}", TotalKill_Score);
    }

    public void ResultScreen()
    {
        Time.timeScale = 0;
        if (TotalKill_Score >= 3000)
        {
            if (!GameManager.instance.IsStoryMode)
            {
                NextChapter.SetActive(false);
            }
            GameClear.SetActive(true);
            GameClearScore_Text.text = string.Format("Score : {0:#,##0}", TotalKill_Score);
            GameManager.instance.currentGameScore[0] = TotalKill_Score;
            if(GameManager.instance.IsNewHighScore(0, TotalKill_Score))
            {
                Debug.Log("최고 기록 갱신!");
            }
        }
        else
        {
            GameOver.SetActive(true);
            GameOverScore_Text.text = string.Format("Score : {0:#,##0}", TotalKill_Score);
        }
    }

    public void NextCh()
    {
        StartCoroutine(NextCh_co());
    }

    private IEnumerator NextCh_co()
    {
        StartCoroutine(Fade_co(false));
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Restart()
    {
        StartCoroutine(Restart_co());
    }

    private IEnumerator Restart_co()
    {
        Time.timeScale = 1.0f;
        DOTween.KillAll();
        StartCoroutine(Fade_co(false));
        yield return new WaitForSeconds(1f);
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void MainMenu()
    {
        StartCoroutine(MainMenu_co());
    }

    private IEnumerator MainMenu_co()
    {
        StartCoroutine(Fade_co(false));
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
