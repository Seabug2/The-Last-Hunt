using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Movement : MonoBehaviour
{

    protected Rigidbody rb;

    [SerializeField,Header("Ÿ�� ���̾�"), Space(10)]
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
    ///�������� �� ó��
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
    /// ���� �����ϰ� �ִ� ��ġ�� 3x3 �׸���� ����
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
    /// �÷��̾� ���� Ư�� ��ġ���� Ray�� ����
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
    /// ���� �����ִ� Ÿ��
    /// </summary>
    public Puzzle_Tile ViewingTile
    {
        get
        {
            //Raycast�� �ؼ� Ÿ���� �����ɴϴ�.
            if (Physics.Raycast(FrontRay, out RaycastHit hit, Mathf.Infinity, tileLayer))
            {
                return hit.transform.GetComponent<Puzzle_Tile>();
            }
            return null;
        }
    }
}
