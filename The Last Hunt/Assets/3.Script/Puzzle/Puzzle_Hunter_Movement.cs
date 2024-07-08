using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Hunter_Movement : MonoBehaviour
{
    Rigidbody rb;
    Animator anim;
    [SerializeField]
    float speed;
    Vector3 dir = new Vector3();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        dir.Set(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.forward = dir;
    }

    void FixedUpdate()
    {
        rb.position += dir * Time.fixedDeltaTime * speed;
    }
}
