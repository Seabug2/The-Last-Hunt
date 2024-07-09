using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Hunter_Movement : MonoBehaviour
{
    Rigidbody rb;
    Animator anim;

    [SerializeField]
    float speed;

    public Vector3 dir = new Vector3();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        //정규화된 수치로 방향 벡터를 설정
        dir = new Vector3(x, 0, y).normalized;

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
            rb.transform.rotation = newRotation;
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + dir * Time.fixedDeltaTime * speed);
    }
}
