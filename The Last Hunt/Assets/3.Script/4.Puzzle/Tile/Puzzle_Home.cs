using System.Collections;
using UnityEngine;

public class Puzzle_Home : Puzzle_Tile
{
    protected override void Awake() => col = GetComponent<BoxCollider>();

    protected override void Start()
    {
        ignoreLayer = 0;
    }

    public override void OnStepped(Puzzle_Horse target)
    {
        //게임이 끝났으면 타일 이벤트 발생 안 함
        if (Puzzle_GameManager.instance.IsGameOver) return;

        StartCoroutine(Puzzle_GameManager.instance.GameClear_co());
    }
}
