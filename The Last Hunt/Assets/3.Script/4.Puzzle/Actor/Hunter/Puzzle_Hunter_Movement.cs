using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Puzzle_Hunter_TileAction))]
public class Puzzle_Hunter_Movement : MonoBehaviour
{
    Rigidbody rb;
    Puzzle_Hunter_TileAction tileChecker;

    /// <summary>
    /// 이동속도
    /// </summary>
    [SerializeField, Header("이동 속도")]
    protected float moveSpeed = 1;
    
    /// <summary>
    /// 회전속도
    /// </summary>
    [SerializeField, Header("회전 속도")]
    protected float rotSpeed = 1;

    /// <summary>
    /// 이동할 방향
    /// </summary>
    [HideInInspector]
    public Vector3 dir = Vector3.zero;

    float leftEnd = 99;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        tileChecker = GetComponent<Puzzle_Hunter_TileAction>();
        leftEnd = GameObject.Find("+++++Home+++++").transform.position.x;
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector3.zero;

        if (dir != Vector3.zero)
        {
            Vector3 newPosition = rb.position + Time.fixedDeltaTime * moveSpeed * dir;
            Vector3 fixedPosition = new Vector3(Mathf.Clamp(newPosition.x, -4.4f, leftEnd), 0, Mathf.Clamp(newPosition.z, -10.4f, 10.4f));
            rb.MovePosition(fixedPosition);

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
            //게임이 끝난 상태가 아니라면 EndGame를 실행
            if (!Puzzle_GameManager.instance.IsGameOver)
            {
                tileChecker.isFallen = true;
                Puzzle_GameManager.instance.EndGame?.Invoke();
                Puzzle_GameManager.instance.GameOver_Hunter(tileChecker);
            }
        }
    }

    private void OnDisable()
    {
        dir = Vector3.zero;
    }
}
