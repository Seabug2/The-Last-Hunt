using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Movement : MonoBehaviour
{

    /// <summary>
    /// �̵��ӵ�
    /// </summary>
    [SerializeField]
    protected float moveSpeed = 1;
    /// <summary>
    /// ȸ���ӵ�
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
    ///�������� �� ó��
    /// </summary>
    protected virtual void Falling() {
        rb.AddForce(dir.normalized, ForceMode.Impulse);
    }

    /// <summary>
    /// x = ������ �� �� �Ÿ�
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

    [SerializeField, Header("Ÿ�� ���̾�"), Space(10)]
    LayerMask tileLayer;

    private void LateUpdate()
    {
        //�� �Ʒ��� sphere ������ŭ�� �����Ͽ� Ÿ���� ���ٸ� �������ϴ�.
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
        //�� �Ʒ��� Ÿ���� �ִ� ���� Ȯ���ϴ� ����
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
            //�� ��ġ���� �Ʒ� �������� ���̰� 1�� Ray�� �߻��Ͽ� ����� Ÿ���� ��ȯ
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
