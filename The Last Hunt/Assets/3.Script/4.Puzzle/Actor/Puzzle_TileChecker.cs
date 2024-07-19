using UnityEngine;
using UnityEngine.Events;

public class Puzzle_TileChecker : MonoBehaviour
{
    protected LayerMask tileLayer;
    float gridSize = 3;

    /// <summary>
    /// ĳ���Ͱ� �������� ����� �͵�
    /// </summary>
    [Header("ĳ���Ͱ� �������� ����� �̺�Ʈ")]
    public UnityEvent FallingEvent = new UnityEvent();

    void Start()
    {
        tileLayer = Puzzle_GameManager.instance.TileLayer;
        gridSize = Puzzle_GameManager.tileSize;

        FallingEvent.AddListener(() =>
        {
            //������ ���� ���°� �ƴ϶�� EndGame�� ����
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
