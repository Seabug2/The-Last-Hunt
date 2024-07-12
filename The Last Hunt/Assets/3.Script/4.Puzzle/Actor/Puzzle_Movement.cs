using UnityEngine;

public class Puzzle_Movement : MonoBehaviour
{
    protected Rigidbody rb;

    //[SerializeField,Header("Ÿ�� ���̾�"), Space(10)]
    protected LayerMask tileLayer;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void Start()
    {
        tileLayer = Puzzle_GameManager.instance.TileLayer;
    }

    /// <summary>
    ///�������� �� ó��
    /// </summary>
    public virtual void Falling()
    {
        rb.constraints = RigidbodyConstraints.None;
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.AddForce(transform.forward, ForceMode.VelocityChange);
        enabled = false;
        Puzzle_GameManager.instance.GameOverEvent?.Invoke();
    }

    [SerializeField, Header("Ÿ���� ������ ���� �Ÿ�"), Space(10)]
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
            return new Vector3(x, 0 //transform.position .y
                , z);
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

#if UNITY_EDITOR
    protected virtual void OnDrawGizmos()
    {
        //���� Ÿ�� �˻�
        Gizmos.color = Color.red;
        Gizmos.DrawRay(FrontRay);
    }
#endif

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
