using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Horse_Movement : Puzzle_Movement
{
    public override void Falling()
    {
        base.Falling();
        //울음 소리 재생
        print("말 뒤짐");
    }

    protected override void Start()
    {
        base.Start();
        MoveToNextTile();
    }

    //말이 자기 앞의 타일을 검사하여 그 타일의 코루틴을 실행
    public void MoveToNextTile()
    {
        if (ViewingTile == null)
        {
            Falling();
            return;
        }
        ViewingTile.TileEvent(this);
    }
}
