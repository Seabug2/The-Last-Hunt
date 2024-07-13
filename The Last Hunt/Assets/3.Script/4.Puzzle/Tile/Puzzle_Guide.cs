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
    /// <param name="IsVisible">�÷��̾� ���� Ÿ���� �����Ѵٸ�, true �������� �ʴ� �ٸ� false</param>
    public void SetInvisible(bool IsVisible)
    {
        rend.enabled = IsVisible;
    }

    private void FixedUpdate()
    {
        //Ÿ�ϸ� �˻�
        Collider[] cols = Physics.OverlapBox(transform.position, Vector3.one * range, Quaternion.identity, ignoreLayer);
        if (cols.Length == 0) return;
        else if (cols[0].TryGetComponent(out Puzzle_Tile tile))
        {
            Overlap(tile.IsOverlapping());
        }
    }
}
