using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Horse_Movement : Puzzle_Movement
{
    private void FixedUpdate()
    {
        rb.position += moveSpeed * Time.fixedDeltaTime * transform.forward;
    }

    protected override void Falling()
    {
        base.Falling();
        //울음 소리 재생
    }
}
