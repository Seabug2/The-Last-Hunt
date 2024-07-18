using System;

public class Puzzle_Horse : Puzzle_TileChecker
{
    protected override void Start()
    {
        base.Start();
        Puzzle_GameManager.instance.EndGame.AddListener(() =>
        {
            Canlced?.Invoke();
        });

        FallingEvent.AddListener(()=> 
        StartCoroutine(Puzzle_GameManager.instance.GameOver_Horse_co()));
    }

    public Action Canlced = null;

    //���� �ڱ� ���� Ÿ���� �˻��Ͽ� �� Ÿ���� �ڷ�ƾ�� ����
    public void MoveToNextTile()
    {
        if (ViewingTile == null)
        {
            FallingEvent.Invoke();

            //��ɲ� ȭ���� �ִϸ��̼� ���
            //��ɲ� ���� ����
            //���� ���� �̺�Ʈ

            return;
        }
        ViewingTile.TileEvent(this);
    }
}
