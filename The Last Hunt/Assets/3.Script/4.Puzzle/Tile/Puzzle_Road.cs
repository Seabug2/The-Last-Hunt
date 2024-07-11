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
    [SerializeField, Tooltip("0 : 동 / 1 : 서 / 2 : 남 / 3 : 북")] bool[] gate = new bool[4];
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
            print(name + "들고 있는 중");
            Overlap(IsOverlapping());
        }
    }

    /// <summary>
    /// 말이 하나의 타일을 이동하는데 걸리는 시간
    /// </summary>
    const float duration = 5f;

    public override void TileEvent(Puzzle_Horse_Movement target)
    {
        StartMoveHorse(target);
    }

    public void StartMoveHorse(Puzzle_Horse_Movement target)
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

    IEnumerator MoveHorse_co(Puzzle_Horse_Movement target)
    {
        Transform horse = target.transform;

        print(name + " : 말 올라옴");

        Vector3 startPos = Vector3.zero;
        Vector3 endPos = Vector3.zero;

        int enteringDirection = -1;

        //말이 동쪽에서 들어옴
        if (horse.position.x > transform.position.x)
        {
            if (gate[0]) enteringDirection = 0;
        }
        //말이 서쪽에서 들어옴
        else if (horse.position.x < transform.position.x)
        {
            if (gate[1]) enteringDirection = 1;
        }
        //말이 남쪽에서 들어옴
        else if (horse.position.z < transform.position.z)
        {
            if (gate[2]) enteringDirection = 2;
        }
        //말이 북쪽에서 들어옴
        else if (horse.position.z > transform.position.z)
        {
            if (gate[3]) enteringDirection = 3;
        }

        if (enteringDirection == -1)
        {
            target.Falling();
            yield break;
        }

        for (int i = 0; i < 4; i++)
        {
            if (enteringDirection == i)
            {
                startPos = transform.position + gatePosition[i];

                for (int j = 0; j < 4; j++)
                {

                    if (gate[j] && j != i)
                    {
                        endPos = transform.position + gatePosition[j];
                    }
                }
                break;
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
