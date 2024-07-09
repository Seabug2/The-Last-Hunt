using System.Collections;
using UnityEngine;

public class Puzzle_Road : Puzzle_Tile
{
    private void LateUpdate()
    {
        if (IsHolding)
        {
            print(name + "들고 있는 중");
            Overlap(IsOverlapping()) ;
        }
    }
}
