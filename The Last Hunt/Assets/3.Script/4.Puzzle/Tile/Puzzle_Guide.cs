using UnityEngine;

public class Puzzle_Guide : Puzzle_Tile
{
    protected override void Start()
    {
        base.Start();
        Holding();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="IsVisible">플레이어 앞의 타일이 존재한다면, true 존재하지 않는 다면 false</param>
    public void SetInvisible(bool IsVisible)
    {
        rend.enabled = IsVisible;
    }

    private void FixedUpdate()
    {
        //타일만 검사
        Collider[] cols = Physics.OverlapBox(transform.position, Vector3.one * range, Quaternion.identity, ignoreLayer);
        if (cols.Length == 0) return;
        else if (cols[0].TryGetComponent(out Puzzle_Tile tile))
        {
            Overlap(tile.IsOverlapping());
        }
    }
}
