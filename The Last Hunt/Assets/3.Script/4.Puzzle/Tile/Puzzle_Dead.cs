using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Dead : Puzzle_Tile
{
    protected override void Awake() => col = GetComponent<BoxCollider>();

    protected override void Start()
    {
        ignoreLayer = 0;
    }

    public override void TileEvent(Puzzle_Horse target)
    {
        //������ �̹� ���� ���¶�� �۵����� �ʽ��ϴ�.
        if (Puzzle_GameManager.instance.IsGameOver) return;

        Puzzle_GameManager.instance.EndGame?.Invoke();
        Puzzle_GameManager.instance.VCamFollowHorse();
        Puzzle_GameManager.instance.GameOver_Horse(target);
    }
}
