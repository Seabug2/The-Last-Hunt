using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Run_CallUI : MonoBehaviour
{
    [SerializeField]
    public Button pause;
    //private GameObject pause1;
    public void Start()
    {
        pause = GameObject.Find("Option Button").GetComponent<Button>();
        pause.interactable = false;
    }
    public void CallPause()
    {
        pause.interactable = true;
    }
   
    //플레이어가 사망시 출력
    public void CallEndUI()
    {
        //두가지 분기 (2분(120초)를 넘겼는가 아닌가)
        //스토리모드인지 아닌지 booㅣ체크
        
    }
}
