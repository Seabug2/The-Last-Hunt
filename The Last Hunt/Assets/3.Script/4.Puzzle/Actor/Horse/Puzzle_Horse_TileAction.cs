using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Horse_TileAction : Puzzle_TileChecker
{
    //���� �ڱ� ���� Ÿ���� �˻��Ͽ� �� Ÿ���� �ڷ�ƾ�� ����
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
