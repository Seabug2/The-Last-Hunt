using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Dead : Puzzle_Tile
{
    protected override void Start()
    {
        ignoreLayer = 1;
    }

    public override void TileEvent(Puzzle_Horse_Movement target)
    {
        print("말이 잘못된 타일을 밟았습니다.");
        target.Falling();
        //StartGameOver();
        Rigidbody hRb = target.GetComponent<Rigidbody>();
        hRb.AddForce((-target.transform.forward + Vector3.up) * 10f, ForceMode.VelocityChange);
        hRb.AddTorque(-transform.right * 180f, ForceMode.VelocityChange);
    }

    void StartGameOver()
    {
        StopGameOver();
        GameOver = StartCoroutine(GameOver_co());
    }

    void StopGameOver()
    {
        if (GameOver != null)
        {
            StopCoroutine(GameOver);
        }
    }

    Coroutine GameOver;
    IEnumerator GameOver_co()
    {
        yield break;
    }
}
