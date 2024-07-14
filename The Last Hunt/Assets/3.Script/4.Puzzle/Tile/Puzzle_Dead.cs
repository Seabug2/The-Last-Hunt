using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Dead : Puzzle_Tile
{
    protected override void Awake() => col = GetComponent<BoxCollider>();

    protected override void Start()
    {
        ignoreLayer = 0;
    }

    public override void TileEvent(Puzzle_Horse target)
    {
        print("말이 잘못된 타일을 밟았습니다.");

        Rigidbody hRb = target.GetComponent<Rigidbody>();
        hRb.AddForce((-target.transform.forward + Vector3.up) * 10f, ForceMode.VelocityChange);
        hRb.AddTorque(-transform.right * 180f, ForceMode.VelocityChange);
    }
}
