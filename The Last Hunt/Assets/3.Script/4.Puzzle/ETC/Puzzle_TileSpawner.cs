using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_TileSpawner : MonoBehaviour
{
    //타일을 저장하고 있어야함
    [SerializeField, Header("타일"), Space(10)]
    //로드타일
    GameObject[] road;
    //데드타일
    [SerializeField]
    GameObject dead;

    [Header("아이템과 장애물"), Space(10)]
    [SerializeField, Header("1 : 곡괭이"), Header("0 : 도끼")]
    GameObject[] items;
    [SerializeField, Header("2 : 늑대"), Header("1 : 바위"), Header("0 : 나무")]
    GameObject[] obstacle;

    ////타일의 크기는?
    //int tileSize = 3;
    
    //private void Start()
    //{
    //    tileSize = Puzzle_GameManager.tileSize;
    //}
}
