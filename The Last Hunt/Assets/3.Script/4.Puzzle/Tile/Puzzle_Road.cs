using System.Collections;
using UnityEngine;

public class Puzzle_Road : Puzzle_Tile
{
    private void LateUpdate()
    {
        if (IsHolding)
        {
            print(name + "��� �ִ� ��");
            Overlap(IsOverlapping()) ;
        }
    }
}
