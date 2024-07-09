using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Movement : MonoBehaviour
{

    /// <summary>
    /// 이동속도
    /// </summary>
    [SerializeField]
    protected float moveSpeed = 1;
    /// <summary>
    /// 회전속도
    /// </summary>
    [SerializeField]
    protected float rotSpeed = 1;

    protected Rigidbody rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    [HideInInspector]
    public Vector3 dir = new Vector3();

    /// <summary>
    ///떨어졌을 때 처리
    /// </summary>
    protected virtual void Falling() {
        rb.AddForce(dir.normalized, ForceMode.Impulse);
    }

    /// <summary>
    /// x = 검출할 앞 쪽 거리
    /// </summary>
    [SerializeField]
    float checkRange = 1f;

    public Vector3 FloorCheckPosition
    {
        get
        {
            return transform.position + transform.forward * checkRange;
        }
    }

    [SerializeField, Header("타일 레이어"), Space(10)]
    LayerMask tileLayer;

    private void LateUpdate()
    {
        //발 아래에 sphere 영역만큼을 검출하여 타일이 없다면 떨어집니다.
        if (Physics.OverlapSphere(FloorCheckPosition, .1f, tileLayer).Length == 0)
        {
            rb.constraints = RigidbodyConstraints.None;
            rb.isKinematic = false;
            rb.useGravity = true;
            enabled = false;
            Falling();
        }
    }

    private void OnDrawGizmos()
    {
        //발 아래에 타일이 있는 지를 확인하는 영역
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(FloorCheckPosition, .1f);
    }

    Ray Ray
    {
        get
        {
            return new Ray(FloorCheckPosition, Vector3.down);
        }
    }

    public Puzzle_Tile MyOnboardTile
    {
        get
        {
            //내 위치에서 아래 방향으로 길이가 1인 Ray를 발사하여 검출된 타일을 반환
            if (Physics.Raycast(Ray, out RaycastHit hit, tileLayer))
            {
                return hit.transform.GetComponent<Puzzle_Tile>();
            }
            else
            {
                return null;
            }
        }
    }
}
