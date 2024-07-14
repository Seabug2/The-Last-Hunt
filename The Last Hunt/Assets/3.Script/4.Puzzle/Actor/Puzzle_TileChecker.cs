using UnityEngine;

public class Puzzle_TileChecker : MonoBehaviour
{
    //[SerializeField,Header("Ÿ�� ���̾�"), Space(10)]
    protected LayerMask tileLayer;
    float gridSize = 3;

    protected virtual void Start()
    {
        tileLayer = Puzzle_GameManager.instance.TileLayer;
        gridSize = Puzzle_GameManager.tileSize;
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
            float x = Mathf.Round(origin.x / gridSize) * gridSize;
            float z = Mathf.Round(origin.z / gridSize) * gridSize;
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

    [SerializeField, Header("�ٴ� ���� ���� ũ��"), Space(10)]
    Vector3 floorCheckerSize;


    public bool IsGrounded
    {
        get
        {
            //ĳ������ �� �Ʒ��� Tile�� �ϳ��� ������ �� ���� �ִ� ������ ����
            return (Physics.OverlapBox(transform.position, floorCheckerSize, Quaternion.identity, tileLayer).Length > 0);
        }
    }

    /// <summary>
    /// ���� �����ִ� Ÿ��
    /// </summary>
    public Puzzle_Tile ViewingTile
    {
        get
        {
            //Raycast�� �ؼ� ���� �þ߿� ���� Ÿ���� �����մϴ�
            if (Physics.Raycast(FrontRay, out RaycastHit hit, Mathf.Infinity, tileLayer))
            {
                return hit.transform.GetComponent<Puzzle_Tile>();
            }
            return null;
        }
    }

    //�������� ���ӿ���
    public virtual void Falling()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;
        rb.isKinematic = false;
        rb.useGravity = true;

        enabled = false;

        //�� �̻� ���� �Ұ�
        //anim.SetTrigger("Falling");
        //GetComponent<Puzzle_Hunter_Input>().enabled = false;
        //GetComponent<Puzzle_Hunter_TileAction>().enabled = false;
        //rb.AddForce(dir.normalized, ForceMode.Impulse);
        //rb.AddForce(transform.forward, ForceMode.VelocityChange);
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {        
        //���� Ÿ�� �˻�
        Gizmos.color = Color.red;
        Gizmos.DrawRay(FrontRay);
        
        //�ٴ� �˻�
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, floorCheckerSize);
    }
#endif
}