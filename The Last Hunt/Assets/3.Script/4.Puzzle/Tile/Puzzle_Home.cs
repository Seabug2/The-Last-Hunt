using System.Collections;
using UnityEngine;

public class Puzzle_Home : Puzzle_Tile
{
    protected override void Awake() => col = GetComponent<BoxCollider>();

    protected override void Start()
    {
        ignoreLayer = 0;
    }

    public override void TileEvent(Puzzle_Horse target)
    {
        Puzzle_GameManager.instance.GameClear();
    }
}
