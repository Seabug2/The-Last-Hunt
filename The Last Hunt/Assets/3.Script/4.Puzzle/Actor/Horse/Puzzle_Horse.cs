using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Horse : Puzzle_TileChecker
{
    //���� �ڱ� ���� Ÿ���� �˻��Ͽ� �� Ÿ���� �ڷ�ƾ�� ����
    public void MoveToNextTile()
    {
        if (ViewingTile == null)
        {
            FallingEvent?.Invoke();

            //��ɲ� ȭ���� �ִϸ��̼� ���
            //��ɲ� ���� ����
            //���� ���� �̺�Ʈ

            return;
        }
        ViewingTile.TileEvent(this);
    }
}
