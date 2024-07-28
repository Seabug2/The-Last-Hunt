using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run_Result : MonoBehaviour
{

    GameManager gameManager;
    [SerializeField]
    GameObject clearUI;
    [SerializeField]
    GameObject failUI;
    
    Timer timer;
    double resultTime;

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
    
}
