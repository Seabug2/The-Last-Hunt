using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_TileSpawner : MonoBehaviour
{
    //Ÿ���� �����ϰ� �־����
    [SerializeField, Header("Ÿ��"), Space(10)]
    //�ε�Ÿ��
    GameObject[] road;
    //����Ÿ��
    [SerializeField]
    GameObject dead;

    [Header("�����۰� ��ֹ�"), Space(10)]
    [SerializeField, Header("1 : ���"), Header("0 : ����")]
    GameObject[] items;
    [SerializeField, Header("2 : ����"), Header("1 : ����"), Header("0 : ����")]
    GameObject[] obstacle;

    ////Ÿ���� ũ���?
    //int tileSize = 3;
    
    //private void Start()
    //{
    //    tileSize = Puzzle_GameManager.tileSize;
    //}
}
