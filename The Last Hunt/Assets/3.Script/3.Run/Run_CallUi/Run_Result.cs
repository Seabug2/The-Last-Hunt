using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Run_Result : MonoBehaviour
{
    private bool isResult = false;
    GameManager gameManager;
    [SerializeField]
    GameObject _clearUI;
    [SerializeField]
    GameObject _failUI;
    GameObject Timer;
    [SerializeField] Text bestText; 
    Timer timer;
    [SerializeField] Text resultText_clear;
    [SerializeField] Text resultText_notclear;
    [SerializeField] double resultTime;
    [SerializeField] double limitTime;
    UserData data;
    /*
    private void Start()
    {
        
        clearUI = GameObject.Find("Result_isClear").GetComponent<GameObject>();
        failUI = GameObject.Find("Result_isFail").GetComponent<GameObject>();
        timer = GetComponent<Timer>();
        
    }
    public void CallResult()
    {
        if(timer.time >= 120)
        {
            ClearResult();
        }
        else if(timer.time < 120)
        {
            FailResult();
        }
    }
    public void ClearResult()
    {
        clearUI.SetActive(true);
        timer.enabled = false;
        timer.time = resultTime;
        gameManager.currentGameScore[2] = (float)resultTime;
        gameManager.IsNewHighScore(2, (float)resultTime);


    }
    public void FailResult()
    {
        failUI.SetActive(true);
        timer.enabled = false;
        timer.time = resultTime;
        gameManager.currentGameScore[2] = (float)resultTime;
        gameManager.IsNewHighScore(2, (float)resultTime);
    }
    */
    private void Start()
    {
        isResult = false;
        resultText_clear = GetComponent<Text>();
        resultText_notclear = GetComponent<Text>();
        bestText = GetComponent<Text>();
        GameObject _clearUI = GameObject.Find("Result_isClear");
        
        if (_clearUI == null)
        {
            Debug.LogError("Result_isClear GameObject를 찾을 수 없습니다.");
        }

        GameObject _failUI = GameObject.Find("Result_isFail");
        if (_failUI == null)
        {
            Debug.LogError("Result_isFail GameObject를 찾을 수 없습니다.");
        }

        GameObject _timer = GameObject.Find("Timer");
        timer = _timer.GetComponent<Timer>();
        if (_timer == null)
        {
            Debug.LogError("Timer 컴포넌트를 찾을 수 없습니다.");
        }

        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager를 찾을 수 없습니다.");
        }
    }

    public void CallResult()
    {
        if (timer != null && timer.time >= limitTime)
        {
            ClearResult();
        }
        else if (timer != null && timer.time < limitTime)
        {
            FailResult();
        }
    }

    public void ClearResult()
    {
        if (_clearUI != null)
        {
            _clearUI.SetActive(true);
        }

        if (timer != null)
        {
            timer.enabled = false;
            timer.time = resultTime;
        }

        if (gameManager != null && !isResult)
        {
            data.IsCleared[2] = true;
            float previousHighScore = data.score[2];
            gameManager.currentGameScore[2] = (float)resultTime;
            gameManager.IsNewHighScore(2, (float)resultTime);
            resultText_clear.text = resultTime.ToString("F2");
            if (previousHighScore<resultTime)
            {
                bestText.text = "Best";
                bestText.gameObject.SetActive(true);
            }
            isResult = true;
        }
       
    }

    public void FailResult()
    {

        if (_failUI != null && !isResult)
        {
            _failUI.SetActive(true);
        }

        if (timer != null)
        {
            timer.enabled = false;
            timer.time = resultTime;
        }

        if (gameManager != null)
        {
            resultText_clear.text = resultTime.ToString("F2");
            gameManager.currentGameScore[2] = (float)resultTime;
            
        }
    }
}
