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

    //말이 자기 앞의 타일을 검사하여 그 타일의 코루틴을 실행
    public void MoveToNextTile()
    {
        if (ViewingTile == null)
        {
            FallingEvent.Invoke();

            //사냥꾼 화내는 애니메이션 재생
            //사냥꾼 조작 차단
            //게임 오버 이벤트

            return;
        }
        ViewingTile.TileEvent(this);
    }
}
