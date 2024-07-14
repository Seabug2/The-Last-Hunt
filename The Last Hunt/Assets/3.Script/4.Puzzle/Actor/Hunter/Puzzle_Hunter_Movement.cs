using UnityEngine;

public class Puzzle_Hunter_Movement : MonoBehaviour
{
    Rigidbody rb;
    Puzzle_TileChecker tileChecker;

    /// <summary>
    /// 이동속도
    /// </summary>
    [SerializeField, Header("이동 속도와 회전 속도"), Space(10)]
    protected float moveSpeed = 1;
    /// <summary>
    /// 회전속도
    /// </summary>
    [SerializeField]
    protected float rotSpeed = 1;
    /// <summary>
    /// 이동할 방향
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
            Vector3 newPosition = rb.position + Time.fixedDeltaTime * moveSpeed * dir;
            rb.MovePosition(newPosition);

            Quaternion targetRotation = Quaternion.LookRotation(dir, Vector3.up);
            Quaternion newRotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * rotSpeed);
            rb.MoveRotation(newRotation);
        }

        //호출 시점을 정확하게 하기 위하여
        //캐릭터 이동 직후 바닥 검사를 할 수 있게
        //tileChecker를 참조
        if (!tileChecker.IsGrounded)
        {
            print("플레이어 떨어짐");
            tileChecker.Falling();
            GetComponent<Puzzle_Hunter>().GameOver();
        }
    }
}
