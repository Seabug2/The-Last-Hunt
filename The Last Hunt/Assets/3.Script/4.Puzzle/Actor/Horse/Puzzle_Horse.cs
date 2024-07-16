using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Horse : Puzzle_TileChecker
{
    [HideInInspector]
    public bool isAlive = true;

    private void Awake()
    {
        isAlive = true;
    }

    //말이 자기 앞의 타일을 검사하여 그 타일의 코루틴을 실행
    public void MoveToNextTile()
    {
        if (ViewingTile == null)
        {
            Falling();

            //사냥꾼 화내는 애니메이션 재생
            //사냥꾼 조작 차단
            //게임 오버 이벤트

            return;
        }
        ViewingTile.TileEvent(this);
    }
}
