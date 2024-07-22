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
    [SerializeField] private GameObject GameOver;
    [SerializeField] private Text GameOverScore_Text;

    [SerializeField] private Slider BowCharge;
    [SerializeField] private PlayerController_Shoot player;

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
    private int TotalKill_Score; // Remember to change to Float

    private void Awake()
    {
        DeerKill_Score = 0;
        ReindeerKill_Score = 0;
        BoarKill_Score = 0;
        WolfKill_Score = 0;
        TotalKill_Score = 0;
        GameClear.SetActive(false);
        GameOver.SetActive(false);
    }

    private void Start()
    {
        ShowMessage(Header_Text.text);
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
        // Save result to JSON
        if (TotalKill_Score >= 1000)
        {
            GameClear.SetActive(true);
            GameClearScore_Text.text = string.Format("Score : {0}", TotalKill_Score);
        }
        else
        {
            GameOver.SetActive(true);
            GameOverScore_Text.text = string.Format("Score : {0}", TotalKill_Score);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("1.Title");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
