using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Horse_Movement : MonoBehaviour
{
    Rigidbody rb;
    Animator anim;
    [SerializeField]
    float speed = 1;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    //private void Start()
    //{
    //    anim.SetBool("Walk",true);
    //}

    //private void FixedUpdate()
    //{
    //    rb.position += transform.forward * Time.fixedDeltaTime * speed;
    //}
}
