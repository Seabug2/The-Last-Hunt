using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Hunter_Movement : MonoBehaviour
{
    Rigidbody rb;
    Animator anim;
    Puzzle_Hunter_Input input;

    /// <summary>
    /// 이동속도
    /// </summary>
    [SerializeField]
    float moveSpeed = 1;
    /// <summary>
    /// 회전속도
    /// </summary>
    [SerializeField]
    float rotSpeed = 1;

    [HideInInspector]
    public Vector3 dir = new Vector3();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        input = GetComponent<Puzzle_Hunter_Input>();
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

        if (dir != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(dir);
            rb.transform.rotation = Quaternion.Slerp(rb.transform.rotation, newRotation, Time.deltaTime * rotSpeed);
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + dir * Time.fixedDeltaTime * moveSpeed);
    }

    private void LateUpdate()
    {
        //바닥에 Lay를 쏴서 아무것도 없으면 땅에 떨어집니다.
        if (Physics.OverlapSphere(transform.position, .2f,1 << LayerMask.NameToLayer("Water")).Length == 0)
        {
            //더 이상 조작 불가
            input.enabled = false;
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.None;
            anim.SetTrigger("Falling");

            this.enabled = true;

            //dir = Vector3.zero;
            //rb.velocity = Vector3.zero;
        }
    }
}
