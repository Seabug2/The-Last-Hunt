using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Hunter_Input : MonoBehaviour
{
    //1.ĳ���͸� ���μ��η� ������ �� �ִ�.
    //2.Ÿ���� ��ų� �Ű� �� �� �ִ�.

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

        //����ȭ�� ��ġ�� ���� ���͸� ����
        Vector3 dir = new Vector3(x, 0, y).normalized;
        movement.dir = dir;

        //�����̽��ٸ� ������ �� 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            carrying.CarryingAction();
        }
    }

}
