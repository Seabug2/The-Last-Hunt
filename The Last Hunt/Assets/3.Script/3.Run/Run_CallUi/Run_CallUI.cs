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
   
    //�÷��̾ ����� ���
    public void CallEndUI()
    {
        //�ΰ��� �б� (2��(120��)�� �Ѱ�°� �ƴѰ�)
        //���丮������� �ƴ��� boo��üũ
        
    }
}
