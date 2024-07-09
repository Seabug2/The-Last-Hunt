using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_CameraController : MonoBehaviour
{
    [SerializeField]
    Transform target;

    Vector3 distOffset;

    void Start()
    {
        distOffset = transform.position - target.position;
    }

    [SerializeField]
    float speed = 1;

    void LateUpdate()
    {
        transform.position = target.position + distOffset;
    }
}
