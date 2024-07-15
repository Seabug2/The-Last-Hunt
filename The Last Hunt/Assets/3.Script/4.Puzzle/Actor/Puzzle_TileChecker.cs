using UnityEngine;

public class Puzzle_TileChecker : MonoBehaviour
{
    //[SerializeField,Header("타일 레이어"), Space(10)]
    protected LayerMask tileLayer;
    float gridSize = 3;

    protected virtual void Start()
    {
        tileLayer = Puzzle_GameManager.instance.TileLayer;
        gridSize = Puzzle_GameManager.instance.tileSize;
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

    //떨어저도 게임오버
    public virtual void Falling()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;
        rb.isKinematic = false;
        rb.useGravity = true;

        enabled = false;

        //더 이상 조작 불가
        //anim.SetTrigger("Falling");
        //GetComponent<Puzzle_Hunter_Input>().enabled = false;
        //GetComponent<Puzzle_Hunter_TileAction>().enabled = false;
        //rb.AddForce(dir.normalized, ForceMode.Impulse);
        //rb.AddForce(transform.forward, ForceMode.VelocityChange);
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
