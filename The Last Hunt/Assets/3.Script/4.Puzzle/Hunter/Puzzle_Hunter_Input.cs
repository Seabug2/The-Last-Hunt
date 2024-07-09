using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Hunter_Input : MonoBehaviour
{
    //1.캐릭터를 가로세로로 조작할 수 있다.
    //2.타일을 들거나 옮겨 둘 수 있다.

    Puzzle_Hunter_Movement movement;
    Puzzle_Hunter_Carrying carrying;

    private void Awake()
    {
        carrying = GetComponent<Puzzle_Hunter_Carrying>();
        movement = GetComponent<Puzzle_Hunter_Movement>();
    }

    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        //정규화된 수치로 방향 벡터를 설정
        Vector3 dir = new Vector3(x, 0, y).normalized;
        movement.dir = dir;

        //스페이스바를 눌렀을 때 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            carrying.CarryingAction();
        }
    }

}
