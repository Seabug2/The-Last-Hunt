using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Hunter_Movement : Puzzle_Movement
{
    Animator anim;

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

        if (dir != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(dir);
            rb.transform.rotation = Quaternion.Slerp(rb.transform.rotation, newRotation, Time.deltaTime * rotSpeed);
        }
    }

    private void FixedUpdate()
    {
        rb.position += moveSpeed * Time.fixedDeltaTime * dir;
    }

    protected override void Falling()
    {
        base.Falling();
        //더 이상 조작 불가
        anim.SetTrigger("Falling");
        GetComponent<Puzzle_Hunter_Input>().enabled = false;
        GetComponent<Puzzle_Hunter_Carrying>().enabled = false;
    }
}
