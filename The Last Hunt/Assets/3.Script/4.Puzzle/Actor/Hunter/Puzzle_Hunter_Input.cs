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

        //스페이스바를 눌렀을 때 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            carrying.CarryingAction();
        }
    }
}
