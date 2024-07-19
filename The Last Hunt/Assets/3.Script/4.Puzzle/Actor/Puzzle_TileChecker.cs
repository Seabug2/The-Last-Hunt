using UnityEngine;
using UnityEngine.Events;

public class Puzzle_TileChecker : MonoBehaviour
{
    protected LayerMask tileLayer;
    float gridSize = 3;

    /// <summary>
    /// 캐릭터가 떨어지면 실행될 것들
    /// </summary>
    [Header("캐릭터가 떨어지면 실행될 이벤트")]
    public UnityEvent FallingEvent = new UnityEvent();

    void Start()
    {
        tileLayer = Puzzle_GameManager.instance.TileLayer;
        gridSize = Puzzle_GameManager.tileSize;

        FallingEvent.AddListener(() =>
        {
            //게임이 끝난 상태가 아니라면 EndGame를 실행
            if (!Puzzle_GameManager.instance.IsGameOver)
            {
                Puzzle_GameManager.instance.EndGame?.Invoke();
            }
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.None;
            rb.isKinematic = false;
            rb.useGravity = true;
        });
    }

    [SerializeField, Header("타일을 감지할 앞쪽 거리"), Space(10)]
    float forwardRange;

    /// <summary>
    /// 현재 검출하고 있는 위치를 3x3 그리드로 설정
    /// </summary>
    public Vector3 ForwardPosition
    {
        get
        {
            Vector3 origin = transform.position + transform.forward * forwardRange;
            float x = Mathf.Round(origin.x / gridSize) * gridSize;
            float z = Mathf.Round(origin.z / gridSize) * gridSize;
            return new Vector3(x, 0 //transform.position .y
                , z);
        }
    }

    /// <summary>
    /// 플레이어 앞의 특정 위치에서 Ray를 생성
    /// </summary>
    Ray FrontRay
    {
        get
        {
            return new Ray(ForwardPosition + Vector3.up, Vector3.down);
        }
    }

    [SerializeField, Header("바닥 감지 영역 크기"), Space(10)]
    Vector3 floorCheckerSize;

    public bool IsGrounded
    {
        get
        {
            //캐릭터의 발 아래에 Tile이 하나라도 있으면 땅 위에 있는 것으로 간주
            return (Physics.OverlapBox(transform.position, floorCheckerSize, Quaternion.identity, tileLayer).Length > 0);
        }
    }

    /// <summary>
    /// 현재 보고있는 타일
    /// </summary>
    public Puzzle_Tile ViewingTile
    {
        get
        {
            //Raycast를 해서 현재 시야에 잡힌 타일을 검출합니다
            if (Physics.Raycast(FrontRay, out RaycastHit hit, Mathf.Infinity, tileLayer))
            {
                return hit.transform.GetComponent<Puzzle_Tile>();
            }
            return null;
        }
    }

   
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        //앞의 타일 검사
        Gizmos.color = Color.red;
        Gizmos.DrawRay(FrontRay);

        //바닥 검사
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, floorCheckerSize);
    }
#endif
}
