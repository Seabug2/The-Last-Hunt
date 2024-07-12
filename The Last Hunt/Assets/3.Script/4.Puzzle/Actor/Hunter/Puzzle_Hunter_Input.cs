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

    public float dead = 1f;
    Vector3 dir;

    private void Update()
    {
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            dir = new Vector3(x, 0, z);
        }
        else
        {
            dir = Vector3.Lerp(dir, Vector3.zero, Time.deltaTime * dead);
        }

        movement.dir = dir;

        //�����̽��ٸ� ������ �� 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            carrying.CarryingAction();
        }
    }
}
