using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Toggle : MonoBehaviour
{
    [SerializeField]
    GameObject TargetUI;

    private void Awake()
    {
        TargetUI.SetActive(false);
    }

    public void Toggle()
    {
        print("Ŭ��");
        TargetUI.SetActive(!TargetUI.activeSelf);
    }
}
