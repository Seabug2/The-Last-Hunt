using UnityEngine;
using UnityEngine.Events;

public class Run_PlayerMove : MonoBehaviour
{
    Rigidbody rb;
    Animator anim;
    public Animator Anim => anim;

    [SerializeField] float forwardSpeed = 1f;
    [SerializeField] float horizontalMoveSpeed = 1f;

    public UnityEvent FallingEvent;
    [SerializeField] LayerMask checkingLayer;

    [SerializeField, Header("사망 파티클"), Space(10)]
    GameObject explosion;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// 점프 중일 경우 true를 반환합니다.
    /// </summary>
    public bool isJumping = false;
    //{
    //    get
    //    {
    //        return anim.GetCurrentAnimatorStateInfo(0).IsName("Jump");
    //    }
    //}

    /// <summary>
    /// 슬라이딩 중일 경우 true를 반환합니다.
    /// </summary>
    public bool isSliding = false;
    //{
    //    get
    //    {
    //        return anim.GetCurrentAnimatorStateInfo(0).IsName("Slide");
    //    }
    //}

    private void Start()
    {
        isJumping = false;
        isSliding = false;
    }

    private void FixedUpdate()
    {
        MoveToForward();
    }

    private void MoveToForward()
    {
        Vector3 dir = Vector3.zero;
        if (!isJumping && !isSliding)
        {
            dir = transform.right * Input.GetAxis("Horizontal") * horizontalMoveSpeed;
        }
        dir += transform.forward * forwardSpeed;
        //캐릭터 진행 방향으로 
        rb.position += dir * Time.fixedDeltaTime;

        Ray ray = new Ray(transform.position + Vector3.up, Vector3.down);

        if (!Physics.Raycast(ray, out RaycastHit hit, 2f, checkingLayer) && !isJumping)
        {
            FallingEvent?.Invoke();
            Run_Manager.instance.EndEvent?.Invoke();
        }
    }

    public void TurnLeft()
    {
        transform.Rotate(new Vector3(0f, -90f, 0f));
    }

    public void TurnRight()
    {
        transform.Rotate(new Vector3(0f, 90f, 0f));
    }

    public void Jumping()
    {
        anim.SetTrigger("Jumping");
        isJumping = true;
    }
    public void Sliding()
    {
        anim.SetTrigger("Sliding");
        isSliding = true;
    }

    void IsntJumping()
    {
        isJumping = false;
    }
    void IsntSliding()
    {
        isSliding = false;
    }

    public void Explosion()
    {
        explosion.transform.SetParent(null);
        explosion.SetActive(true);
        Destroy(gameObject);
    }
}
