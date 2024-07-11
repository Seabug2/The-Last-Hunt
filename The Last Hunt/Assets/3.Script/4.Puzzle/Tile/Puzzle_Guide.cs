using System.Collections;
using UnityEngine;

public class Puzzle_Guide : Puzzle_Tile
{
    protected override void Start()
    {
        base.Start();
        Holding();
    }

    public void SetInvisible(bool IsVisible)
    {
        rend.enabled = IsVisible;
    }

    private void LateUpdate()
    {
        //타일만 검사
        Collider[] cols = Physics.OverlapBox(transform.position, Vector3.one * range, Quaternion.identity, myLayer);
        if (cols.Length == 0) return;
        else if (cols[0].TryGetComponent(out Puzzle_Road road))
        {
            Overlap(road.IsOverlapping());
        }
    }
}
