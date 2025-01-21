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
        //������ �������� Ÿ�� �̺�Ʈ �߻� �� ��
        if (Puzzle_GameManager.instance.IsGameOver) return;

        StartCoroutine(Puzzle_GameManager.instance.GameClear_co());
    }
}
