using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Hunter_Movement : Puzzle_Movement
{
    Animator anim;

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

    protected new void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical"))
        {
            anim.SetBool("isMoving", true);
        }
        else if (!Input.GetButton("Horizontal") && !Input.GetButton("Vertical"))
        {
            anim.SetBool("isMoving", false);
        }
    }

    private void FixedUpdate()
    {
        if (dir != Vector3.zero)
        {
            transform.position += Time.fixedDeltaTime * moveSpeed * dir;
            Quaternion targetRotation = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotSpeed);
        }

        //발 아래에 sphere 영역만큼을 검출하여 타일이 없다면 떨어집니다.
        if (Physics.OverlapBox(transform.position, floorCheckerSize, Quaternion.identity, tileLayer).Length == 0)
        {
            Falling();
        }
    }

    public override void Falling()
    {
        base.Falling();
        //더 이상 조작 불가
        anim.SetTrigger("Falling");
        GetComponent<Puzzle_Hunter_Input>().enabled = false;
        GetComponent<Puzzle_Hunter_Carrying>().enabled = false;
        rb.AddForce(dir.normalized, ForceMode.Impulse);
    }

    [SerializeField, Header("바닥 감지 영역 크기"), Space(10)]
    Vector3 floorCheckerSize;

#if UNITY_EDITOR
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        //앞의 타일 검사
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, floorCheckerSize);
    }
#endif
}
