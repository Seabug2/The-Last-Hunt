using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Carriage_Movement : MonoBehaviour
{
    [SerializeField]
    Transform target;
    Rigidbody rb;
    [SerializeField]
    float speed = 1;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        transform.LookAt(target.position);
        rb.position = Vector3.Lerp(rb.position, target.position,Time.fixedDeltaTime * speed);
    }
}
