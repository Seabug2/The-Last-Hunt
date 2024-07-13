using System.Collections;
using UnityEngine;

public class Puzzle_Goal : Puzzle_Tile
{
    protected override void Start()
    {
        ignoreLayer = 0;
    }
    public override void TileEvent(Puzzle_Horse_TileAction target)
    {
        print("집에 도착했습니다!");
        Puzzle_GameManager.instance.GameClearEvent?.Invoke();
    }

    void StartGameOver()
    {
        StopGameOver();
        GameClear = StartCoroutine(GameClear_co());
    }

    void StopGameOver()
    {
        if (GameClear != null)
        {
            StopCoroutine(GameClear);
        }
    }

    Coroutine GameClear;
    IEnumerator GameClear_co()
    {
        yield break;
    }
}
