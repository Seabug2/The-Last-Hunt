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
        print("���� �����߽��ϴ�!");
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