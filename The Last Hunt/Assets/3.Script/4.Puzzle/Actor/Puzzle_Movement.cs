using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Movement : MonoBehaviour
{

    protected Rigidbody rb;

    [SerializeField,Header("타일 레이어"), Space(10)]
    protected LayerMask tileLayer;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        tileLayer = Puzzle_GameManager.instance.TileLayer;
    }

    /// <summary>
    ///떨어졌을 때 처리
    /// </summary>
    public virtual void Falling() {
        rb.constraints = RigidbodyConstraints.None;
        rb.isKinematic = false;
        rb.useGravity = true;
        enabled = false;
    }

    [SerializeField]
    float forwardRange;

    /// <summary>
    /// 현재 검출하고 있는 위치를 3x3 그리드로 설정
    /// </summary>
    public Vector3 ForwardPosition
    {
        get
        {
            Vector3 origin = transform.position + transform.forward * forwardRange;
            float x = Mathf.Round(origin.x / 3) * 3;
            float z = Mathf.Round(origin.z / 3) * 3;
            return new Vector3(x, 0, z);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(FrontRay);
    }

    /// <summary>
    /// 현재 보고있는 타일
    /// </summary>
    public Puzzle_Tile ViewingTile
    {
        get
        {
            //Raycast를 해서 타일을 가져옵니다.
            if (Physics.Raycast(FrontRay, out RaycastHit hit, Mathf.Infinity, tileLayer))
            {
                return hit.transform.GetComponent<Puzzle_Tile>();
            }
            return null;
        }
    }
}
