using UnityEngine;

public class Puzzle_Hunter_Movement : MonoBehaviour
{
    Rigidbody rb;
    Puzzle_TileChecker tileChecker;

    /// <summary>
    /// �̵��ӵ�
    /// </summary>
    [SerializeField, Header("�̵� �ӵ��� ȸ�� �ӵ�"), Space(10)]
    protected float moveSpeed = 1;
    /// <summary>
    /// ȸ���ӵ�
    /// </summary>
    [SerializeField]
    protected float rotSpeed = 1;
    /// <summary>
    /// �̵��� ����
    /// </summary>
    [HideInInspector]
    public Vector3 dir = Vector3.zero;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        tileChecker = GetComponent<Puzzle_TileChecker>();
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector3.zero;

        if (dir != Vector3.zero)
        {
            transform.position += Time.fixedDeltaTime * moveSpeed * dir;
            Quaternion targetRotation = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotSpeed);
        }

        //ȣ�� ������ ��Ȯ�ϰ� �ϱ� ���Ͽ�
        //ĳ���� �̵� ���� �ٴ� �˻縦 �� �� �ְ�
        //tileChecker�� ����
        if (!tileChecker.IsGrounded)
        {
            print("�÷��̾� ������");
            tileChecker.Falling();
            GetComponent<Puzzle_Hunter>().GameOver();
        }
    }
}
