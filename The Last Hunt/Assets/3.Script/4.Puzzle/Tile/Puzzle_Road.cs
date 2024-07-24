using System.Collections;
using UnityEngine;

public class Puzzle_Road : Puzzle_Tile
{
    /// <summary>
    /// 0 = 동
    /// 1 = 서
    /// 2 = 남
    /// 3 = 북
    /// </summary>
    [SerializeField, Header("0 : 동 / 1 : 서 / 2 : 남 / 3 : 북")] bool[] gate = new bool[4];
    /// <summary>
    /// 0 = 동
    /// 1 = 서
    /// 2 = 남
    /// 3 = 북
    /// </summary>
    readonly Vector3[] gatePosition = new Vector3[]
    {
        new Vector3(1.5f,0,0),
        new Vector3(-1.5f,0,0),
        new Vector3(0,0,-1.5f),
        new Vector3(0,0,1.5f)
    };

    private void FixedUpdate()
    {
        if (IsHolding)
        {
            Overlap(IsOverlapping());
        }
    }

    /// <summary>
    /// 말이 하나의 타일을 이동하는데 걸리는 시간
    /// </summary>
    const float duration = 6.9f;

    public override void TileEvent(Puzzle_Horse target)
    {
        StartMoveHorse(target);
    }

    public void StartMoveHorse(Puzzle_Horse target)
    {
        StopMoveHorse();
        MoveHorse = StartCoroutine(MoveHorse_co(target));
    }

    public void StopMoveHorse()
    {
        if (MoveHorse != null)
        {
            StopCoroutine(MoveHorse);
        }
    }

    Coroutine MoveHorse;

    IEnumerator MoveHorse_co(Puzzle_Horse target)
    {
        target.currentTile = this;
        Transform horse = target.transform;
        Vector3 endPos = Vector3.zero;

        int enteringDirection = GetEnteringDirection(horse.position);

        //말이 잘못된 방향으로 들어온 경우
        if (enteringDirection == -1)
        {
            //게임이 이미 끝난 경우에는 실행하지 않음
            if (!Puzzle_GameManager.instance.IsGameOver)
            {
                Puzzle_GameManager.instance.EndGame?.Invoke();
                Puzzle_GameManager.instance.VCamFollowHorse();
                Puzzle_GameManager.instance.GameOver_Horse(target);
            }
            yield break;
        }

        #region
        ////말이 동쪽에서 들어옴
        //if (horse.position.x > transform.position.x)
        //{
        //    if (gate[0]) enteringDirection = 0;
        //}
        ////말이 서쪽에서 들어옴
        //else if (horse.position.x < transform.position.x)
        //{
        //    if (gate[1]) enteringDirection = 1;
        //}
        ////말이 남쪽에서 들어옴
        //else if (horse.position.z < transform.position.z)
        //{
        //    if (gate[2]) enteringDirection = 2;
        //}
        ////말이 북쪽에서 들어옴
        //else if (horse.position.z > transform.position.z)
        //{
        //    if (gate[3]) enteringDirection = 3;
        //}
        #endregion

        Vector3 startPos = transform.position + gatePosition[enteringDirection];
        for (int j = 0; j < 4; j++)
        {
            if (gate[j] && j != enteringDirection)
            {
                endPos = transform.position + gatePosition[j];
                break; // 첫 번째 맞는 게이트를 찾으면 루프를 탈출
            }
        }

        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            float f = elapsedTime / duration;

            Vector3 bezierPoint = CalculateQuadraticBezierPoint(f, startPos, transform.position, endPos);
            horse.LookAt(bezierPoint, Vector3.up);
            horse.position = bezierPoint;
            elapsedTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        horse.position = endPos;

        target.MoveToNextTile();
    }
    int GetEnteringDirection(Vector3 horsePosition)
    {
        Vector3 v = horsePosition - transform.position;
        float enterX = Mathf.RoundToInt(v.x);
        float enterZ = Mathf.RoundToInt(v.z);

        if (enterX > 0 && gate[0]) return 0; // 동쪽
        if (enterX < 0 && gate[1]) return 1; // 서쪽
        if (enterZ < 0 && gate[2]) return 2; // 남쪽
        if (enterZ > 0 && gate[3]) return 3; // 북쪽

        return -1; // 조건에 맞지 않는 경우
    }

    Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0; // (1-t)^2 * p0
        p += 2 * u * t * p1; // 2(1-t)t * p1
        p += tt * p2;        // t^2 * p2

        return p;
    }
#if UNITY_EDITOR
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.cyan;
        for (int i = 0; i < 4; i++)
        {
            if (gate[i])
            {
                Gizmos.DrawSphere(transform.position + gatePosition[i], .2f);
            }
        }
    }
#endif
}
