using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Dead : Puzzle_Tile
{
    public override void TileEvent(Puzzle_Horse_Movement target)
    {
        print("���� �߸��� Ÿ���� ��ҽ��ϴ�.");
        target.Falling();
        //StartGameOver();
        target.GetComponent<Rigidbody>().AddForce((-target.transform.forward + Vector3.up) * 10f, ForceMode.VelocityChange);
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
