using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Horse_Movement : Puzzle_Movement
{
    public override void Falling()
    {
        base.Falling();
        //���� �Ҹ� ���
        print("�� ����");
    }

    private void Start()
    {
        MoveToNextTile();
        //Invoke("MoveToNextTile",0.1f);
    }

    //���� �ڱ� ���� Ÿ���� �˻��Ͽ� �� Ÿ���� �ڷ�ƾ�� ����
    public void MoveToNextTile()
    {
        if(ViewingTile == null)
        {
            Falling();
            return;
        }
        ViewingTile.TileEvent(this);
    }
}
